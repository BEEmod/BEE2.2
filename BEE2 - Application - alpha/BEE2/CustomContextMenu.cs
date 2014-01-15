using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BEE2
{
    public partial class CustomContextMenu : Form
    {
        public CustomContextMenu()
        {
            InitializeComponent();
        }
        protected override void OnLostFocus(EventArgs e)
        {
            Hide();
            base.OnLostFocus(e);
        }

        PuzzleItem ThePuzzleItem;
        public void ShowItem(Point aPoint, PuzzleItem aPuzzleItem, Point Offset, bool ShouldChangeActualItem)
        {
            if (aPuzzleItem != null)
            {
                if (!ShouldChangeActualItem)
                {
                    aPuzzleItem = aPuzzleItem.Clone();
                    ThePuzzleItem = null;
                }
                else
                    ThePuzzleItem = aPuzzleItem;

                ShowItem(aPoint, aPuzzleItem);
                aPoint = MousePosition;
                aPoint.X = aPoint.X - Offset.X - (aPuzzleItem.SubcatagoryIndex * 65);
                aPoint.Y = aPoint.Y - Offset.Y;
                if (aPoint.X < 0) aPoint.X = 0;
                if (aPoint.Y < 0) aPoint.Y = 0;

                //check to make sure it will appear on screen
                int screenHeight = Screen.FromControl(this).Bounds.Height;
                int screenWidth = Screen.FromControl(this).Bounds.Width;
                //MessageBox.Show(this.Location.X + " " + this.Location.Y);
                this.TopMost = false;
                Point aMousePoint = System.Windows.Forms.Cursor.Position;
                if (aPoint.X + Width > screenWidth || aPoint.X + Width<0)
                {
                    aPoint.X = screenWidth - Width;
                    this.TopMost = true;
                }
                if (aPoint.Y + Height > screenHeight || aPoint.Y + Height<0)
                {
                    aPoint.Y = screenHeight - Height;
                    this.TopMost = true;
                }

                aMousePoint = new Point(aPoint.X + Offset.X + aPuzzleItem.SubcatagoryIndex * 66, aPoint.Y + Offset.Y);
                System.Windows.Forms.Cursor.Position = aMousePoint;
                Location = aPoint;
            }
        }

        public void ShowItem(Point aPoint, PuzzleItem aPuzzleItem, Point Offset)
        {
            ShowItem(aPoint, aPuzzleItem, Offset, false);
        }

        public void ShowItem(Point aPoint, PuzzleItem aPuzzleItem)
        {

            Show();
            panelItemHolder_Visual1.PuzzleItems.Clear();
            panelItemHolder_Visual1.RemoveAllFromSelected();



            this.Location = aPoint;
            PuzzleItem aPuzzleItem2 = aPuzzleItem.Clone();
            aPuzzleItem2.SubcatagoryIndex = 0;
            for (int i = 0; i <= aPuzzleItem.SubcatagoryIndexMax; i++)
            {
                panelItemHolder_Visual1.PuzzleItems.Add(aPuzzleItem2.Clone());
                aPuzzleItem2.ToggleSubcatagory();
            }
            panelItemHolder_Visual1.AddToSelected(aPuzzleItem.SubcatagoryIndex);


            panelItemHolder_Visual1.Width = 337 - (4 - aPuzzleItem.SubcatagoryIndexMax) * 64;


            UpdateProperties(aPuzzleItem2);
        }

        private void CustomContextMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// Panel to invalidate when changing the subitem index
        /// </summary>
        public PanelItemHolder PanelToInvalidate;
        private void panelItemHolder_Visual1_MouseClick(object sender, MouseEventArgs e)
        {
            if (ThePuzzleItem != null && panelItemHolder_Visual1.GetSelectedItem() != null)
            {
                ThePuzzleItem.SubcatagoryIndex = panelItemHolder_Visual1.GetSelectedItem().SubcatagoryIndex;
                if (PanelToInvalidate != null)
                    PanelToInvalidate.Invalidate();
            }
            UpdateProperties(panelItemHolder_Visual1.GetSelectedItem());
        }

        public VBSPStyle CurrentStyle = null;
        private void UpdateProperties(PuzzleItem aPuzzleItem)
        {
            string NA = "N/A";
            if (aPuzzleItem != null && panelItemHolder_Visual1.GetSelectedItem() != null)
            {
                label5.Text = aPuzzleItem.TypeName;
                label6.Text = aPuzzleItem.Name_ToolTip;
                label7.Text = aPuzzleItem.Author;
                itemDescriptionTextBox.Text = aPuzzleItem.Description;
                if (CurrentStyle != null)
                {
                    label11.Text = CurrentStyle.Name;
                    label12.Text = CurrentStyle.Author;
                    styleDescriptionTextBox.Text = CurrentStyle.Comment;
                }
                else
                {
                    label11.Text = NA;
                    label12.Text = NA;
                    styleDescriptionTextBox.Text = NA;
                }
                //label11.Text = 
            }
            else
            {
                label5.Text = NA;
                label6.Text = NA;
                label7.Text = NA;
                itemDescriptionTextBox.Text = NA;
                if (CurrentStyle != null)
                {
                    label11.Text = CurrentStyle.Name;
                    label12.Text = CurrentStyle.Author;
                    styleDescriptionTextBox.Text = CurrentStyle.Comment;
                }
                else
                {
                    label11.Text = NA;
                    label12.Text = NA;
                    styleDescriptionTextBox.Text = NA;
                }
            }
            itemDescriptionTextBox.Select(0,0);
            styleDescriptionTextBox.Select(0,0);
        }

    }
}
