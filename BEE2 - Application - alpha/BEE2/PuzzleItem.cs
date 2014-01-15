using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Linq;


namespace BEE2
{
    public class PuzzleItem : PictureBox
    {

        public PanelItemHolder MyHolder { get; set; }
        public string TypeName { get; set; }
        public string Author;
        public string Description;
        public string[] Filters;


        private string fileName;


        public Image PreviewImage
        {
            get { return ((SubItem)SubItems[subcatagoryIndex]).PreviewImage; }
            set { ((SubItem)SubItems[subcatagoryIndex]).PreviewImage = value; }
        }
        //private PictureBox previewImage;

        public VBSPStyle style;




        //public Image PreviewImage;

        /// <summary>
        /// Creates a new PuzzleItem based off of a singular item file.
        /// </summary>
        /// <param name="fileName"></param>
        public PuzzleItem(string fileName)
        {
            this.fileName = fileName;
            //previewImage = new PictureBox();
            LoadItem();
        }

        /// <summary>
        /// Creates a new PuzzleItem based off of a given item definition
        /// </summary>
        /// <param name="itemDefinition">Item definitions can be taken directly from the /scripts/editoritems.txt</param>
        public PuzzleItem(string[] itemDefinition)
        {
            this.fileName = "";
            //previewImage = new PictureBox();
            LoadItem(itemDefinition);
        }

        /// <summary>
        /// Creates a new PuzzleItem based off of a given item definition, and a specified subcatagory
        /// </summary>
        /// <param name="itemDefinition">Item definitions can be taken directly from the /scripts/editoritems.txt</param>
        /// <param name="subcatagory"></param>
        public PuzzleItem(string[] itemDefinition, int subcatagory)
        {
            this.fileName = "";
            //previewImage = new PictureBox();
            LoadItem(itemDefinition);
            SubcatagoryIndex = subcatagory;
        }

        /// <summary>
        /// Creates a new PuzzleItem based off of a given item definition, and a specified subcatagory
        /// but using the existing image
        /// </summary>
        /// <param name="itemDefinition">Item definitions can be taken directly from the /scripts/editoritems.txt</param>
        /// <param name="subcatagory"></param>
        /// <param name="image"></param>
        public PuzzleItem(PuzzleItem p)
        {
            this.fileName = p.fileName;
            this.TypeName = p.TypeName;
            this.Author = p.Author;
            this.Description = p.Description;

            // This object is shared with the original item
            this.ItemDefinition = p.ItemDefinition; // shallow clone

            // The subitems need to be different objects so we can change the image in a copy.
            // But the objects inside the subitem are the same objects until we change them.
            this.SubItems = new ArrayList();
            foreach (SubItem s in p.SubItems)
            {
                this.SubItems.Add(s.Clone());
            }

            this.subcatagoryIndex = p.subcatagoryIndex;

        }

        ///// <summary>
        ///// I dont think this is used anymore
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <param name="myHolder"></param>
        //public PuzzleItem(string fileName, PanelItemHolder myHolder)
        //{
        //    this.fileName = fileName;
        //    previewImage = new PictureBox();
        //    LoadItem();
        //    this.MyHolder = myHolder;
        //}
        public PuzzleItem Clone()
        {
            //should probably just clone from the definition anyway
            //if (fileName != "")
            //    return new PuzzleItem(fileName);
            return new PuzzleItem(this);
        }

        private void LoadItem()
        {
            string[] itemContents;
            if (fileName.Contains(":\\"))
                itemContents = File.ReadAllLines(fileName);
            else
                itemContents = File.ReadAllLines(Global.ItemDirectory + "\\" + fileName + Global.IndividualItemExtension);
            LoadItem(itemContents);
        }


        private int subcatagoryIndex = 0;
        public int SubcatagoryIndex
        {
            get { return subcatagoryIndex; }
            set
            {
                if (value < SubItems.Count)
                    subcatagoryIndex = value;
            }
        }
        /// <summary>
        /// Maximume value for the SubcatagoryIndex
        /// </summary>
        public int SubcatagoryIndexMax
        {
            get
            {
                return SubItems.Count - 1;
            }

        }
        public void ToggleSubcatagory()
        {
            if (SubcatagoryIndex + 1 >= SubItems.Count)
                SubcatagoryIndex = 0;
            else
                SubcatagoryIndex++;

        }

        public bool IsInFilter(string filter)
        {
            return Filters.Contains(filter);
        }

        /// <summary>
        /// Contains the whole Item definition to be used in editoritems.txt
        /// </summary>
        string[] ItemDefinition;
        ArrayList SubItems;
        private void LoadItem(string[] itemContents)
        {
            ItemDefinition = itemContents;
            TypeName = Global.FindValue(ItemDefinition, "Type");

            Author = Global.FindValue(ItemDefinition, "Author");
            Description = Global.FindValue(ItemDefinition, "Description");
            if (Description != null)
                Description = Description.Replace("\\n", "\n    ");

            string[][] subItemDefinitions = Global.FindSubValues(ItemDefinition, "SubType");
            SubItems = new ArrayList();
            if (subItemDefinitions != null)
            {
                for (int i = 0; i < subItemDefinitions.Length; i++)
                {
                    if (Global.FindValue(subItemDefinitions[i], "Image") != null)
                        SubItems.Add(new SubItem(subItemDefinitions[i]));
                }
            }
            char[] parms = new char[1]; parms[0] = ';';
            string singleLineFilter = Global.FindValue(ItemDefinition, "Filter");
            singleLineFilter = singleLineFilter ?? "Miscellaneous";
            singleLineFilter += ";All Items";
            string[] split = singleLineFilter.Split(parms);
            Filters = (from s in split where s.Trim()!="" select s.Trim()).ToArray();

            //string[] basemodeui = File.ReadAllLines(Portal2Directory+resourceDirectory + "\\basemodui_" + language + ".txt");
            //name = Global.FindValue(itemContents, "Name");
            //string newname = Global.FindValue(basemodeui, name);
            //if (newname != null) { name = newname; }


            ////need to find multiple values, and create a list of sube items



            ////string imagepath = portal2Directory+"\\"+portal2_dlc2_materials + "\\puzzlemaker\\" + Global.FindValue(itemContents, "Image");


            ////((System.ComponentModel.ISupportInitialize)(previewImage)).BeginInit();
            ////previewImage.Location = new System.Drawing.Point(0, 0);
            ////previewImage.Name = Name_ToolTip;
            ////previewImage.Size = new System.Drawing.Size(64, 64);
            ////previewImage.TabIndex = 0;
            ////previewImage.TabStop = false;
            ////previewImage.SizeMode = PictureBoxSizeMode.StretchImage;
            //////previewImage.MouseClick += new System.Windows.Forms.MouseEventHandler(MouseClick);//we never use this, nor do I think it even works
            ////((System.ComponentModel.ISupportInitialize)(previewImage)).EndInit();
            //previewImage.Image = dlc2.GetPaletteImage(Global.FindValue(itemContents, "Image"));
            ////previewImage.Image = Image.FromFile(portal2Directory + "\\" + portal2_dlc2_materials + "\\puzzlemaker\\" + Global.FindValue(itemContents, "Image"));

            ////PreviewImage = Image.FromFile(portal2Directory + "\\" + portal2_dlc2_materials + "\\puzzlemaker\\" + Global.FindValue(itemContents, "Image"));

            ////ChangeValue(itemContents, "SomeProperty", "new value");
            ////File.WriteAllLines(itemDirectory + "\\" + fileName + individualItemExtension, itemContents);

        }








        ///// <summary>
        ///// This never worked, and it had to be done manually
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (MyHolder != null)
        //    {
        //        if (isSelected)
        //        {
        //            MyHolder.RemoveFromSelected(this);
        //            //draw me different
        //            isSelected = false;

        //        }
        //        else
        //        {
        //            MyHolder.AddToSelected(this);
        //            //draw me different
        //            isSelected = true;
        //        }

        //    }
        //}
        //private bool isSelected = false;



        /// <summary>
        /// The Name of the item as defined in the loaded resource file.
        /// Also will be the name which shows up in game when removing connections
        /// </summary>
        public string Name_ToolTip { get { return ((SubItem)SubItems[subcatagoryIndex]).Name; } }
        //private string name;

        /// <summary>
        /// The ToolTip of the item as defined in the loaded resource file.
        /// The name which shows up in game in the palette.
        /// </summary>
        public string ToolTip { get { return ((SubItem)SubItems[subcatagoryIndex]).Tooltip; } }
        //private string name;


        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(PuzzleItem)) return false;
            PuzzleItem temp = (PuzzleItem)obj;
            return temp.style == style && TypeName == temp.TypeName && temp.subcatagoryIndex == subcatagoryIndex;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }


        /// <summary>
        ///  Gets the Puzzle Item definition assuming that none of the subitems are on the palette
        /// </summary>
        /// <returns></returns>
        public string[] GetDefinition()
        {
            return GetDefinition(new int[0], new int[0]);
        }

        /// <summary>
        /// Gets the Puzzle Item definition based on which subitems will be on the palette and where
        /// </summary>
        /// <param name="activeSubItems">List of sub items to appear on the palette</param>
        /// <param name="subItemLocations">List of the location of each sub item relative to the int[] activeSubItems</param>
        /// <returns></returns>
        public string[] GetDefinition(int[] activeSubItems, int[] subItemLocations)
        {
            //sort these by activeSubItems

            bool redo = false;
            do
            {
                redo = false;
                int previous = -1;
                for (int i = 0; i < activeSubItems.Length; i++)
                {
                    if (previous > activeSubItems[i])
                    {
                        redo = true;
                        //swap
                        int temp = activeSubItems[i - 1];
                        activeSubItems[i - 1] = activeSubItems[i];
                        activeSubItems[i] = temp;

                        //also for subItemLocations
                        int temp2 = subItemLocations[i - 1];
                        subItemLocations[i - 1] = subItemLocations[i];
                        subItemLocations[i] = temp2;

                    }
                    previous = activeSubItems[i];
                }
            } while (redo);


            string[] body = new string[ItemDefinition.Length + 1];
            body[0] = "\"Item\"";
            ArrayList definition = new ArrayList();
            for (int i = 1; i < body.Length; i++)
            {
                body[i] = ItemDefinition[i - 1];
            }
            string[] values = new string[SubcatagoryIndexMax + 1];
            int subItemLocations_index = 0;
            for (int i = 0; i < values.Length; i++)
            {
                int gridWidth = 4;//palette width
                if (IntArrayContains(activeSubItems, i))
                    values[i] = (subItemLocations[subItemLocations_index] % gridWidth).ToString() + " " + (subItemLocations[subItemLocations_index++] / gridWidth).ToString() + " 0";
                else
                    values[i] = Global.NotOnPaletteEntry;
            }
            body = Global.ChangeSubValues(body, "Position", values);

            //replace if nessesary, the image
            //if there is only one subitem in use, and its possible to have more than one
            if (activeSubItems.Length == 1 && SubcatagoryIndexMax > 1)
            {
                //and this item has an alternate image
                int index = Global.PreferredImages_Names.IndexOf(this.TypeName);
                if (index >= 0)
                    body = Global.ChangeValue(body, "Image", (string)Global.PreferredImages_Paths[index], activeSubItems[0]);
            }



            return body;
        }
        private bool IntArrayContains(int[] body, int element)
        { foreach (int peice in body) { if (peice == element) return true; } return false; }

    }
        
}
