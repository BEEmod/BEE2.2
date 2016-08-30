using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;

namespace BEE2
{
    public class VBSPStyle
    {
        // A list of all the base items (not subtypes), loaded at form creation for this style
        // All items and subtypes should be cloned (indirectly?) from this list
        public List<PuzzleItem> BaseItems = new List<PuzzleItem>();

        public VBSPStyle(string FilePath)
        {
            VBSPStyle_shared(File.ReadAllLines(FilePath));
        }

        public VBSPStyle(string[] Definition)
        {
            VBSPStyle_shared(Definition);
        }

        private void VBSPStyle_shared(string[] Definition)
        {
            wholeDefinition = Definition;

            //Parse the definition for style info
            ArrayList styleDefinition = new ArrayList();
            bool stylish = false;
            foreach (string line in Definition)
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
            StyleDefinition = (string[])styleDefinition.ToArray(typeof(string));
            
            //take out the comments
            for(int i=0; i<StyleDefinition.Length;i++)
            {
                if(StyleDefinition[i].Trim().StartsWith("//"))
                    StyleDefinition[i]=StyleDefinition[i].Trim().Substring(2,StyleDefinition[i].Trim().Length-2);
            }

            //find specific details from the style
            Name = Global.FindValue(StyleDefinition, "Name");
            Author = Global.FindValue(StyleDefinition, "Author");
            Comment = Global.FindValue(StyleDefinition, "Comment");
            Base = Global.FindValue(StyleDefinition, "Base");
        }
        public string Name;
        public string Author;
        public string Comment;
        public string Base;

        private string[] StyleDefinition;
        private string[] wholeDefinition;
        public string[] WholeDefinition { get { return wholeDefinition; } }//going to have to change this if you want to export prefrences with it
    }
}
