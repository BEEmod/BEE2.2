using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BEE2
{
    class PaletteLayout
    {
        public string Name { get; private set; }
        //private string[] _items;
        //private int[] _subitems;

        public Dictionary<string, int> Items;

        //public PaletteLayout(string filePath)
        public PaletteLayout(string[] definition)
        {
            Items = new Dictionary<string, int>();

            //_items = new string[32];//4 * 8
            //_subitems = new int[32];

            //find the name of this style
            Name = ResourceFile.FindValue(definition, "Name");

            //int beginIndex = -1;
            ////take out the comments, trim and find the start
            //for (int i = 0; i < definition.Length; i++)
            //{
            //    if (definition[i].Contains("//"))
            //        definition[i] = definition[i].Substring(0, definition[i].IndexOf("//")).Trim();
            //    else
            //        definition[i] = definition[i].Trim();
            //    if (definition[i].ToLower().Contains("@beginlist"))
            //        beginIndex = i + 1;

            //    //alow us to cheat and not rewrite the current builds
            //    if (definition[i].StartsWith("panelItemHolderPalette.AddType("))
            //    {
            //        //then cut this out and the last part
            //        if (definition[i].EndsWith(");"))
            //            definition[i] = definition[i].Substring(0, definition[i].IndexOf(");"));
            //        definition[i] = definition[i].Remove(0, "panelItemHolderPalette.AddType(".Length);
            //    }
            //}

            ////load the items
            //int itemIndex = 0;
            //for (int i = beginIndex; i < definition.Length; i++)
            //{
            //    if (definition[i].Trim().Length > 3)//has to be atleast this long to hold enough data... trim just to be safe
            //    {
            //        string[] values = definition[i].Split(',');

            //        _items[itemIndex] = values[0].Trim('\"');
            //        _subitems[itemIndex] = int.Parse(values[1].Trim());
            //        itemIndex++;
            //    }
            //}

            //take out the comments, trim and find the start
            definition = Global.RemoveComments(definition).Where(item => !item.Contains("@beginlist") && !item.Contains("@endlist"))
                .Select(item => item.Replace(");", "").Replace("panelItemHolderPalette.AddType(", "")).ToArray();

            //load the items
            definition.Where(item => item.Trim().Length > 3).ToList()
                .ForEach(item => Items.Add(item.Split(',')[0].Trim('\"'), int.Parse(item.Split(',')[1])));
        }
    }
}
