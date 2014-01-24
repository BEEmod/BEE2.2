using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

namespace BEE2
{
    public class Global
    {
        public static string Language { get; private set; }
        public static List<Style> Styles { get; private set; }

        public static List<PuzzleItem> PuzzleItems { get; private set; }
        public static List<StyledPuzzleItem> StyledPuzzleItems { get; private set; }
        public static string NotOnPaletteEntry { get { return "999 999 999"; } } //will be removed in the end and never seen in the exported editoritems

        public static string[] BaseModUI { get; private set; }
        public static Dictionary<string, string> PreferredImages { get; private set; }
        public static PackageFile DLC2PackageFile { get; private set; }
        public static string MissingImagePath { get; private set; }

        #region Directories
        public static string Portal2Directory { get; private set; }
        //In /Portal 2
        public static string DLC2Directory { get; private set; }
        public static string SixenseDirectory { get; private set; }
        //in /DLC2_PATH
        public static string DLC2ResourceDirectory { get; private set; }
        //In /BEE2 
        public static string StylesDirectory { get; private set; }
        public static string PaletteDirectory { get; private set; }
        public static string SettingsDirectory { get; private set; }
        public static string ItemsDirectory { get; private set; }
        public static string VisualResourcesDirectory { get; private set; }
        #endregion

        #region Paths
        //In /Portal 2
        private const string DLC2_PATH = "portal2_dlc2";
        private const string SIXENSE_PATH = "portal2_sixense";

        //in /DLC2_PATH
        private const string DLC2_RESOURCE_PATH = "resource";

        //In /BEE2 
        private const string STYLES_PATH = "styles";
        private const string PALETTE_PATH = "palettes";
        private const string SETTINGS_PATH = "settings";
        private const string ITEMS_PATH = "items";
        private const string VISUAL_RESOURCES_PATH = "visual_resources";

        private const string LOG_NAME = "log.txt";
        private const string SETTINGS_FILE_NAME = "BEE2_settings.txt";
        private const string MISSING_IMAGE_NAME = "budget_error.png";
        #endregion

        private static string[] _settings;

        #region Constructor
        static Global()
        {
            // Set directories

            #region In /BEE2
            //These used to include the Portal2Directory, however, that cannot be loaded until the settings have been loaded from the 
            //SettingsDirectory, so now this step is a bit silly.
            StylesDirectory = STYLES_PATH;
            PaletteDirectory = PALETTE_PATH;
            SettingsDirectory = SETTINGS_PATH;
            ItemsDirectory = ITEMS_PATH;
            VisualResourcesDirectory = VISUAL_RESOURCES_PATH;
            MissingImagePath = VisualResourcesDirectory + '\\' + MISSING_IMAGE_NAME;
            #endregion

            LoadSettings();

            #region Get the Portal2Directory
            if (!File.Exists(Portal2Directory + "\\portal2.exe"))
            {
                string temp = "";
                bool foundPortal2Directory = false;
                //look for 'portal2.exe' programatically
                try
                {
                    string here = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    //trims off the last 2 directories from the path
                    for (int i = 0; i < 2; i++) { here = here.Replace('\\' + here.Split('\\').Last(), ""); }
                    if (File.Exists(here + "\\portal2.exe"))
                    {
                        Portal2Directory = here;
                        foundPortal2Directory = true;
                    }
                }
                catch (Exception ex)
                {
                    LogProblem("Trying to traverse up 2 levels failed while looking for the portal2 directory" +
                        Environment.NewLine + ex.ToString());
                    // I dont care if this breaks.... just move along
                }

                string[] possibleDirectories = new string[]
                {
                    "Program Files (x86)\\Steam\\steamapps\\common\\portal 2",
                    "Program Files\\Steam\\steamapps\\common\\portal 2",
                    "Steam\\steamapps\\common\\portal 2"
                };

                for (char c = 'A'; c <= 'Z' && !foundPortal2Directory; c++)
                {
                    foreach (string pos in possibleDirectories)
                    {
                        string path = c + ":\\" + pos + "\\portal2.exe";
                        if (File.Exists(path))
                        {
                            Portal2Directory = c + ":\\" + pos;
                            foundPortal2Directory = true;
                            break;
                        }
                    }
                }

                if (!foundPortal2Directory)
                {
                    //couldn't manually find it, ask the user
                    MessageBox.Show("The Portal2.exe could not be located." + Environment.NewLine +
                                    "The portal2.exe can typically be found in <Program Files>\\Steam\\SteamApps\\common\\Portal 2\\" + Environment.NewLine +
                                    "Please Navigate to the portal 2.exe in the next window.", "CAN'T FIND PORTAL 2");

                    Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
                    dialog.ShowDialog();
                    temp = dialog.FileName;
                    if (temp.EndsWith("portal2.exe"))
                    {
                        Portal2Directory = temp.Replace("portal2.exe", "");
                        foundPortal2Directory = true;
                    }
                    else
                    {
                        MessageBox.Show("My informants indicate you did not select 'portal2.exe' and thusly I shall depart. I bid you farewell." +
                             Environment.NewLine + Environment.NewLine + "Yours Truly," + Environment.NewLine + "BEE2");
                        Environment.Exit(0);
                    }
                    //close the program
                }
                ResourceFile.ChangeValue(_settings, "portal2.exe", Portal2Directory);
                File.WriteAllLines(SettingsDirectory + "\\BEE2_settings.txt", _settings);
            }
            #endregion

            #region In /Portal 2
            DLC2Directory = Portal2Directory + '\\' + DLC2_PATH;
            SixenseDirectory = Portal2Directory + '\\' + SIXENSE_PATH;
            if (!Directory.Exists(SixenseDirectory))
                SixenseDirectory = null;

            // In /DLC2_PATH
            DLC2ResourceDirectory = Portal2Directory + '\\' + DLC2_RESOURCE_PATH;
            #endregion

            DLC2PackageFile = new PackageFile(DLC2Directory);

            LoadStyles();
            LoadStyledPuzzleItems();
            LoadPreferredImages();
            LoadBaseModUI();

        }
        #endregion

        #region Initial Load...
        private static void LoadSettings()
        {
            _settings = File.ReadAllLines(SettingsDirectory + "\\BEE2_settings.txt");
            Portal2Directory = ResourceFile.FindValue(_settings, "portal2.exe");
            Language = ResourceFile.FindValue(_settings, "language");
        }

        /// <summary>Loads StyleMod styles into a list and links each one to its base style</summary>
        private static void LoadStyles()
        {
            //Load styles
            Styles = new List<Style>();
            string[] filePaths = Directory.GetFiles(STYLES_PATH, "*.style");
            foreach (string path in filePaths)
            {
                Styles.Add(new Style(File.ReadAllLines(path)));
            }

            //link styles to their base style
            Styles.ForEach(style => style.BaseStyle = Styles.Where(item => item.Name == style.Base).FirstOrDefault());
        }

        /// <summary> Loads all styled items from the items folder and properly attaches them to the PuzzleItem</summary>
        private static void LoadStyledPuzzleItems()
        {
            // Item collections can define new styles based on base styles from the .style files
            // So go through each item collection and create the new styles
            //TODO: Perhaps item collections shouldn't be able to created styles on the fly, but I'm not sure.

            string[] filePaths = Directory.GetFiles(ITEMS_PATH, "*.beec");
            foreach (string path in filePaths)
            {
                string[] filecontents = File.ReadAllLines(path);
                //Parse the definition for style info
                List<string> styleDefinition = new List<string>();
                bool inStyleSection = false;
                foreach (string line in filecontents)
                {
                    if (line.Contains("@EndStyle"))
                    {
                        inStyleSection = false;
                        break;//take out the break if you want the whole file to be parsed for styleinfo
                    }
                    if (inStyleSection)
                        styleDefinition.Add(line);
                    if (line.Contains("@Style"))
                        inStyleSection = true;
                }
                string[] StyleDefinition = styleDefinition.ToArray();

                //take out the comments
                StyleDefinition = Global.RemoveComments(StyleDefinition);
                //for (int i = 0; i < StyleDefinition.Length; i++)
                //{
                //    if (StyleDefinition[i].Trim().StartsWith("//"))
                //        StyleDefinition[i] = StyleDefinition[i].Trim().Substring(2, StyleDefinition[i].Trim().Length - 2);
                //}

                //find specific details from the style
                string styleName = ResourceFile.FindValue(StyleDefinition, "Style");
                string baseStyleName = ResourceFile.FindValue(StyleDefinition, "Base");
                string author = ResourceFile.FindValue(StyleDefinition, "Author");
                string comment = ResourceFile.FindValue(StyleDefinition, "Comment");
                if (styleName == null)
                {
                    if (baseStyleName != null)
                        styleName = baseStyleName;
                    else styleName = "Clean";
                }

                // if it's a new style...
                Style thisStyle = GetStyleByName(styleName);
                if (thisStyle == null && baseStyleName != styleName)
                {
                    // ...based on an existing style...
                    Style baseStyle = GetStyleByName(baseStyleName);
                    if (baseStyle != null)
                    {
                        // ...then add it.
                        thisStyle = new Style(baseStyle.WholeDefinition);
                        thisStyle.Name = styleName;
                        thisStyle.Author = author;
                        thisStyle.Comment = comment;
                        thisStyle.Base = baseStyleName;
                        thisStyle.BaseStyle = baseStyle;
                        Styles.Add(thisStyle);
                    }
                }
                //TODO: Or we could say that it is defaultly based off of the clean style, as I believe is done elsewhere in here.
                if (thisStyle == null)
                    continue;

                //Now actually load the StyledPuzzleItems
                string[][] individualItemDefinitions = ResourceFile.FindSubValues(filecontents, "Item");
                foreach (string[] definition in individualItemDefinitions)
                {
                    StyledPuzzleItem aStyledPuzzleItem = new StyledPuzzleItem(definition);
                    aStyledPuzzleItem.Style = thisStyle;
                    //thisStyle.BaseItems.Add(aPuzzleItem);
                    //don't need subcategories yet
                }




            }
        }

        public static void LoadPreferredImages()
        {
            PreferredImages = new Dictionary<string, string>();
            string[] filePaths = Directory.GetFiles(SETTINGS_PATH, "*.beepi");
            foreach (string path in filePaths)
            {
                string[] fileContents = File.ReadAllLines(path);
                fileContents = RemoveComments(fileContents);
                fileContents.Where(contents => !string.IsNullOrEmpty(contents.Trim())).ToList()
                    .ForEach(entry => PreferredImages.Add(entry.Trim().Split('\"')[0], entry.Trim().Split('\"')[1]));
                //for (int i = 0; i < fileContents.Length; i++)
                //{
                //    if (fileContents[i].Trim() != "")
                //    {
                //        string[] lineContents = fileContents[i].Trim().Split('\"');
                //        PreferredImages_Names.Add(lineContents[1]);
                //        PreferredImages_Paths.Add(lineContents[3]);
                //    }
                //}
            }
        }

        public static void LoadBaseModUI()
        {
            BaseModUI = File.ReadAllLines(Portal2Directory + DLC2ResourceDirectory + "\\basemodui_" + Language + ".txt");
        }
        #endregion

        #region Methods
        public static Style GetStyleByName(string name)
        {
            return Styles.Where(item => item.Name == name).FirstOrDefault();
        }

        public static void ChangeLanguage(string language)
        {
            ResourceFile.ChangeValue(_settings, "language", language.ToLower());
            Language = language;
        }
        #endregion

        #region Utility
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        /// <summary> Transforms a Bitmap object into a BitmapSource object which can be used in wpf applications</summary>
        /// http://khason.net/blog/how-to-use-systemdrawingbitmap-hbitmap-in-wpf/
        /// <param name="source"></param>
        /// <returns></returns>
        public static BitmapSource LoadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs = null;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                   IntPtr.Zero, Int32Rect.Empty,
                   System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }

        /// <summary>Removes inline comments and returns the result</summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string[] RemoveComments(string[] body)
        {
            for (int i = 0; i < body.Length; i++)
            {
                body[i] = body[i].Trim();
                if (body[i].Contains("//"))
                    body[i] = body[i].Remove(body[i].IndexOf("//"));
            }
            return body;
        }

        public static void LogProblem(string problem)
        {
            File.WriteAllText(LOG_NAME, "=========== " + DateTime.Now.ToString() + " ===========" + Environment.NewLine);
            File.WriteAllText(LOG_NAME, problem + Environment.NewLine + Environment.NewLine);
        }
        #endregion

        #region StyleMod
        public static string GetInternalName(string filename)
        {
            if (!File.Exists(filename))
                return "";
            else
                return FileVersionInfo.GetVersionInfo(filename).InternalName;
        }

        public static bool CheckFilesInStylesChanger()
        {
            string path = Portal2Directory + "\\portal2_dlc2\\scripts\\";
            // does editoritems_original.txt exist?
            if (!File.Exists(path + "editoritems_original.txt"))
            {
                MessageBox.Show("I don't see the backup of Valve's original editoritems.txt.\nSo I'll automatically back up the current editoritems.txt for you now as editoritems_original.txt.\nIf the current editoritems.txt is a modified version, you should manually back up the original Valve version as editoritems_original.txt.");
                File.Copy(path + "editoritems.txt", path + "editoritems_original.txt", false);
            }

            path = Portal2Directory + "\\bin\\";
            // does vbsp_styles.exe exist?
            if (!File.Exists(path + "vbsp_styles.exe"))
            {

                if (File.Exists(path + "vbsp.exe"))
                {
                    if (GetInternalName(path + "vbsp.exe") == "vbsp_styles.exe")
                    {
                        MessageBox.Show("That's strange, BEEMod's level geometry compiler hooker is installed as vbsp.exe like it should be, but the backup vbsp_styles.exe is missing.\nI'll automatically fix that for you, otherwise the next update would remove our mod.");
                        File.Copy(path + "vbsp.exe", path + "vbsp_styles.exe", false);
                    }
                    else
                    {
                        MessageBox.Show("vbsp_styles.exe is missing. Reinstall BEEMod 2.");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("vbsp.exe and vbsp_styles.exe are both missing. Verify the cache in Steam for Portal 2, then reinstall BEEMod 2.");
                    return false;
                }
            }

            // has vbsp.exe been updated by Steam, so the vbsp.exe is no longer my program?
            if (GetInternalName(path + "vbsp.exe") != "vbsp_styles.exe")
            {
                MessageBox.Show("Steam has automatically updated the level geometry compiler vbsp.exe.\nI'll automatically hook it and use the updated version for you.");
                File.Copy(path + "vbsp.exe", path + "vbsp_original.exe", true);
                File.Copy(path + "vbsp_styles.exe", path + "vbsp.exe", true);
            }
            else
            {
                // vbsp.exe is ours, so do we still have valve's?
                if (!File.Exists(path + "vbsp_original.exe"))
                {
                    MessageBox.Show("The original vbsp.exe is missing. Verify your steam cache for Portal 2 to get it back, then run BEEMod 2.");
                    return false;
                }
            }

            // does vrad_styles.exe exist?
            if (!File.Exists(path + "vrad_styles.exe"))
            {

                if (File.Exists(path + "vrad.exe"))
                {
                    if (GetInternalName(path + "vrad.exe") == "vrad_styles.exe")
                    {
                        MessageBox.Show("That's strange, BEEMod's calculating lighting hooker is installed as vrad.exe like it should be, but the backup vrad_styles.exe is missing.\nI'll automatically fix that for you, otherwise the next update would remove our mod.");
                        File.Copy(path + "vrad.exe", path + "vrad_styles.exe", false);
                    }
                    else
                    {
                        MessageBox.Show("vrad_styles.exe is missing. Reinstall BEEMod 2.");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("vrad.exe and vrad_styles.exe are both missing. Verify the cache in Steam for Portal 2, then reinstall BEEMod 2.");
                    return false;
                }
            }

            // has vrad.exe been updated by Steam, so the vrad.exe is no longer my program?
            if (GetInternalName(path + "vrad.exe") != "vrad_styles.exe")
            {
                MessageBox.Show("Steam has automatically updated the level geometry compiler vrad.exe.\nI'll automatically hook it and use the updated version for you.");
                File.Copy(path + "vrad.exe", path + "vrad_original.exe", true);
                File.Copy(path + "vrad_styles.exe", path + "vrad.exe", true);
            }
            else
            {
                // vrad.exe is ours, so do we still have valve's?
                if (!File.Exists(path + "vrad_original.exe"))
                {
                    MessageBox.Show("The original vrad.exe is missing. Verify your steam cache for Portal 2 to get it back, then run BEEMod 2.");
                    return false;
                }
            }

            return true;
        }

        public static bool processExists(string exeFileName)
        {
            return (System.Diagnostics.Process.GetProcessesByName(exeFileName).Length != 0);
        }

        public static bool IsPortal2Running()
        {
            return processExists("portal2");
        }

        // Allow Puzzle Maker to generate thumbnails by stopping the thumbnail from being Read-Only
        public static void ClearThumbnail()
        {
            string path;
            DirectoryInfo dir;
            // If they have the SIXENSE DLC for the Razer Hydra or Playstation Move,
            // the thumbnail will also be in the sixense_dlc folder

            // make all untitled.jpg files in puzzles dirs have no attributes (not read only)
            if (SixenseDirectory != null)
            {
                path = SixenseDirectory + "\\puzzles";
                dir = new DirectoryInfo(path);
                foreach (DirectoryInfo d in dir.GetDirectories())
                {
                    string s = Path.Combine(d.FullName, "untitled.jpg");
                    if (File.Exists(s))
                    {
                        // Making untitled.jpg writable
                        File.SetAttributes(s, FileAttributes.Normal);
                    }
                }
            }

            // make all untitled.jpg files in puzzles dirs have no attributes (not read only)
            path = Portal2Directory + "\\portal2\\puzzles";
            dir = new DirectoryInfo(path);
            foreach (DirectoryInfo d in dir.GetDirectories())
            {
                string s = Path.Combine(d.FullName, "untitled.jpg");
                if (File.Exists(s))
                {
                    // Making untitled.jpg writable
                    File.SetAttributes(s, FileAttributes.Normal);
                }
            }
        } // end of function ClearThumbnail
        #endregion
    }
}
