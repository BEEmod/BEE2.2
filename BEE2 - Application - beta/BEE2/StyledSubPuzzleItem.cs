using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BEE2
{
    public class StyledSubPuzzleItem
    {
        public string Name { get; private set; }
        public string Tooltip { get; private set; }
        public BitmapSource ImageSource { get; private set; }

        private string[] _subDefinition;

        public StyledSubPuzzleItem(string[] subDefinition)
        {
            _subDefinition = subDefinition;
            string name = ResourceFile.FindValue(_subDefinition, "Name");


            string newname = ResourceFile.FindValue(Global.BaseModUI, name);
            if (newname != null) Name = newname; else Name = name;

            name = ResourceFile.FindValue(_subDefinition, "Tooltip");
            if (name == null)
                newname = Name;
            else newname = ResourceFile.FindValue(Global.BaseModUI, name);
            if (newname != null)
                Tooltip = newname;
            else
                Tooltip = name;

            try
            {
                Bitmap temp;
                temp = Global.DLC2PackageFile.GetPaletteImage(ResourceFile.FindValue(_subDefinition, "Image"));
                //PreviewImage = Global.LoadBitmap( Global.DLC2PackageFile.GetPaletteImage(ResourceFile.FindValue(subDefinition, "Image"))); 
                if (temp == null)
                {
                    temp = Global.DLC2PackageFile.GetPaletteImage("palette\\budget_error"); //or could use 
                    if (temp == null)
                        temp = new Bitmap(32, 32);
                }
                ImageSource = Global.LoadBitmap(temp);
            }
            catch
            {
                ImageSource = new BitmapImage(new Uri(Global.MissingImagePath));
            }

        }

        public StyledSubPuzzleItem Clone()
        {
            return new StyledSubPuzzleItem(_subDefinition);
        }
    }
}
