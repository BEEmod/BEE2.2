using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Linq;

namespace BEE2
{
    public partial class Form1 : Form
    {
        private bool readyToChangeStyle = false;
        public Form1()
        {
            InitializeComponent();
            panelItemHolderBucket.UpdateScrollbar += updateScrollbar;
            AddAnyEvents(this);

            this.panelItemHolderBucket.MouseWheel += new MouseEventHandler(panelItemHolderBucket_MouseWheel);

        }

        private void AddAnyEvents(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Tag != null && c.Tag is String && (c.Tag as String) != "")
                {
                    c.MouseEnter += any_MouseEnter;
                    c.MouseLeave += any_MouseLeave;
                }
                AddAnyEvents(c);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                LoadVBSPStyles();
                //if (!Global.CheckFilesInStylesChanger())
                //{
                //    MessageBox.Show("BEEMod 2 won't work when you run Portal 2, until you fix those problems.");
                //}
                //Global.ClearThumbnail();


                //panelItemHolderRecentlyUsed.ChangeGridSize(2, 8);
                //panelItemHolderBucket.ChangeGridSize(6,8);
                //panelItemHolderRecentlyUsed.InfiniteGrid = true;
                panelItemHolderPalette.OverloadDumpSite = panelItemHolderRecentlyUsed;
                panelItemHolderPalette.InitialOffsetPoint = new Point(26, 36);
                panelItemHolderPalette.BackgroundImage = Image.FromFile("visual_resources\\menu.png");
                //Form1.DefaultBackColor = Color.FromArgb(205, 208, 206);
                this.BackColor = Color.FromArgb(205, 208, 206);

                paletteSelectedItemLabel.Text = "";
                paletteSelectedItemLabel.BackColor = PanelItemHolder_Visual.Background_Fill_Color;

                panelItemHolderBucket.DeleteAtEnd = false;
                Global.PopulateWithAllItemsCollection(panelItemHolderBucket, "Clean", "");
                panelItemHolderBucket.scrollToRow(0);
                vbspStyleComboBox.Text = "Clean";
                itemFilterComboBox.Text = "All Items";

                //Set visual images
                removeButton.Image = Global.ScaleImage(Global.dlc2.GetPaletteImage("palette/toolbar_delete"), removeButton.Size.Width, removeButton.Size.Height);
                toggleSubTypeButton.Image = Global.ScaleImage(Global.dlc2.GetPaletteImage("palette/toolbar_rebuild"), toggleSubTypeButton.Size.Width, toggleSubTypeButton.Size.Height);

                //AddDefaultTypesWithBEELayout();
                readyToChangeStyle = true;

                LoadFilters();

                LoadPalettes();
                Global.LoadPreferredImages();
                aCustomContextMenu.PanelToInvalidate = panelItemHolderPalette;
                //this might be repeated someplace else
                vbspStyleComboBox_SelectedIndexChanged(this, new EventArgs());
                panelItemHolderBucket.Invalidate();
                panelItemHolderBucket.autoSize();
            }
            catch(Exception ex)
            {
                File.AppendAllText("exceptions.txt", "\n\n======================================================================\n\n" + ex.ToString());
            }

        }

        private void LoadFilters()
        {
            Global.LoadFilters();

            List<string> filters = File.ReadAllLines(Global.SettingsDirectory+"/filters_order.txt").ToList();
            int indexToInsert = -1;
            for (int i = 0; i < filters.Count; i++)
            {
                if (filters[i].Trim() == "[*]")
                {
                    indexToInsert = i;
                    break;
                }
            }
            if (indexToInsert == -1) indexToInsert = filters.Count;
            itemFilterComboBox.Items.Clear();
            itemFilterComboBox.Items.AddRange(filters.ToArray());
            itemFilterComboBox.Items.RemoveAt(indexToInsert);
            foreach (string k in Global.Filters.Keys)
            {
                if (!(from f in filters where f.Trim() == k select f).Any())
                    itemFilterComboBox.Items.Insert(indexToInsert, ((string)filters[indexToInsert].Clone()).Replace("[*]", k));
            }
            itemFilterComboBox.SelectedIndex = 0;
        }

        //private void AddDefaultTypesWithDefaultLayout()
        //{
        //    panelItemHolderPalette.RemoveAll();
        //    panelItemHolderPalette.AddType("ITEM_BUTTON_PEDESTAL", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_BUTTON_FLOOR", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_BUTTON_FLOOR", 1, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_BUTTON_FLOOR", 2, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_CUBE", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CUBE", 1, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CUBE", 3, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CUBE", 4, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_TBEAM", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CATAPULT", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LIGHT_BRIDGE", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PANEL_STAIRS", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_BARRIER_HAZARD", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_BARRIER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PISTON_PLATFORM", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_RAIL_PLATFORM", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_LASER_EMITTER_CENTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LASER_CATCHER_CENTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LASER_RELAY_CENTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CUBE", 2, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_PANEL_CLEAR", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PANEL_ANGLED", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PANEL_FLIP", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_SECONDARY_OBSERVATION_ROOM", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_BARRIER_HAZARD", 1, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_TURRET", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_GOO", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LIGHT_PANEL", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_PAINT_SPLAT", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PAINT_SPLAT", 1, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PAINT_SPLAT", 2, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PAINT_SPLAT", 3, panelItemHolderBucket);
        //}

        //private void AddDefaultTypesWithBEELayout()
        //{
        //    panelItemHolderPalette.RemoveAll();
        //    panelItemHolderPalette.AddType("ITEM_BUTTON_PEDESTAL", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_BUTTON_FLOOR", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CUBE", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PAINT_SPLAT", 3, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_TBEAM", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CATAPULT", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LIGHT_PANEL", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PANEL_STAIRS", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_BARRIER_HAZARD", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LIGHT_BRIDGE", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PISTON_PLATFORM", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_RAIL_PLATFORM", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_LASER_EMITTER_CENTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LASER_CATCHER_CENTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LASER_RELAY_CENTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_BARRIER", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_PANEL_CLEAR", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PANEL_ANGLED", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PANEL_FLIP", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_SECONDARY_OBSERVATION_ROOM", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_GOO", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_TURRET", 0, panelItemHolderBucket);
        //}

        //private void AddBEETypesWithBEELayout()
        //{
        //    panelItemHolderPalette.RemoveAll();
        //    panelItemHolderPalette.AddType("ITEM_BUTTON_PEDESTAL", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_BUTTON_FLOOR", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CUBE", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PAINT_SPLAT", 3, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_TBEAM", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CATAPULT", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_DRAW_BRIDGE", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PANEL_STAIRS", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_BARRIER_HAZARD", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LIGHT_BRIDGE", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PISTON_PLATFORM", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_RAIL_PLATFORM", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_LASER_EMITTER_CENTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LASER_CATCHER_CENTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LASER_RELAY_CENTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_BARRIER", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_PANEL_CLEAR", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PANEL_ANGLED", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_PANEL_FLIP", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_SECONDARY_OBSERVATION_ROOM", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_GOO", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_TURRET", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_CRUSHER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_VENT", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_HIGH_ENERGY_PELLET_EMITTER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_HIGH_ENERGY_PELLET_CATCHER", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("DOOR", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LIGHT_PANEL", 0, panelItemHolderBucket);

        //    panelItemHolderPalette.AddType("ITEM_TRIGGERS", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_BEE_LOGIC", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_AUTOPORTAL", 0, panelItemHolderBucket);
        //    panelItemHolderPalette.AddType("ITEM_LIGHT_PANEL_WARM", 0, panelItemHolderBucket);
        //}


        private void button4_Click(object sender, EventArgs e)
        {

            //AddDefaultTypesWithDefaultLayout();
            //PuzzleItem.PopulateWithAllItems(panelItemHolderPalette);
        }

        //private void panelItemHolderPalette_MouseClick(object sender, MouseEventArgs e)
        //{
        //    checkInnnn();
        //    if (panelItemHolderPalette.GetSelected().Count > 0)
        //    {
        //        //
        //    }
        //}
        //private void checkInnnn()
        //{
        //    label1.Text = innnn;
        //}
        //public static string innnn = "";


        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            //PuzzleItem.PopulateWithAllItems(panelItemHolderBucket);
            //PuzzleItem.PopulateWithAllItems(panelItemHolderBucket);
            Global.PopulateWithAllItemsCollection(panelItemHolderBucket, "Clean", "");
        }

        private void panelItemHolderPalette_MouseDown(object sender, MouseEventArgs e)
        {
            toolTip1.RemoveAll();
            paletteSelectedItemLabel.Text = panelItemHolderPalette.GetSelectedItemName();
        }

        private void scrollUpButton_Click(object sender, EventArgs e)
        {
            panelItemHolderBucket.scrollUp();
        }

        private void scrollDownButton_Click(object sender, EventArgs e)
        {
            panelItemHolderBucket.scrollDown();
        }
        public void updateScrollbar(int firstItemToDraw, int count, int GridWidth, int GridLength, int width, int height)
        {
            if (GridWidth > 0)
            {
                int v;
                BucketScrollbar.LargeChange = GridLength;
                BucketScrollbar.SmallChange = 1;
                if (count > 0)
                    BucketScrollbar.Maximum = (count - 1) / GridWidth;
                else
                    BucketScrollbar.Maximum = 0;
                v = firstItemToDraw / GridWidth;
                if (v != BucketScrollbar.Value) BucketScrollbar.Value = v;
                BucketScrollbar.Left = panelItemHolderBucket.Left + width + 12;
                BucketScrollbar.Height = height + 12;
            }
            //if (BucketScrollbar.LargeChange - 1 == BucketScrollbar.Maximum)
            //    BucketScrollbar.Enabled = false; //could also do the Visible property
            //else
            //    BucketScrollbar.Enabled = true;
        }

        private void MarkAsSender(PanelItemHolder panel)
        {
            lastClickedPanel = panel;
        }
        private object lastClickedPanel = null;
        private void panelItemHolderPalette_DragDrop(object sender, DragEventArgs e)
        {
            //todoFIXED: stop panel crash when something other than a PuzzleItem is dropped here
            if (!e.Data.GetDataPresent(typeof(PuzzleItem)))
                return;

            //todoFIXED: get them to drop into the right spot.... problem is that when something is dropped on the origin for the panel, it isn't the origin in the e variable
            //looks like the origin is 8,30...no idea why, maybe its different for other operating systems, or settings.

            //panelItemHolderPalette.getItemAt(new Point(e.X, e.Y));
            Point aPoint = new Point(e.X - this.Location.X - panelItemHolderPalette.Location.X - 8, e.Y - this.Location.Y - panelItemHolderPalette.Location.Y - 30);
            //if (lastClickedPanel!=null&&lastClickedPanel.Equals(panelItemHolderPalette))

            PuzzleItem aPuzzleItem = (PuzzleItem)e.Data.GetData(typeof(PuzzleItem));
            int numcheck = panelItemHolderPalette.GetNumberOfItems();

            if (lastClickedPanel.Equals(panelItemHolderPalette))
            {
                panelItemHolderPalette.RemoveItem(panelItemHolderPalette.GetSelectedItem());

                //special case if the palette is full, this caused the last item to bump off
                //if the panel was full
                //if (numcheck >= 32)
                //{
                //    //but now has less
                //    if (panelItemHolderPalette.GetNumberOfItems() == 31)
                //    {
                //        //then grab the first item from the recently used bucket and toss it back in here
                //        panelItemHolderPalette.InsertAfter(panelItemHolderRecentlyUsed.getItemNumber(0), 30);
                //        panelItemHolderRecentlyUsed.RemoveItemNumber(0);
                //    }
                //}

            }
            panelItemHolderPalette.DropItemOnPoint(aPuzzleItem, aPoint);

            panelItemHolderPalette.RemoveAllFromSelected();
            panelItemHolderPalette.AddToSelected(aPuzzleItem);
            paletteSelectedItemLabel.Text = panelItemHolderPalette.GetSelectedItemName();


            //Graphics gr = panelItemHolderPalette.CreateGraphics();
            //Pen aPen = new Pen(Color.Blue);
            //gr.DrawLine(aPen, new Point(e.X, e.Y), new Point(e.X, e.Y-20));

        }

        private void panelItemHolderPalette_DragEnter(object sender, DragEventArgs e)
        {
            //e.Effect = DragDropEffects.Move;
        }

        private void panelItemHolderPalette_DragLeave(object sender, EventArgs e)
        {
            panelItemHolderPalette.clearUpdateDraggingInsertElement();
        }
        private void panelItemHolderPalette_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(PuzzleItem)))
                return;
            e.Effect = DragDropEffects.Copy | DragDropEffects.Move; ;
            Point aPoint = new Point(e.X - this.Location.X - panelItemHolderPalette.Location.X - 8, e.Y - this.Location.Y - panelItemHolderPalette.Location.Y - 30);
            panelItemHolderPalette.updateDraggingInsertElement(aPoint);
            //PuzzleItem aPuzzleItem;
            //try
            //{
            //    aPuzzleItem = (PuzzleItem)e.Data.GetData(typeof(PuzzleItem));

            //    e.Effect = DragDropEffects.Move;
            //}
            //catch
            //{
            //    e.Effect = DragDropEffects.None;
            //}
            //if (e.Data.GetType() == typeof(PuzzleItem))


            //Pen aPen = new Pen(Color.Blue);
            //Graphics ex = panelItemHolderPalette.CreateGraphics(); 
            //ex.DrawLine(aPen, new Point(e.X, e.Y), new Point(e.X, e.Y-15));
        }

        private void panelItemHolderBucket_MouseMove(object sender, MouseEventArgs e)
        {
            //PuzzleItem aPuzzleItem = sender as PuzzleItem;
            PuzzleItem aPuzzleItem = panelItemHolderBucket.GetSelectedItem();
            if (aPuzzleItem != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    MarkAsSender(panelItemHolderBucket);
                    aPuzzleItem = aPuzzleItem.Clone();
                    DoDragDrop(aPuzzleItem, DragDropEffects.Copy);
                }
            }
            int x = e.X, y = e.Y;

            aPuzzleItem = panelItemHolderBucket.getItemAt(new Point(x, y));
            if (aPuzzleItem != null)
            {
                if (aPuzzleItem != oldpitem)
                {
                    //toolTip1.Show(aPuzzleItem.Name_ToolTip, panelItemHolderBucket, new Point(0, panelItemHolderBucket.Height-4), 2000);
                    toolTip1.Show(aPuzzleItem.Name_ToolTip, panelItemHolderBucket, new Point(0, panelItemHolderBucket.GridLength * 65 + 10), 2000);
                    oldpitem = aPuzzleItem;
                }
            }
            else
            {
                toolTip1.RemoveAll();
                oldpitem = null;
            }
        }

        private void panelItemHolderRecentlyUsed_MouseMove(object sender, MouseEventArgs e)
        {
            PuzzleItem aPuzzleItem = panelItemHolderRecentlyUsed.GetSelectedItem();
            if (aPuzzleItem != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    MarkAsSender(panelItemHolderRecentlyUsed);
                    aPuzzleItem = aPuzzleItem.Clone();
                    DoDragDrop(aPuzzleItem, DragDropEffects.Copy);
                }
            }

            panelItemHolderBucket_Focus();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            panelItemHolderPalette.RemoveItem(panelItemHolderPalette.GetSelectedItem());
        }

        CustomContextMenu aCustomContextMenu = new CustomContextMenu();
        private void panelItemHolderBucket_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    PuzzleItem aPuzzleItem = panelItemHolderBucket.getItemAt(e.Location);
            //    ////Build menu strip
            //    //contextMenuStrip1.Items.Clear();
            //    ////add the name
            //    //ToolStripMenuItem item = new ToolStripMenuItem(aPuzzleItem.Name_ToolTip);
            //    //item.Enabled = false;
            //    //contextMenuStrip1.Items.Add(item);

            //    //contextMenuStrip1.Items[0].Text = aPuzzleItem.Name_ToolTip;
            //    //ContextMenuStripSelecteditem = aPuzzleItem; 
            //    //contextMenuStrip1.Show(Cursor.Position);


            //    //aCustomContextMenu.Show(new Point(e.X + panelItemHolderBucket.Location.X, e.Y + panelItemHolderBucket.Location.Y));
            //    aCustomContextMenu.ShowItem(MousePosition,aPuzzleItem, panelItemHolderBucket.getOffsetOnPoint(e.Location));


            //}
        }


        protected override void OnGotFocus(EventArgs e)
        {
            aCustomContextMenu.Close();
            base.OnGotFocus(e);
        }


        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            PanelItemHolder.Control_Press = e.Control;
            if (e.KeyCode == Keys.Delete)
                panelItemHolderPalette.RemoveItem(panelItemHolderPalette.GetSelectedItem());
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            PanelItemHolder.Control_Press = e.Control;
        }

        private PuzzleItem oldpitem = null;
        private void panelItemHolderPalette_MouseMove(object sender, MouseEventArgs e)
        {
            PuzzleItem aPuzzleItem = panelItemHolderPalette.GetSelectedItem();
            if (aPuzzleItem != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    MarkAsSender(panelItemHolderPalette);
                    aPuzzleItem = aPuzzleItem.Clone();
                    DoDragDrop(aPuzzleItem, DragDropEffects.Move);
                }
            }
            int x = e.X, y = e.Y;
            aPuzzleItem = panelItemHolderPalette.getItemAt(new Point(x, y));
            if (aPuzzleItem != null)
            {
                if (aPuzzleItem != oldpitem)
                {
                    //toolTip1.Show(aPuzzleItem.Name_ToolTip, panelItemHolderPalette, new Point(paletteSelectedItemLabel.Left - panelItemHolderPalette.Left, paletteSelectedItemLabel.Top - panelItemHolderPalette.Top + 32), 2000);
                    toolTip1.Show(aPuzzleItem.Name_ToolTip, panelItemHolderPalette, new Point(paletteSelectedItemLabel.Left - panelItemHolderPalette.Left - 9, paletteSelectedItemLabel.Top - panelItemHolderPalette.Top + 32 + 40), 2000);
                    oldpitem = aPuzzleItem;
                }
            }
            else
            {
                toolTip1.RemoveAll();
                oldpitem = null;
            }

            panelItemHolderBucket_Focus();
        }

        private void panelItemHolderBucket_Focus()
        {
            aCustomContextMenu.Hide();
            panelItemHolderBucket.Focus();
        }
        //private void clickAnyWhereExcept_panelItemHolderBucket()
        //{
        //    aCustomContextMenu.Hide();
        //}

        private void panelItemHolderRecentlyUsed_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(PuzzleItem)))
                return;
            e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
        }

        private void panelItemHolderRecentlyUsed_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(PuzzleItem)))
                return;
            if (lastClickedPanel.Equals(panelItemHolderRecentlyUsed))
                panelItemHolderRecentlyUsed.RemoveItem(panelItemHolderRecentlyUsed.GetSelectedItem());
            PuzzleItem aPuzzleItem = (PuzzleItem)e.Data.GetData(typeof(PuzzleItem));
            panelItemHolderRecentlyUsed.InsertBefore(aPuzzleItem, 0);
            panelItemHolderRecentlyUsed.RemoveAllFromSelected();
            panelItemHolderRecentlyUsed.AddToSelected(aPuzzleItem);
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] itemsss = panelItemHolderPalette.BuildEditorItems(panelItemHolderBucket.PuzzleItems.ToList(), vbspStyleComboBox.Text);
            File.WriteAllLines(Global.Portal2Directory + "\\portal2_dlc2\\scripts\\editoritems.txt", panelItemHolderPalette.BuildEditorItems(panelItemHolderBucket.PuzzleItems.ToList(), vbspStyleComboBox.Text));
            if (Global.IsPortal2Running())
            {
                MessageBox.Show("Portal 2's PeTI style   items have been changed.\n\nBUT You need to exit and restart Portal 2 for the changes to take effect.");
            }
            else
            {
                MessageBox.Show("Portal 2's PeTI style and items have been changed.\n\nYou can now start Portal 2.");
            }
        }

        private void ToggleSubTypeButton_Click(object sender, EventArgs e)
        {
            panelItemHolderPalette.ToggleSelectedSubcatagory();
            paletteSelectedItemLabel.Text = panelItemHolderPalette.GetSelectedItemName();
            panelItemHolderPalette.Invalidate();
        }

        private void removeButton_MouseHover(object sender, EventArgs e)
        {

            //toolTip1.Show(removeButton.Tag.ToString(), this, new Point(Cursor.Position.X - this.Location.X, Cursor.Position.Y - this.Location.Y) , 2000); 
        }

        private void toggleSubTypeButton_MouseHover(object sender, EventArgs e)
        {
            //toolTip1.Show(toggleSubTypeButton.Tag.ToString(), this, new Point(Cursor.Position.X - this.Location.X, Cursor.Position.Y - this.Location.Y), 2000);
        }

        private void any_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Control && ((Control)sender).Tag is String)
                toolTip1.Show(((Control)sender).Tag.ToString(), ((Control)sender), new Point(((Control)sender).Width / 2, ((Control)sender).Height + 2), 2000);
        }

        private void any_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
        }

        private void panelItemHolderBucket_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.RemoveAll();
        }

        private void BucketScrollbar_Scroll(object sender, ScrollEventArgs e)
        {
            panelItemHolderBucket.scrollToRow(e.NewValue);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //AddBEETypesWithBEELayout();
        }

        private void LoadVBSPStyles()
        {
            Global.LoadVBSPStyles();
            Global.LoadItemStyles();

            //change the drop menu
            vbspStyleComboBox.Items.Clear();
            foreach (object style in Global.ListOfStyles)
            {
                vbspStyleComboBox.Items.Add(((VBSPStyle)style).Name);
            }
            vbspStyleComboBox.Text = ((VBSPStyle)Global.ListOfStyles[0]).Name;
        }

        private void vbspStyleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!readyToChangeStyle)
                return;

            Global.StyleName = vbspStyleComboBox.Text;

            // Remember what was in our palette
            string[] types = new string[panelItemHolderPalette.GetNumberOfItems()];
            int[] subtypes = new int[panelItemHolderPalette.GetNumberOfItems()];
            for (int i = 0; i < panelItemHolderPalette.GetNumberOfItems(); i++)
            {
                types[i] = panelItemHolderPalette.getItemNumber(i).TypeName;
                subtypes[i] = panelItemHolderPalette.getItemNumber(i).SubcatagoryIndex;
            }
            // Remember what was in the recently used bucket
            string[] typesR = new string[panelItemHolderRecentlyUsed.GetNumberOfItems()];
            int[] subtypesR = new int[panelItemHolderRecentlyUsed.GetNumberOfItems()];
            for (int i = 0; i < panelItemHolderRecentlyUsed.GetNumberOfItems(); i++)
            {
                typesR[i] = panelItemHolderRecentlyUsed.getItemNumber(i).TypeName;
                subtypesR[i] = panelItemHolderRecentlyUsed.getItemNumber(i).SubcatagoryIndex;
            }
            // Load new style's items
            panelItemHolderBucket.RemoveAll();
            Global.PopulateWithAllItemsCollection(panelItemHolderBucket, vbspStyleComboBox.Text, itemFilterComboBox.Text.Trim());
            // Put the new ones back in the palette
            panelItemHolderPalette.RemoveAll();
            for (int i = 0; i < types.Length; i++)
            {
                panelItemHolderPalette.AddType(types[i], subtypes[i], panelItemHolderBucket);
            }
            //Put new ones back the recently used bucket
            panelItemHolderRecentlyUsed.RemoveAll();
            for (int i = 0; i < typesR.Length; i++)
            {
                panelItemHolderRecentlyUsed.AddType(typesR[i], subtypesR[i], panelItemHolderBucket);
            }

            //change the style being used in the context menu strip
            aCustomContextMenu.CurrentStyle = Global.GetStyle(vbspStyleComboBox.Text);
            //panelItemHolderBucket.autoSize();
            panelItemHolderBucket.refreshScrollBar();
        }

        private void itemFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (!readyToChangeStyle)
            //     return;

            panelItemHolderBucket.Filter = itemFilterComboBox.Text;
            //just take what we have and filter it
            //List<PuzzleItem> everything = new List<PuzzleItem>();
            //for (int i = 0; i < panelItemHolderBucket.ListOfPuzzleItems.Count; i++)
            //{
            //    everything.Add((PuzzleItem)panelItemHolderBucket.ListOfPuzzleItems[i]);
            //}
            //for (int i = 0; i < panelItemHolderBucket.unfilteredPuzzleItems.Count; i++)
            //{
            //    everything.Add((PuzzleItem)panelItemHolderBucket.unfilteredPuzzleItems[i]);
            //}
            ////now filter
            //panelItemHolderBucket.ListOfPuzzleItems.Clear();
            //panelItemHolderBucket.unfilteredPuzzleItems.Clear();
            ////return;
            //if (Global.StyleName == null) return;
            //List<PuzzleItem> items = Global.GetStyle(Global.StyleName).BaseItems;
            //List<PuzzleItem> belong = new List<PuzzleItem>(from ev in everything where (from i in items where i.Equals(ev) select i).FirstOrDefault() != null select ev);
            //List<PuzzleItem> doesntbelong = new List<PuzzleItem>(from ev in everything where (from i in items where i.Equals(ev) select i).FirstOrDefault() == null select ev);
            //for (int i = 0; i < belong.Count; i++)
            //{
            //    panelItemHolderBucket.ListOfPuzzleItems.Add(belong[i]);
            //}
            //for (int i = 0; i < doesntbelong.Count; i++)
            //{
            //    panelItemHolderBucket.unfilteredPuzzleItems.Add(doesntbelong[i]);
            //}

            //panelItemHolderBucket.Invalidate();


            //panelItemHolderBucket.RemoveAll();
            //Global.PopulateWithAllItemsCollection(panelItemHolderBucket, vbspStyleComboBox.Text, itemFilterComboBox.Text.Trim());


            //// Remember what was in our palette
            //string[] types = new string[panelItemHolderPalette.GetNumberOfItems()];
            //int[] subtypes = new int[panelItemHolderPalette.GetNumberOfItems()];
            //for (int i = 0; i < panelItemHolderPalette.GetNumberOfItems(); i++)
            //{
            //    types[i] = panelItemHolderPalette.getItemNumber(i).TypeName;
            //    subtypes[i] = panelItemHolderPalette.getItemNumber(i).SubcatagoryIndex;
            //}
            //// Load new style's items
            //panelItemHolderBucket.RemoveAll();

            //Global.PopulateWithAllItemsCollection(panelItemHolderBucket, vbspStyleComboBox.Text, itemFilterComboBox.Text.Trim());
            //// Put the new ones back in the palette
            //panelItemHolderPalette.RemoveAll();
            //for (int i = 0; i < types.Length; i++)
            //{
            //    panelItemHolderPalette.AddType(types[i], subtypes[i], panelItemHolderBucket);
            //}
            //BucketScrollbar.LargeChange = panelItemHolderBucket.GridLength;
            //BucketScrollbar.SmallChange = 1;
            //if (panelItemHolderBucket.GetNumberOfItems() > 0)
            //    BucketScrollbar.Maximum = (panelItemHolderBucket.GetNumberOfItems() - 1) / panelItemHolderBucket.GridWidth;
            //else
            //    BucketScrollbar.Maximum = 0;
            //BucketScrollbar.Value = 0;

            //panelItemHolderBucket.autoSize();

            panelItemHolderBucket.refreshScrollBar();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { paletteLayoutComboBox.Text = "Empty"; }
            catch { }
        }

        private void LoadPalettes()
        {

            Global.LoadPalettes();
            paletteLayoutComboBox.Items.Clear();
            foreach (object palette in Global.ListOfPalettes)
            {
                paletteLayoutComboBox.Items.Add(((PaletteLayout)palette).Name);
            }
            foreach (object palette in Global.ListOfPalettes)
            {
                if (((PaletteLayout)palette).Name == "Portal 2")
                    paletteLayoutComboBox.Text = "Portal 2";//should use a prefrences setting here
            }
            if (paletteLayoutComboBox.Text == "")
            {
                if (paletteLayoutComboBox.Items.Count != 0)
                {
                    paletteLayoutComboBox.Text = (string)paletteLayoutComboBox.Items[0];
                }

            }
        }

        private void paletteLayoutComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (object palette in Global.ListOfPalettes)
            {
                if (((PaletteLayout)palette).Name == paletteLayoutComboBox.Text)
                {
                    ((PaletteLayout)palette).LoadPanel(panelItemHolderPalette, panelItemHolderBucket);
                    break;
                }
            }
        }
        private PromtForm aPromtForm;
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool nameisgood;
            //prompt for name 
            do
            {
                nameisgood = true;
                if (aPromtForm == null)
                    aPromtForm = new PromtForm("Palette Name", "", "Cancel", "Save Palette");
                aPromtForm.ShowDialog(this);
                if (aPromtForm.SafeData)
                {
                    foreach (object palette in Global.ListOfPalettes)
                    {
                        if (((PaletteLayout)palette).Name.Trim() == aPromtForm.ReturnValue.Trim())
                        {
                            nameisgood = false;
                            MessageBox.Show("That name is already taken, try something else", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                    break;

            } while (!nameisgood);
            if (aPromtForm.SafeData)
            {
                string name = aPromtForm.ReturnValue;

                //make sure a palette with that name doesn't already exist

                Global.SavePalette(name, name, panelItemHolderPalette.PuzzleItems.ToList());
                Global.LoadPalettes();
                paletteLayoutComboBox.Items.Clear();
                foreach (object palette in Global.ListOfPalettes)
                {
                    paletteLayoutComboBox.Items.Add(((PaletteLayout)palette).Name);
                }
                paletteLayoutComboBox.Text = name;
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog();
        }

        private void panelItemHolderBucket_SizeChanged(object sender, EventArgs e)
        {
            //todo: if at the bottom, scroll to the bottom

        }

        private void panelItemHolderBucket_MouseEnter(object sender, EventArgs e)
        {
            panelItemHolderBucket_Focus();
        }

        private void panelItemHolderBucket_MouseWheel(object sender, MouseEventArgs e)
        {
            //todo: stop mousescrolling from going too far
            int newvalue = BucketScrollbar.Value + (e.Delta > 0 ? -1 : 1);
            if (!(newvalue < 0 || newvalue > BucketScrollbar.Maximum - panelItemHolderBucket.GridLength + 1))
                BucketScrollbar.Value = newvalue;
        }

        private void BucketScrollbar_ValueChanged(object sender, EventArgs e)
        {
            panelItemHolderBucket.scrollToRow(BucketScrollbar.Value);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            panelItemHolderBucket_Focus();
        }

        PuzzleItem ContextMenuStripSelecteditem;
        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ContextMenuStripSelecteditem != null)
            {
                string NA = "N/A";
                MessageBox.Show("Name: " + ContextMenuStripSelecteditem.Name_ToolTip + Environment.NewLine +
                    "Author: " + (ContextMenuStripSelecteditem.Author == null ? NA : ContextMenuStripSelecteditem.Author) + Environment.NewLine +
                    "Description: " + (ContextMenuStripSelecteditem.Description == null ? NA : ContextMenuStripSelecteditem.Description));
            }
        }

        private void panelItemHolderPalette_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PuzzleItem aPuzzleItem = panelItemHolderPalette.getItemAt(e.Location);
                Point aPoint = panelItemHolderPalette.getOffsetOnPoint(e.Location);
                aCustomContextMenu.ShowItem(MousePosition, aPuzzleItem, new Point(aPoint.X + 6, aPoint.Y + 6), true);
            }
        }

        private void panelItemHolderRecentlyUsed_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PuzzleItem aPuzzleItem = panelItemHolderRecentlyUsed.getItemAt(e.Location);
                aCustomContextMenu.ShowItem(MousePosition, aPuzzleItem, panelItemHolderRecentlyUsed.getOffsetOnPoint(e.Location));
            }
        }


        PuzzleItem _itemForContextMenu = null;
        private void panelItemHolderBucket_MouseDown(object sender, MouseEventArgs e)
        {
            _itemForContextMenu = panelItemHolderBucket.getItemAt(e.Location);
        }

        private void panelItemHolderBucket_MouseUp(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right && panelItemHolderBucket.getItemAt(e.Location) == _itemForContextMenu)
            {
                PuzzleItem aPuzzleItem = panelItemHolderBucket.getItemAt(e.Location);
                aCustomContextMenu.ShowItem(MousePosition, aPuzzleItem, panelItemHolderBucket.getOffsetOnPoint(e.Location));
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}




//"Item"
//    {
//        "ItemClass"	"ItemButtonFloor"
//        "Type"		"ITEM_BEE_CCFLAG_LASERFIZZLER"
//        "Filter"		"BEE 2;BenVlodgi & Rantis"
//        "Editor"
//        {
//            "SubTypeProperty"		"ButtonType"
//            "SubType"
//            {
//                "Name"		"PORTAL2_PuzzleEditor_Item_bee_ccflag_laserfizzler1"
//                "Model"
//                {
//                    "ModelName"		"logic_and.3ds"
//                    "TextureName"	"cube.png"
//                }
//                "Palette"
//                {
//                    "Tooltip"		"LOGIC GATES"
//                    "Image"			"palette/logic.png"
//                    "Position"		"1 7 0"
//                }
//                "Sounds"
//                {
//                    "SOUND_CREATED"					"P2Editor.PlaceButton"
//                    "SOUND_EDITING_ACTIVATE"		"P2Editor.ExpandButton"
//                    "SOUND_EDITING_DEACTIVATE"		"P2Editor.CollapseButton"
//                    "SOUND_DELETED"					"P2Editor.RemoveButton"
//                }
//            }
			
//            "SubType"
//            {
//                "Name"		"PORTAL2_PuzzleEditor_Item_bee_ccflag_laserfizzler2"
//                "Model"
//                {
//                    "ModelName"		"logic_or.3ds"
//                    "TextureName"	"cuberedirection.png"
//                }
//                "Sounds"
//                {
//                    "SOUND_CREATED"					"P2Editor.PlaceButton"
//                    "SOUND_EDITING_ACTIVATE"		"P2Editor.ExpandButton"
//                    "SOUND_EDITING_DEACTIVATE"		"P2Editor.CollapseButton"
//                    "SOUND_DELETED"					"P2Editor.RemoveButton"
//                }
//            }
//            "SubType"
//            {
//                "Name"		"PORTAL2_PuzzleEditor_Item_bee_ccflag_laserfizzler3"
//                "Model"
//                {
//                    "ModelName"		"logic_not.3ds"
//                    "TextureName"	"cube.png"
//                }
//                "Sounds"
//                {
//                    "SOUND_CREATED"					"P2Editor.PlaceButton"
//                    "SOUND_EDITING_ACTIVATE"		"P2Editor.ExpandButton"
//                    "SOUND_EDITING_DEACTIVATE"		"P2Editor.CollapseButton"
//                    "SOUND_DELETED"					"P2Editor.RemoveButton"
//                }
//            }
//            "MovementHandle"	"HANDLE_NONE"
//        }
//        "Properties"
//        {
//            "ConnectionCount"
//            {
//                "DefaultValue"		"0"
//                "Index"		"1"
//            }
//            "ButtonType"
//            {
//                "DefaultValue"	"0"
//                "Index"			"0"
//            }
//        }
//        "Exporting"
//        {
//            "Instances"
//            {
//                "0"
//                {
//                    "Name"			"instances/bee2/ccflag_laserfizzler1.vmf"
//                    "EntityCount"	"0"
//                }
//                "1"
//                {
//                    "Name"			"instances/bee2/ccflag_laserfizzler1.vmf"
//                    "EntityCount"	"0"
//                }
//                "2"
//                {
//                    "Name"			"instances/bee2/ccflag_laserfizzler2.vmf"
//                    "EntityCount"	"0"
//                }
//                "3"
//                {
//                    "Name"			"instances/bee2/ccflag_laserfizzler2.vmf"
//                    "EntityCount"	"0"
//                }
//                "4"
//                {
//                    "Name"			"instances/bee2/ccflag_laserfizzler3.vmf"
//                    "EntityCount"	"0"
//                }
//                "5"
//                {
//                    "Name"			"instances/bee2/ccflag_laserfizzler3.vmf"
//                    "EntityCount"	"0"
//                }
//            }

//            "TargetName"		"ccflag_laserfizzler"
			
//            "Offset"	"64 64 64"
//        }
//    }
//    "Item"
//    {
//        "Filter"		"BEE 2;BenVlodgi & Rantis"
//        "Type"		"ITEM_CATAPULT_128"
//        "ItemClass"		"ItemCatapult"
//        "Editor"
//        {
//            "SubType"
//            {
//                "Name"		"PORTAL2_PuzzleEditor_Item_faith_plate"
//                "Model"
//                {
//                    "ModelName"		"faithplate.3ds"
//                    "TextureName"		"faithplate.png"
//                }
//                "Palette"
//                {
//                    "Tooltip"	"PORTAL2_PuzzleEditor_Palette_faith_plate"
//                    "Image"		"palette/faithplate.png"
//                    "Position"	"1 2 0"
//                }
//                "Sounds"
//                {
//                    "SOUND_CREATED"					"P2Editor.PlaceOther"
//                    "SOUND_EDITING_ACTIVATE"		"P2Editor.ExpandOther"
//                    "SOUND_EDITING_DEACTIVATE"		"P2Editor.CollapseOther"
//                    "SOUND_DELETED"					"P2Editor.RemoveOther"
//                }
//            }
//            "MovementHandle"		"HANDLE_CATAPULT"
//            //InvalidSurface"		"CEILING"
//        }
//        "Properties"
//        {
//            "CatapultSpeed"
//            {
//                "DefaultValue"		"500"
//                "Index"		"1"
//            }
//            "TargetName"
//            {
//                "DefualtValue"		"target"
//                "Index"		"2"
//            }
//            "VerticalAlignment"
//            {
//                "DefaultValue"		"0"
//                "Index"		"0"
//            }
//        }
//        "Exporting"
//        {
//            "Instances"
//            {
//                "0"
//                {
//                    "Name"				"instances/p2editor_clean/faith_plate_floor.vmf"
//                    "EntityCount"		"8"
//                    "BrushCount"		"3"
//                    "BrushSideCount"	"22"
//                }
//            }

//            "TargetName"		"fp"
//            "Offset"		"64 64 64"
//            "OccupiedVoxels"
//            {
//                "Voxel"
//                {
//                    "Pos"		"0 0 0"
//                    "CollideType"		"COLLIDE_SOLID"
//                    "CollideAgainst"	"COLLIDE_SOLID COLLIDE_GLASS COLLIDE_GRATING COLLIDE_ANTLINES"

//                    "Surface"
//                    {
//                        "Normal"	"0 0 1"
//                    }
//                }
//            }
//            "EmbeddedVoxels"
//            {
//                "Voxel"
//                {
//                    "Pos"		"0 0 0"
//                }
//            }
			
//            "EmbedFace"
//            {
//                "Solid"
//                {
//                    "Center"		"64 16 128"
//                    "Dimensions"	"128 32 4"
//                    "Grid"			"4x4"
//                }
//                "Solid"
//                {
//                    "Center"		"64 112 128"
//                    "Dimensions"	"128 32 4"
//                    "Grid"			"4x4"
//                }
//            }
//            "Overlay"
//            {
//                "Material"		"signage/signage_overlay_arrow"
//                "Center"		"16 16 0"
//                "Dimensions"	"32 32 0"
//                "Rotation"		"0"
//            }
//            "Overlay"
//            {
//                "Material"		"signage/signage_overlay_catapult1"
//                "Center"		"112 16 0"
//                "Dimensions"	"32 32 0"
//                "Rotation"		"180"
//            }
//            "Overlay"
//            {
//                "Material"		"signage/signage_overlay_catapult2"
//                "Center"		"112 112 0"
//                "Dimensions"	"32 32 0"
//                "Rotation"		"180"
//            }
//        }
//    }	