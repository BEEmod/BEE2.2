using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Linq;

namespace BEE2
{
    class Global
    {
        public static string Portal2Directory { get { return portal2Directory; } set { portal2Directory = value; } }
        private static string portal2Directory;//beta
        public static string SixensePath;

        //private static string portal2_dlc2_materials = "\\portal2_dlc2\\materials";//dont use this now that we load vmts
        public static string resourceDirectory = "\\portal2_dlc2\\resource";
        public static string Language { get { return language; } set { language = value; } }
        public static string language = "english";
        public static string NotOnPaletteEntry = "999 999 999";//should be removed in the end and never seen in the exported editoritems
        public static string[] basemodeui;

        public static string StylesDirectory = "styles";
        public static string PaletteDirectory = "palettes";
        public static string SettingsDirectory = "settings";

        public static Dictionary<string, List<string>> Filters = new Dictionary<string, List<string>>();

        public static PackageFile dlc2;
        public static Dictionary<string, List<string>> ItemTypeStyles;
        public static string StyleName { get; set; }

        #region stuff

        // Static constructor
        static Global()
        {
            //Get BEE2 Settings
            string[] settings = File.ReadAllLines(SettingsDirectory + "\\BEE2_settings.txt");
            string s = FindValue(settings, "portal2.exe");


            if (!File.Exists(Path.Combine(s, "portal2.exe")))
            {
                s = Path.GetDirectoryName(Application.ExecutablePath + "\\..\\..");
                if (!File.Exists(Path.Combine(s, "portal2.exe")))
                {
                    s = "C:\\Steam\\steamapps\\common\\portal 2";
                    if (!File.Exists(Path.Combine(s, "portal2.exe")))
                    {
                        s = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\portal 2";
                        if (!File.Exists(Path.Combine(s, "portal2.exe")))
                        {
                            s = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\valvetestapp841";
                            if (!File.Exists(Path.Combine(s, "portal2.exe")))
                            {

                                //s = Path.GetDirectoryName(Application.ExecutablePath);//this shouldn't matter at this point, right?
                                //we should look for the exe ourselves
                                MessageBox.Show("The Portal2.exe could not be located." + Environment.NewLine +
                                    "The portal2.exe can typically be found in <Program Files>\\Steam\\SteamApps\\common\\Portal 2\\" + Environment.NewLine +
                                    "Please Navigate to the portal 2.exe in the next window.", "CAN'T FIND PORTAL 2");
                                OpenFileDialog dialog = new OpenFileDialog();
                                dialog.ShowDialog();
                                s = dialog.FileName;
                                if (s.EndsWith("portal2.exe"))
                                    s = s.Replace("portal2.exe", "");
                                else
                                    MessageBox.Show("ERROR?... strange");
                                //close the program
                            }
                        }
                    }
                }
            }
            if (s != FindValue(settings, "portal2.exe"))
                ChangeValue(settings, "portal2.exe", s);
            File.WriteAllLines(SettingsDirectory + "\\BEE2_settings.txt", settings);

            //MessageBox.Show("Okay so now we've set your file path to be\n" + s);
            if (s.EndsWith("\\")) s = s.Substring(0, s.Length - 1);
            //MessageBox.Show("changed... maybe\n" + s);
            portal2Directory = s;
            dlc2 = new PackageFile(Portal2Directory + "\\portal2_dlc2");
            basemodeui = File.ReadAllLines(Portal2Directory + resourceDirectory + "\\basemodui_" + language + ".txt");
            SixensePath = portal2Directory + "\\portal2_sixense";
            if (!Directory.Exists(SixensePath))
                SixensePath = null;

        }


        public static string ItemDirectory { get { return itemDirectory; } set { itemDirectory = value; } }
        static string itemDirectory = "items";
        private static string individualItemExtension = ".bee";
        public static string IndividualItemExtension { get { return individualItemExtension; } }
        public static void PopulateWithAllItems(PanelItemHolder holder)
        {
            string[] filePaths = Directory.GetFiles(itemDirectory, "*.bee");
            string[] temp;
            foreach (string path in filePaths)
            {
                if (path.EndsWith(individualItemExtension))
                {
                    temp = path.Split('\\');

                    PuzzleItem aPuzzleItem = new PuzzleItem(temp[temp.Length - 1].Replace(individualItemExtension, ""));
                    //get each subcatagory
                    for (int i = 0; i <= aPuzzleItem.SubcatagoryIndexMax; i++)
                    {
                        holder.InsertBefore(aPuzzleItem.Clone(), 0);
                        aPuzzleItem.ToggleSubcatagory();
                    }
                }
            }
        }

        public static void SavePalette(string fileName, string paletteName, List<PuzzleItem> listOfItems)
        {
            ArrayList goingOut = new ArrayList();
            goingOut.Add("\"Name\" \"" + paletteName + "\"");
            goingOut.Add("@beginlist");
            for (int i = 0; i < listOfItems.Count; i++)
            {
                if (i % 4 == 0)//every row
                    goingOut.Add(Environment.NewLine + "//Row " + i / 4);
                goingOut.Add("\"" + (listOfItems[i]).TypeName + "\", " + (listOfItems[i]).SubcatagoryIndex);
            }
            File.WriteAllLines(PaletteDirectory + "/" + fileName + ".beep", (string[])goingOut.ToArray(typeof(string)));
        }

        public static ArrayList ListOfPalettes;
        public static void LoadPalettes()
        {
            ListOfPalettes = new ArrayList();
            string[] filePaths = Directory.GetFiles(PaletteDirectory, "*.beep");
            foreach (string path in filePaths)
            {
                ListOfPalettes.Add(new PaletteLayout(path));
            }
        }


        public static ArrayList ListOfStyles;
        public static void LoadVBSPStyles()
        {
            ListOfStyles = new ArrayList();
            string[] filePaths = Directory.GetFiles(Global.StylesDirectory, "*.style");
            foreach (string path in filePaths)
            {
                ListOfStyles.Add(new VBSPStyle(path));
            }
        }

        public static VBSPStyle GetStyle(string name)
        {
            foreach (VBSPStyle style in ListOfStyles)
            {
                if (style.Name == name)
                    return style;
            }
            return null;
        }

        public static void LoadItemStyles()
        {
            // Item collections can define new styles based on base styles from the .style files
            // So go through each item collection and create the new styles
            string[] filePaths = Directory.GetFiles(Global.ItemDirectory, "*.beec");
            foreach (string path in filePaths)
            {
                string[] filecontents = File.ReadAllLines(path);
                //Parse the definition for style info
                ArrayList styleDefinition = new ArrayList();
                bool stylish = false;
                foreach (string line in filecontents)
                {
                    if (line.Contains("@EndStyle"))
                    {
                        stylish = false;
                        break;//take out the break if you want the whole file to be parsed for styleinfo
                    }
                    if (stylish)
                        styleDefinition.Add(line);
                    if (line.Contains("@Style"))
                        stylish = true;
                }
                string[] StyleDefinition = (string[])styleDefinition.ToArray(typeof(string));
                //take out the comments
                for (int i = 0; i < StyleDefinition.Length; i++)
                {
                    if (StyleDefinition[i].Trim().StartsWith("//"))
                        StyleDefinition[i] = StyleDefinition[i].Trim().Substring(2, StyleDefinition[i].Trim().Length - 2);
                }
                //find specific details from the style
                string styleName = Global.FindValue(StyleDefinition, "Style");
                string baseStyleName = Global.FindValue(StyleDefinition, "Base");
                string author = Global.FindValue(StyleDefinition, "Author");
                string comment = Global.FindValue(StyleDefinition, "Comment");
                if (styleName == null)
                {
                    if (baseStyleName != null)
                        styleName = baseStyleName;
                    else styleName = "Clean";
                }

                // if it's a new style...
                VBSPStyle thisStyle = GetStyle(styleName);
                if (thisStyle == null && baseStyleName != styleName)
                {
                    // ...based on an existing style...
                    VBSPStyle baseStyle = GetStyle(baseStyleName);
                    if (baseStyle != null)
                    {
                        // ...then add it.
                        thisStyle = new VBSPStyle(baseStyle.WholeDefinition);
                        thisStyle.Name = styleName;
                        thisStyle.Author = author;
                        thisStyle.Comment = comment;
                        thisStyle.Base = baseStyleName;
                        ListOfStyles.Add(thisStyle);
                    }
                }
                if (thisStyle == null)
                    continue;

                /*
                // make a list of the styles available for each item type
                ItemTypeStyles = new Dictionary<string, List<string>>();
                foreach (string s in filecontents)
                {
                    string line = s.Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length >= 10)
                    {
                        if (line.Substring(1, 5).Equals("Type\""))
                        {
                            string typeName = line.Substring(4 + 2).Trim();
                            if (!ItemTypeStyles.ContainsKey(typeName))
                                ItemTypeStyles.Add(typeName, new List<string>());
                            ItemTypeStyles[typeName].Add(typeName);
                        }
                    }
                }
                */

                string[][] individualItemDefinitions = Global.FindSubValues(filecontents, "Item");
                foreach (string[] definition in individualItemDefinitions)
                {
                    PuzzleItem aPuzzleItem = new PuzzleItem(definition);
                    aPuzzleItem.style = thisStyle;
                    thisStyle.BaseItems.Add(aPuzzleItem);
                    //don't need subcategories yet

                }


            } // next

        }

        /// <summary>
        /// Scales image to the largest possible value constraining to to the MaxWidth and MaxHeight, and maintaining the aspect ratio.
        /// </summary>
        /// http://stackoverflow.com/questions/6501797/resize-image-proportionally-with-maxheight-and-maxwidth-constraints
        /// <param name="image">Image to be scaled</param>
        /// <param name="maxWidth">Maximum width image can be scaled to.</param>
        /// <param name="maxHeight">Maximum height image can be scaled to.</param>
        /// <returns></returns>
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }



        public static ArrayList PreferredImages_Names;
        public static ArrayList PreferredImages_Paths;
        public static void LoadPreferredImages()
        {
            PreferredImages_Names = new ArrayList();
            PreferredImages_Paths = new ArrayList();
            string[] filePaths = Directory.GetFiles(SettingsDirectory, "*.beepi");
            foreach (string path in filePaths)
            {
                string[] fileContents = File.ReadAllLines(path);
                fileContents = RemoveComments(fileContents);
                for (int i = 0; i < fileContents.Length; i++)
                {
                    if (fileContents[i].Trim() != "")
                    {
                        string[] lineContents = fileContents[i].Trim().Split('\"');
                        PreferredImages_Names.Add(lineContents[1]);
                        PreferredImages_Paths.Add(lineContents[3]);
                    }
                }
            }

        }

        /// <summary>
        /// Removes inline comments and returns the result
        /// </summary>
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







        /// <summary>
        /// Locates the value of the property given in the given body
        /// </summary>
        /// <param name="body">Body of text to search through</param>
        /// <param name="property">Property to search for in the body</param>
        /// <returns>Returns the value of the property given</returns>
        public static string FindValue(string[] body, string property)
        {
            string line = "";
            try
            {

                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found! What is its value?
                            //make sure that there is even room for there to be a value
                            if (line.Length >= property.Length + 5)
                            {
                                string value = line.Substring(property.Length + 2).Trim();
                                return value.Substring(1, value.Length - 2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception while trying to find \"" + property + "\"");
            }
            return null;
        }
        public static string[] ChangeValue(string[] body, string property, string value)
        {
            string line = "";
            try
            {

                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found! What is its value?

                            body[i] = "\t\"" + property + "\"\t\"" + value + "\"";
                            return body;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception while trying to change\"" + property + "\" to \"" + value + "\"");
            }
            return null;
        }

        /// <summary>
        /// Changes the (numInstance)th instance of the value to property, starting with 0th
        /// </summary>
        /// <param name="body"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <param name="numInstance"></param>
        /// <returns></returns>
        public static string[] ChangeValue(string[] body, string property, string value, int numInstance)
        {
            string line = "";
            int X = 0;
            try
            {

                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            if (X++ < numInstance)
                                continue;
                            //property found! What is its value?

                            body[i] = "\t\"" + property + "\"\t\"" + value + "\"";
                            return body;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception while trying to change\"" + property + "\" to \"" + value + "\"");
            }
            return null;
        }


        /// <summary>
        /// Finds the bracketed values enclosed within the target value
        /// </summary>
        /// <param name="body"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string[] FindSubValue(string[] body, string property)
        {
            string line = "";
            try
            {

                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found!
                            ArrayList contents = new ArrayList();
                            int braceDepth = 0;
                            int j = i + 1;
                            do
                            {
                                line = body[j].Trim();

                                if (line == "{")
                                    braceDepth++;
                                else if (line == "}")
                                    braceDepth--;
                                //else
                                contents.Add(line);
                                j++;
                            } while (braceDepth > 0);

                            return (string[])contents.ToArray(typeof(string));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception while trying to find \"" + property + "\"");
            }
            return null;
        }

        /// <summary>
        /// Finds all instances of the bracketed values enclosed within the target values
        /// </summary>
        /// <param name="body"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string[][] FindSubValues(string[] body, string property)
        {
            string line = "";
            ArrayList returnValue = new ArrayList();
            try
            {

                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found!
                            ArrayList contents = new ArrayList();
                            int braceDepth = 0;
                            int j = i + 1;
                            do
                            {
                                line = body[j].Trim();

                                if (line == "{")
                                    braceDepth++;
                                else if (line == "}")
                                    braceDepth--;
                                //else
                                contents.Add(line);
                                j++;
                            } while (braceDepth > 0);

                            returnValue.Add((string[])contents.ToArray(typeof(string)));
                        }
                    }
                }
                if (returnValue.Count > 0)
                    return (string[][])returnValue.ToArray(typeof(string[]));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception while trying to find \"" + property + "\"");
            }
            return null;
        }
        /// <summary>
        /// Returns the passed body with each property changed to the string[] values 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="property"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string[] ChangeSubValues(string[] body, string property, string[] values)
        {
            string line = "";
            int valuesindex = 0;
            try
            {
                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found!

                            //change the property
                            ////dont output this line if this item is not on the palette at all
                            //if (values[valuesindex] != Global.NotOnPaletteEntry)
                            //{
                            body[i] = body[i].Substring(0, body[i].IndexOf("\""));
                            body[i] += "\"" + property + "\" \"" + values[valuesindex] + "\"";
                            //}
                            //else
                            //{
                            //    body[i] = "//removed position definition";// "//\"Position\"\t\"-1 -1 -1\"";
                            //}
                            valuesindex++;
                            //valuesindex = valuesindex >= values.Length ? 0 : valuesindex;//start the valuesindex over if it runs out of values
                        }
                    }
                }
                return body;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Exception while trying to find \"" + property + "\"");
            }
            return null;
        }



        public static string GetInternalName(string filename)
        {
            if (!File.Exists(filename))
                return "";
            else
                return FileVersionInfo.GetVersionInfo(filename).InternalName;
        }

        public static bool CheckFilesInStylesChanger()
        {
            string path = portal2Directory + "\\portal2_dlc2\\scripts\\";
            // does editoritems_original.txt exist?
            if (!File.Exists(path + "editoritems_original.txt"))
            {
                MessageBox.Show("I don't see the backup of Valve's original editoritems.txt.\nSo I'll automatically back up the current editoritems.txt for you now as editoritems_original.txt.\nIf the current editoritems.txt is a modified version, you should manually back up the original Valve version as editoritems_original.txt.");
                File.Copy(path + "editoritems.txt", path + "editoritems_original.txt", false);
            }

            path = portal2Directory + "\\bin\\";
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
            if (SixensePath != null)
            {
                path = SixensePath + "\\puzzles";
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
            path = portal2Directory + "\\portal2\\puzzles";
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

        #region filters

        public static void LoadFilters()
        {
            List<PuzzleItem> allPuzzleItems = new List<PuzzleItem>();

            string[] filePaths = Directory.GetFiles(itemDirectory, "*.bee");
            string[] temp;


            foreach (string path in filePaths)
            {
                string[] filecontents = File.ReadAllLines(path);

                string[][] individualItemDefinitions = Global.FindSubValues(filecontents, "Item");
                foreach (string[] definition in individualItemDefinitions)
                {
                    PuzzleItem aPuzzleItem = new PuzzleItem(definition);
                    foreach (string f in aPuzzleItem.Filters)
                    {
                        List<string> filter;
                        Filters.TryGetValue(f, out filter);
                        if (filter == null)
                        {
                            filter = new List<string>();
                            Filters.Add(f, filter);
                        }
                        if (!filter.Contains(aPuzzleItem.TypeName))
                            filter.Add(aPuzzleItem.TypeName);

                    }
                }
            } // next

        }

        public static List<string> GetItemsFromFilter(string filterName)
        {
            List<string> filter = null;
            if (filterName != null)
                Global.Filters.TryGetValue(filterName, out filter);
            return filter;
        }

        public static void PopulateWithAllItemsCollection(PanelItemHolder holder, string styleName, string filterName)
        {
            List<PuzzleItem> allItems = new List<PuzzleItem>(); // can only have duplicates in same style
            List<PuzzleItem> styleItems = new List<PuzzleItem>(); // can have duplicates
            List<PuzzleItem> nonMatchingItems = new List<PuzzleItem>();
            List<PuzzleItem> sortedItems = new List<PuzzleItem>();
            List<PuzzleItem> remainingItems = new List<PuzzleItem>();
            int RecursionDepth = 0;
            //List<string> filter;


            //Global.Filters.TryGetValue(filterName, out filter);

            //switch (filterName)
            //{
            //    case "All Items": filter = null;
            //        break;
            //    case "Portal 2 Items": filter = Portal2Items;
            //        break;
            //    case "BEE Items": filter = BEE1Items;
            //        break;
            //    case "HMW Items": filter = HMWItems;
            //        break;
            //    case "Carl's Items": filter = CarlItems;
            //        break;
            //    case "FGE Items": filter = FGEItems;
            //        break;
            //    case "Miscellaneous": filter = MiscItems;
            //        break;
            //    default: filter = null;
            //        break;
            //}

            VBSPStyle style = Global.GetStyle(styleName);
            while (style != null)
            {
                RecursionDepth++;
                if (RecursionDepth > 12)
                    break;
                styleItems.Clear();
                foreach (PuzzleItem aPuzzleItem in style.BaseItems)
                {
                    if ((!HasItemType(allItems, aPuzzleItem.TypeName)))
                    {
                        //get each subcatagory
                        for (int i = 0; i <= aPuzzleItem.SubcatagoryIndexMax; i++)
                        {
                            styleItems.Add(aPuzzleItem.Clone());
                            aPuzzleItem.ToggleSubcatagory();
                        }
                    }
                }
                allItems.AddRange(styleItems);

                style = Global.GetStyle(style.Base);
            } // repeat for base style of this style, so eg. 1950s items show up for 1970 if they aren't in 1970

            //if (filterName != "Items Matching Style")
            //{
            //    // lastly, add any remaining items that don't have a matching style
            //    foreach (VBSPStyle style2 in Global.ListOfStyles)
            //    {
            //        foreach (PuzzleItem aPuzzleItem in style2.BaseItems)
            //        {
            //            if ((!HasItemType(allItems, aPuzzleItem.TypeName)))
            //            {
            //                //get each subcatagory
            //                for (int i = 0; i <= aPuzzleItem.SubcatagoryIndexMax; i++)
            //                {
            //                    PuzzleItem subtype = aPuzzleItem.Clone();
            //                    if (subtype.PreviewImage != null)
            //                    {
            //                        subtype.PreviewImage = Global.ScaleImage(subtype.PreviewImage, 64, 64);
            //                        using (Graphics g = Graphics.FromImage(subtype.PreviewImage))
            //                        {
            //                            g.DrawString(style2.Name, new Font("Tahoma", 12), Brushes.Blue, new PointF(0, 0));
            //                        }
            //                    }
            //                    allItems.Add(subtype);
            //                    aPuzzleItem.ToggleSubcatagory();
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    // lastly, add any remaining items that don't have a matching style
            //    foreach (VBSPStyle style2 in Global.ListOfStyles)
            //    {
            //        foreach (PuzzleItem aPuzzleItem in style2.BaseItems)
            //        {
            //            if ((!HasItemType(allItems, aPuzzleItem.TypeName)))
            //            {
            //                //get each subcatagory
            //                for (int i = 0; i <= aPuzzleItem.SubcatagoryIndexMax; i++)
            //                {
            //                    PuzzleItem subtype = aPuzzleItem.Clone();
            //                    if (subtype.PreviewImage != null)
            //                    {
            //                        subtype.PreviewImage = Global.ScaleImage(subtype.PreviewImage, 64, 64);
            //                        using (Graphics g = Graphics.FromImage(subtype.PreviewImage))
            //                        {
            //                            g.DrawString(style2.Name, new Font("Tahoma", 12), Brushes.Blue, new PointF(0, 0));
            //                        }
            //                    }
            //                    holder.unfilteredPuzzleItems.Add(subtype);
            //                    aPuzzleItem.ToggleSubcatagory();
            //                }
            //            }
            //        }
            //    }
            //}


            foreach (PuzzleItem p in allItems)
            {
                holder.InsertBefore(p, 0);
            }
            //////if (filter != null)
            //////{
            //////    // now sort them according to the filter
            //////    string[] sorter = filter.ToArray();
            //////    //if (sorter == null || sorter == MiscItems) sorter = AllItems;

            //////    remainingItems.AddRange(allItems);
            //////    foreach (string s in sorter)
            //////    {
            //////        // in order, add all items that match to the sortedItems
            //////        foreach (PuzzleItem p in allItems)
            //////        {
            //////            if (p.TypeName == s)
            //////            {
            //////                sortedItems.Add(p);
            //////                remainingItems.Remove(p);
            //////            }
            //////        }
            //////    }
            //////    sortedItems.AddRange(remainingItems);

            //////    // now add them all to our PanelItemHolder
            //////    foreach (PuzzleItem aPuzzleItem in sortedItems)
            //////    {
            //////        if (filter.Contains(aPuzzleItem.TypeName))
            //////            holder.InsertAfter(aPuzzleItem, holder.GetNumberOfItems() - 1);
            //////        else
            //////            holder.PuzzleItems.Add(aPuzzleItem);
            //////    }
            //////}



            if (filterName.Trim() == "Items Matching Style")
            {
                remainingItems.Clear();
                remainingItems.AddRange(allItems);
                // lastly, add any remaining items that don't have a matching style
                //foreach (VBSPStyle style2 in Global.ListOfStyles)
                //{
                VBSPStyle style2 = Global.GetStyle(StyleName);
                foreach (PuzzleItem aPuzzleItem in style2.BaseItems)
                {
                    if (aPuzzleItem.SubcatagoryIndexMax > 0)
                    {
                        holder.InsertAfter(aPuzzleItem.Clone(), holder.GetNumberOfItems() - 1);
                        remainingItems.Remove((from ri in remainingItems where ri.TypeName == aPuzzleItem.TypeName select ri).FirstOrDefault());
                    }

                    //    if ((!HasItemType(allItems, aPuzzleItem.TypeName)))
                    //    {
                    //        //get each subcatagory
                    //        for (int i = 0; i <= aPuzzleItem.SubcatagoryIndexMax; i++)
                    //        {
                    //            PuzzleItem subtype = aPuzzleItem.Clone();
                    //            if (subtype.PreviewImage != null)
                    //            {
                    //                subtype.PreviewImage = Global.ScaleImage(subtype.PreviewImage, 64, 64);
                    //                using (Graphics g = Graphics.FromImage(subtype.PreviewImage))
                    //                {
                    //                    g.DrawString(style2.Name, new Font("Tahoma", 12), Brushes.Blue, new PointF(0, 0));
                    //                }
                    //            }
                    //            holder.InsertAfter(subtype, holder.GetNumberOfItems() - 1);
                    //            //holder.unfilteredPuzzleItems.Add(subtype);
                    //            aPuzzleItem.ToggleSubcatagory();
                    //        }
                    //    }
                }
                foreach (PuzzleItem aPuzzleItem in remainingItems)
                {
                    holder.PuzzleItems.Add(aPuzzleItem);
                }
                //holder.PuzzleItems.AddRange(remainingItems);
                //}
            }
            int oooo = 0;
        }

        #region old flters
        //static string[] Portal2Items = {
        //    "ITEM_BUTTON_PEDESTAL", "ITEM_BUTTON_FLOOR", "ITEM_CUBE", "ITEM_TBEAM", "ITEM_CATAPULT", 
        //    "ITEM_LIGHT_BRIDGE", "ITEM_BARRIER_HAZARD", "ITEM_BARRIER", "ITEM_PISTON_PLATFORM", "ITEM_RAIL_PLATFORM",
        //    "ITEM_LASER_EMITTER_CENTER", "ITEM_LASER_CATCHER_CENTER", "ITEM_LASER_RELAY_CENTER", "ITEM_PANEL_CLEAR", 
        //    "ITEM_PANEL_ANGLED", "ITEM_PANEL_FLIP", "ITEM_PANEL_STAIRS", "ITEM_TURRET", "ITEM_GOO", 
        //    "ITEM_LIGHT_PANEL", "ITEM_PAINT_SPLAT", "ITEM_SECONDARY_OBSERVATION_ROOM",
        //};

        //static string[] BEE1Items = {
        //    "ITEM_DRAW_BRIDGE",
        //    "ITEM_CRUSHER",
        //    "ITEM_VENT",
        //    "ITEM_HIGH_ENERGY_PELLET_EMITTER",
        //    "ITEM_HIGH_ENERGY_PELLET_CATCHER",
        //    "DOOR",
        //    "ITEM_TRIGGERS",
        //    "ITEM_BEE_LOGIC",
        //    "ITEM_AUTOPORTAL",
        //    "ITEM_LIGHT_PANEL_WARM",
        //};

        //static string[] HMWItems = {
        //    "ITEM_PORTAL_MAGNET", "ITEM_SAVEPOINT", "ITEM_LOGIC_GATE", "ITEM_SQUARE_LIGHT",
        //    "ITEM_SENDIFICATOR", "ITEM_SENDIFICATOR_LASER",
        //};

        //static string[] CarlItems = {
        //    "ITEM_BARRIER_HAZARD", "ITEM_PAINT_SPLAT_REFLECTION", "ITEM_128X64X64", 
        //};

        //static string[] FGEItems = {
        //    "ITEM_ANIMAL_KING",
        //    "ITEM_FG_AUTOPORTAL",
        //    "ITEM_DROPPER_BOMB",
        //    "ITEM_GLASS_BREAK",
        //    "ITEM_CAMERA",
        //    "ITEM_CHOREO",
        //    "ITEM_DROPPER_FG",
        //    "ITEM_LOGIC_GATE_FG",
        //    "ITEM_PORTAL1_WALL",
        //    "ITEM_PHYSLER",
        //    "ITEM_PORTAL_CUBE",
        //    "ITEM_PORTALGUN",
        //    "ITEM_SENDIFICATOR",
        //    "ITEM_SENDIFICATOR_LASER",
        //    "ITEM_SKYBOX",
        //    "ITEM_SKYBOX_WALL",
        //    "ITEM_SOT",
        //    "ITEM_SOT_FIELD",
        //    "ITEM_TNT_TURRET",
        //    "ITEM_TORCH",
        //    "ITEM_VACTUBE_1",
        //    "ITEM_VACTUBE_2",            
        //};

        //static string[] AllItems = {
        //    "ITEM_BUTTON_PEDESTAL", "ITEM_BUTTON_FLOOR", "ITEM_CUBE", "ITEM_TBEAM", "ITEM_CATAPULT", 
        //    "ITEM_LIGHT_BRIDGE", "ITEM_BARRIER_HAZARD", "ITEM_BARRIER", "ITEM_PISTON_PLATFORM", "ITEM_RAIL_PLATFORM",
        //    "ITEM_LASER_EMITTER_CENTER", "ITEM_LASER_CATCHER_CENTER", "ITEM_LASER_RELAY_CENTER", "ITEM_PANEL_CLEAR", 
        //    "ITEM_PANEL_ANGLED", "ITEM_PANEL_FLIP", "ITEM_PANEL_STAIRS", "ITEM_TURRET", "ITEM_GOO", 
        //    "ITEM_LIGHT_PANEL", "ITEM_PAINT_SPLAT", "ITEM_SECONDARY_OBSERVATION_ROOM",
        //    "ITEM_DRAW_BRIDGE",
        //    "ITEM_CRUSHER",
        //    "ITEM_VENT",
        //    "ITEM_HIGH_ENERGY_PELLET_EMITTER",
        //    "ITEM_HIGH_ENERGY_PELLET_CATCHER",
        //    "DOOR",
        //    "ITEM_TRIGGERS",
        //    "ITEM_BEE_LOGIC",
        //    "ITEM_AUTOPORTAL",
        //    "ITEM_LIGHT_PANEL_WARM",
        //    "ITEM_PORTAL_MAGNET", "ITEM_SAVEPOINT", "ITEM_LOGIC_GATE", "ITEM_SQUARE_LIGHT",
        //    "ITEM_SENDIFICATOR",
        //    "ITEM_SENDIFICATOR_LASER",
        //    "ITEM_PAINT_SPLAT_REFLECTION", "ITEM_128X64X64", 
        //    "ITEM_ANIMAL_KING",
        //    "ITEM_FG_AUTOPORTAL",
        //    "ITEM_DROPPER_BOMB",
        //    "ITEM_GLASS_BREAK",
        //    "ITEM_CAMERA",
        //    "ITEM_CHOREO",
        //    "ITEM_DROPPER_FG",
        //    "ITEM_LOGIC_GATE_FG",
        //    "ITEM_PORTAL1_WALL",
        //    "ITEM_PHYSLER",
        //    "ITEM_PORTAL_CUBE",
        //    "ITEM_PORTALGUN",
        //    "ITEM_SKYBOX",
        //    "ITEM_SKYBOX_WALL",
        //    "ITEM_SOT",
        //    "ITEM_SOT_FIELD",
        //    "ITEM_TNT_TURRET",
        //    "ITEM_TORCH",
        //    "ITEM_VACTUBE_1",
        //    "ITEM_VACTUBE_2",            
        //};

        //static string[] MiscItems = {"ignore this"};
        #endregion


        //private static bool FilterContains(string [] a, string s) {
        //    if (a == null) 
        //        return true;
        //    if (a == MiscItems)
        //        return !FilterContains(AllItems, s);
        //    foreach (string i in a)
        //    {
        //        if (i == s) return true;
        //    }
        //    return false;
        //}

        public static bool HasItemType(List<PuzzleItem> L, string type)
        {
            foreach (PuzzleItem p in L)
            {
                if (p.TypeName == type)
                    return true;
            }
            return false;
        }


        #endregion


    } // end of Global class
} // end of namespace BEE2