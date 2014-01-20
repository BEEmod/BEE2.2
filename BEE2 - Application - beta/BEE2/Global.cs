using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BEE2
{
    public class Global
    {
        public static List<Style> Styles { get; private set; }

        public static List<PuzzleItem> PuzzleItems { get; private set; }
        public static List<StyledPuzzleItem> StyledPuzzleItems { get; private set; }


        private const string STYLES_DIRECTORY = "styles";
        private const string PALETTE_DIRECTORY = "palettes";
        private const string SETTINGS_DIRECTORY = "settings";
        private const string ITEM_DIRECTORY = "items";

        private const string LOG_FILE_NAME = "log.txt";


        static Global()
        {
            LoadStyles();
            LoadStyledPuzzleItems();
        }

        /// <summary>Loads StyleMod styles into a list and links each one to its base style</summary>
        private static void LoadStyles()
        {
            //Load styles
            Styles = new List<Style>();
            string[] filePaths = Directory.GetFiles(STYLES_DIRECTORY, "*.style");
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

            string[] filePaths = Directory.GetFiles(ITEM_DIRECTORY, "*.beec");
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
                Style thisStyle = GetStyle(styleName);
                if (thisStyle == null && baseStyleName != styleName)
                {
                    // ...based on an existing style...
                    Style baseStyle = GetStyle(baseStyleName);
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

        public static Style GetStyle(string name)
        {
            return Styles.Where(item => item.Name == name).FirstOrDefault();
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
            File.WriteAllText(LOG_FILE_NAME, "=========== " + DateTime.Now.ToString() + " ===========" + Environment.NewLine);
            File.WriteAllText(LOG_FILE_NAME, problem + Environment.NewLine + Environment.NewLine);
        }


    }
}
