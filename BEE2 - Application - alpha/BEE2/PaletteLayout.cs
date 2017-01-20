using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BEE2
{
    class PaletteLayout
    {
        public PaletteLayout(string filePath)
        {
            string[] fileContents = File.ReadAllLines(filePath);
            items = new string[32];//4 * 8
            subitems = new int[32];

            //find the name of this style
            Name = Global.FindValue(fileContents, "Name");

            int beginIndex = -1;
            //take out the comments, trim and find the start
            for (int i = 0; i < fileContents.Length; i++)
            {
                if (fileContents[i].Contains("//"))
                    fileContents[i] = fileContents[i].Substring(0, fileContents[i].IndexOf("//")).Trim();
                else
                    fileContents[i] = fileContents[i].Trim();
                if (fileContents[i].ToLower().Contains("@beginlist"))
                    beginIndex = i+1;

                //alow us to cheat and not rewrite the current builds
                if (fileContents[i].StartsWith("panelItemHolderPalette.AddType("))
                {
                    //then cut this out and the last part
                    if(fileContents[i].EndsWith(");"))
                        fileContents[i] = fileContents[i].Substring(0, fileContents[i].IndexOf(");"));
                    fileContents[i] = fileContents[i].Remove(0, "panelItemHolderPalette.AddType(".Length);
                    
                }

            }
            //load the items
            int itemIndex = 0;
            for (int i = beginIndex; i < fileContents.Length; i++)
            {
                if (fileContents[i].Trim().Length > 3)//has to be atleast this long to hold enough data... trim just to be safe
                {
                    string[] values = fileContents[i].Split(',');

                    items[itemIndex] = values[0].Trim('\"');
                    subitems[itemIndex] = int.Parse(values[1].Trim());
                    itemIndex++;
                }
            }
        }

        public void LoadPanel(PanelItemHolder panelToLoadTo, PanelItemHolder panelToLoadFrom)
        {
            panelToLoadTo.RemoveAll();
            for (int i = 0; i < items.Length; i++)
            {
                panelToLoadTo.AddType(items[i], subitems[i], panelToLoadFrom);
            }

        }
        public string Name { get; set; }
        private string[] items;
        private int[] subitems;
    }
}
