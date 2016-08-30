using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace BEE2
{
    public class PackageFileException : Exception
    {
    }

    public class PackageEntry
    {
        public string FileName;
        public string DirectoryName;
        public string TypeName;
        public UInt32 CRC32;
        public UInt32 Size;
        public UInt32 Offset;
        public UInt16 ArchiveIndex;
        public byte[] SmallData;
    }

    public class PackageFile
    {
        public Dictionary<string, Bitmap> ImageCache = new Dictionary<string,Bitmap>();
        public List<PackageEntry> Entries;
        public string folder;

        private System.Object lockThis = new System.Object();
        private System.Object lockThis2 = new System.Object();

        public string ReadASCIIZ(Stream stream)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(256);
            char c = (char)stream.ReadByte();
            while (c != '\0')
            {
                sb.Append(c);
                c = (char)stream.ReadByte();
            }
            return sb.ToString();
        }
        public UInt32 ReadU32(Stream stream)
        {
            UInt32 r = (UInt32)stream.ReadByte();
            r = r | (((UInt32)stream.ReadByte()) << 8);
            r = r | (((UInt32)stream.ReadByte()) << 16);
            r = r | (((UInt32)stream.ReadByte()) << 24);
            return r;
        }
        public UInt16 ReadU16(Stream stream)
        {
            UInt16 r = (UInt16)stream.ReadByte();
            r = (UInt16)(r | (UInt16)(((UInt16)stream.ReadByte()) << 8));
            return r;
        }

        public void LoadFolder(string path)
        {
            folder = path;
            string filename = path + "\\pak01_dir.vpk";
            using (FileStream f = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                Deserialize(f);
            }
        }

        public PackageFile(string FolderPath)
        {
            VtfLib.vlInitialize();
            LoadFolder(FolderPath);
        }

        public PackageEntry GetEntry(string filename)
        {
            string ext = Path.GetExtension(filename).ToLowerInvariant().Substring(1);
            filename = filename.Replace("/", "\\").ToLowerInvariant();
            bool foundtype = false;
            foreach (PackageEntry entry in Entries)
            {
                if (!entry.TypeName.Equals(ext))
                {
                    if (foundtype) return null;
                    else continue;
                }
                string fullpath = Path.Combine(entry.DirectoryName, Path.ChangeExtension(entry.FileName, entry.TypeName));
                if (fullpath.Equals(filename))
                    return entry;
                else foundtype = true;
            }
            return null;
        }

        // filename is a relative path, eg. materials\models\props_map_editor\palette\cubes.vtf
        // you can use either kind of slash, and it's not case sensitive
        public byte[] GetFileContents(string filename)
        {
            lock (lockThis2)
            {
                string fullpath = Path.Combine(folder, filename.Replace("/", "\\"));
                if (File.Exists(fullpath)) // files in the folder should override those in the package (for us)
                {
                    return File.ReadAllBytes(fullpath);
                }
                else
                {
                    PackageEntry entry = GetEntry(filename);
                    if (entry == null) // if file not in folder or archive
                        return null;
                    if (entry.Size == 0 && entry.SmallData.Length > 0) // if file in archive index
                    {
                        return entry.SmallData;
                    }
                    else // if file in archive
                    {
                        string archiveName = Path.Combine(folder, string.Format("pak01_{0:D3}.vpk", entry.ArchiveIndex));
                        if (!File.Exists(archiveName))
                            return null;
                        using (FileStream f = new FileStream(archiveName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            f.Seek(entry.Offset, SeekOrigin.Begin);
                            byte[] data = new byte[entry.Size];
                            f.Read(data, 0, (int)entry.Size);
                            return data;
                        }
                    } // end if file in archive
                }
            }
        }

        public unsafe Bitmap LoadVTFImage(byte[] lpInput)
        {
            lock (lockThis)
            {
                if (lpInput == null || lpInput.Length == 0)
                    return null;
                uint uiImage;
                VtfLib.vlCreateImage(&uiImage);
                VtfLib.vlBindImage(uiImage);

                try
                {
                    fixed (byte* lpBuffer = lpInput)
                    {
                        if (!VtfLib.vlImageLoadLump(lpBuffer, (uint)lpInput.Length, false))
                        {
                            throw new FormatException(VtfLib.vlGetLastError());
                        }
                    }

                    byte[] lpImageData = new byte[VtfLib.vlImageComputeImageSize(VtfLib.vlImageGetWidth(), VtfLib.vlImageGetHeight(), 1, 1, VtfLib.ImageFormat.ImageFormatBGRA8888)];
                    fixed (byte* lpOutput = lpImageData)
                    {
                        if (!VtfLib.vlImageConvert(VtfLib.vlImageGetData(0, 0, 0, 0), lpOutput, VtfLib.vlImageGetWidth(), VtfLib.vlImageGetHeight(), VtfLib.vlImageGetFormat(), VtfLib.ImageFormat.ImageFormatBGRA8888))
                        {
                            throw new FormatException(VtfLib.vlGetLastError());
                        }
                        Bitmap b = new Bitmap((int)VtfLib.vlImageGetWidth(), (int)VtfLib.vlImageGetHeight(), (int)VtfLib.vlImageGetWidth() * 4, System.Drawing.Imaging.PixelFormat.Format32bppArgb, (IntPtr)lpOutput);
                        Bitmap b2 = new Bitmap(b);
                        b.Dispose();
                        b = null;
                        return b2;
                    }

                }
                finally
                {
                    VtfLib.vlDeleteImage(uiImage);
                }
            }
        }

        public Bitmap GetImage(string filename)
        {
            filename = filename.Replace("/", "\\").ToLowerInvariant();
            if (!ImageCache.ContainsKey(filename))
                ImageCache[filename]=LoadVTFImage(GetFileContents(filename));
            return ImageCache[filename];
        }

        public Bitmap GetPaletteImage(string filename)
        {
            filename = Path.ChangeExtension(filename, ".vtf").Replace("/", "\\").ToLowerInvariant();
            if (!ImageCache.ContainsKey(filename))
            {
                ImageCache[filename] = LoadVTFImage(GetFileContents(Path.Combine("materials\\models\\props_map_editor", filename)));
            }
            return ImageCache[filename];
        }

        public void Deserialize(Stream stream)
        {
            List<PackageEntry> entries = new List<PackageEntry>();

            if (ReadU32(stream) != 0x55AA1234) //  Left 4 Dead, before June 25, 2009 update
            {
                stream.Seek(-4, SeekOrigin.Current);
            }
            else // Left 4 Dead update, Left 4 Dead 2, Alien Swarm, Portal 2, Source Filmmaker, Dota 2
            {
                uint version = ReadU32(stream);
                uint indexSize = ReadU32(stream);

                if (version >= 2) // Counter-Strike: Global Offensive
                {
                    uint zero = ReadU32(stream); // 0 in CSGO
                    uint FooterLength = ReadU32(stream);
                    uint FortyEight = ReadU32(stream); // 48 in CSGO
                    uint zero2 = ReadU32(stream); // 0 in CSGO
                }
            }
            // Types
            while (true)
            {
                string typeName = ReadASCIIZ(stream);
                if (typeName == "")
                {
                    break;
                }

                // Directories
                while (true)
                {
                    string directoryName = ReadASCIIZ(stream);
                    if (directoryName == "")
                    {
                        break;
                    }

                    // Files
                    while (true)
                    {
                        string fileName = ReadASCIIZ(stream);
                        if (fileName == "")
                        {
                            break;
                        }

                        PackageEntry entry = new PackageEntry();
                        entry.FileName = fileName.ToLowerInvariant();
                        entry.DirectoryName = directoryName.Replace("/", "\\").ToLowerInvariant();
                        entry.TypeName = typeName.ToLowerInvariant();
                        entry.CRC32 = ReadU32(stream);
                        entry.SmallData = new byte[ReadU16(stream)];
                        entry.ArchiveIndex = ReadU16(stream);
                        entry.Offset = ReadU32(stream);
                        entry.Size = ReadU32(stream);

                        UInt16 terminator = ReadU16(stream);
                        if (terminator != 0xFFFF)
                        {
                            throw new FormatException("invalid terminator");
                        }

                        if (entry.SmallData.Length > 0)
                        {
                            stream.Read(entry.SmallData, 0, entry.SmallData.Length);
                        }

                        entries.Add(entry);
                    }
                }
            }

            this.Entries = entries;
        }
    }
}