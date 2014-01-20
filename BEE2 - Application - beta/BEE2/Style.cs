using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEE2
{
    public class Style
    {
        #region Properties
        public string Name { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }
        public string Base { get; set; }
        public Style BaseStyle { get; set; }

        public string[] StyleDefinition { get; private set; }
        public string[] WholeDefinition { get; private set; } //going to have to change this if you want to export prefrences with it
        #endregion

        public Style(string[] definition)
        {
            #region Get style specific Text
            WholeDefinition = definition;
            List<string> styleDefinition = new List<string>();
            bool inStyleSection = false;
            foreach (string line in definition)
            {
                if (line.Contains("@EndStyle"))
                {
                    inStyleSection = false;
                    break; //take out the break if you want the whole file to be parsed for styleinfo
                }
                if (inStyleSection)
                    styleDefinition.Add(line);
                if (line.Contains("@Style"))
                    inStyleSection = true;
            }
            StyleDefinition = styleDefinition.ToArray();
            #endregion

            #region Take out the comments
            StyleDefinition = Global.RemoveComments(StyleDefinition);
            //This Code looks wack, so I just am calling the Global remove comments instead
            //Not sure what I was thinking when I wrote it o_O

            //for (int i = 0; i < StyleDefinition.Length; i++)
            //{
            //    if (StyleDefinition[i].Trim().StartsWith("//"))
            //        StyleDefinition[i] = StyleDefinition[i].Trim().Substring(2, StyleDefinition[i].Trim().Length - 2);
            //}
            #endregion

            #region Find specific details from the style
            Name = ResourceFile.FindValue(StyleDefinition, "Name");
            Author = ResourceFile.FindValue(StyleDefinition, "Author");
            Comment = ResourceFile.FindValue(StyleDefinition, "Comment");
            Base = ResourceFile.FindValue(StyleDefinition, "Base");
            #endregion
        }
    }
}
