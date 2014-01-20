using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEE2
{
    public class PuzzleItem
    {
        public string TypeName { get; private set; }
        public StyledPuzzleItem Item { get; private set; }
        public List<StyledPuzzleItem> Items { get; private set; }

        public PuzzleItem(string typeName)
        {
            TypeName = typeName;
            Items = new List<StyledPuzzleItem>();
        }


        public void SetStyle(Style style)
        {
            StyledPuzzleItem item = null;
            do
            {
                item = Items.Where(i => i.Style == style).FirstOrDefault();
                if (item == null)
                {
                    style = style.BaseStyle;
                    if (style == null)
                        item = Items.FirstOrDefault();//just grab the first one you can find
                    //TODO: instead, add a default property to styled items, and grab default item instead
                    //      do this incase there are more than one branch with a styled version
                }
            } while (item == null); //There should not ever be an infinite loop, because PuzzleItems shouldn't
            //ever be created without atleast one StyledPuzzleItem
        }
    }
}
