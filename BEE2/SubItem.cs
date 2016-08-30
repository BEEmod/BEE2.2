using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace BEE2
{
    class SubItem
    {
        string[] subDefinition;
        public SubItem(string[] contents)
        {
            subDefinition = contents;
            Load();
        }
        public SubItem(SubItem s)
        {
            this.subDefinition = s.subDefinition;
            this.name = s.name;
            this.tooltip = s.tooltip;
            this.previewImage = s.previewImage;


        }
        private void Load()
        {
            string aname;//attempt to stop the weird flicker the label has when clicking between items

            aname = Global.FindValue(subDefinition, "Name");
            string newname = Global.FindValue(Global.basemodeui, aname);
            if (newname != null)  name = newname;  else name = aname;

            aname = Global.FindValue(subDefinition, "Tooltip");
            if (aname == null)
              newname = name;
            else newname = Global.FindValue(Global.basemodeui, aname);
            if (newname != null) tooltip = newname; else tooltip = aname;

            try
            {
                previewImage = Global.dlc2.GetPaletteImage(Global.FindValue(subDefinition, "Image"));
                if (previewImage == null)
                {
                    previewImage = Global.dlc2.GetPaletteImage("palette\\budget_error"); //or could use 
                    if (previewImage == null)
                        previewImage = new Bitmap(32, 32);
                }
            }
            catch
            {
                previewImage = Image.FromFile("visual_resources\\budget_error.png");
            }
        }
        string name, tooltip;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Tooltip
        {
            get { return tooltip; }
            set { tooltip = value; }
        }
        public Image PreviewImage
        {
            get { return previewImage; }
            set { previewImage = value; }
        }
        private Image previewImage;
        
        public SubItem Clone()
        {
            return new SubItem(this);
        }


    }
}
