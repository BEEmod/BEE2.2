using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;
using System.IO;
namespace BEE2
{
    public class PanelItemHolder_Visual : PanelItemHolder
    {
        private static Color right_line = Color.FromArgb(83, 84, 83);
        private static Color top_line = Color.FromArgb(102, 103, 102);
        private static Color left_line = Color.FromArgb(123, 123, 123);
        private static Color bottom_line = left_line;
        private static Color bottomshadow = Color.FromArgb(173, 175, 174);
        private static Color rightshadow = Color.FromArgb(170, 172, 170);

        private static Color background_fill = Color.FromArgb(245, 245, 245);
        public static Color Background_Fill_Color { get { return background_fill; } }
        private static Color background_items_fill = Color.FromArgb(230, 232, 233);
        private static Color background_items_lines = Color.FromArgb(220, 220, 220);

        protected Point upper_right_point;
        protected Point upper_left_point;
        protected Point lower_right_point;
        protected Point lower_left_point;

        int firstload = 5;
        override public void ChangeGridSize(int length, int width)
        {
            if (gridLength != length || gridWidth != width || firstload>0)
            {
                base.ChangeGridSize(length, width);
                setUIPoints();
                firstload--;
            }
        }

        //placing this here, because the constructor wont set for some reason
        new Point InitialOffsetPoint = new Point(6,6);
        public PanelItemHolder_Visual()
        {
            setUIPoints();
            PuzzleItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PuzzleItems_CollectionChanged);
        }

        void PuzzleItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (UpdateScrollbar != null)
                UpdateScrollbar(0, _dispalyablePuzzleItems.Count, gridWidth, gridLength, (ItemsImageSize + PaddingBetweenItems) * gridWidth - PaddingBetweenItems, (ItemsImageSize + PaddingBetweenItems) * gridLength - PaddingBetweenItems);
        }
        //public Image HeaderImage;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!HoldOffOnPainting)
                update(e.Graphics);
        }

        private bool HoldOffOnPainting = false;

        public override void AddToSelected(List<PuzzleItem> value)
        {
            HoldOffOnPainting = true;
            base.AddToSelected(value);
            HoldOffOnPainting = false;
        }
        public override void AddToSelected(PuzzleItem value)
        {
            HoldOffOnPainting = true;
            base.AddToSelected(value);
            HoldOffOnPainting = false;
        }
        public override void RemoveFromSelected(List<PuzzleItem> value)
        {
            HoldOffOnPainting = true;
            base.RemoveFromSelected(value);
            HoldOffOnPainting = false;
        }
        public override void RemoveFromSelected(PuzzleItem value)
        {
            HoldOffOnPainting = true;
            base.RemoveFromSelected(value);
            HoldOffOnPainting = false;
        }





        protected void setUIPoints()
        {
            upper_left_point = new Point(0, 0);
            upper_right_point = new Point(gridWidth * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.X * 2-2, 0);
            lower_left_point = new Point(0, gridLength * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.Y*2-2);
            lower_right_point = new Point(gridWidth * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.X * 2-2, gridLength * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.Y*2-2);
        
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (AutoSizePalette)
            {
                int old = gridSize;
                autoSize();
                if (old != gridSize)
                    Invalidate();
            }
            //recalculateGridDrawRange();
        }
        public bool AutoSizePalette = true;

        public override void autoSize()
        {
            ChangeGridSize((Height - InitialOffsetPoint.Y * 2) / (ItemsImageSize + PaddingBetweenItems), (Width - InitialOffsetPoint.X * 2) / (ItemsImageSize + PaddingBetweenItems));
            if (UpdateScrollbar != null)
                UpdateScrollbar(gridSize<=_dispalyablePuzzleItems.Count?0:firstItemToDraw, _dispalyablePuzzleItems.Count, gridWidth, gridLength, (ItemsImageSize + PaddingBetweenItems) * gridWidth - PaddingBetweenItems, (ItemsImageSize + PaddingBetweenItems) * gridLength - PaddingBetweenItems);
            
        }

        //protected int firstItemToDraw = 0;
        public void scrollUp()
        {
            if (firstItemToDraw != 0)
            {
                if (firstItemToDraw < GridWidth)
                    firstItemToDraw = 0;
                else
                    firstItemToDraw -= GridWidth;
                Invalidate();
            }
            if (UpdateScrollbar != null)
                UpdateScrollbar(firstItemToDraw, _dispalyablePuzzleItems.Count, gridWidth, gridLength, (ItemsImageSize + PaddingBetweenItems) * gridWidth - PaddingBetweenItems, (ItemsImageSize + PaddingBetweenItems) * gridLength - PaddingBetweenItems);
        }
        public void scrollDown()
        {
            if (firstItemToDraw != _dispalyablePuzzleItems.Count)
            {
                if (firstItemToDraw + GridWidth > _dispalyablePuzzleItems.Count)
                    ;//firstItemToDraw = aListOfPuzzleItems.Count - 1;
                else
                {
                    firstItemToDraw += GridWidth;
                    Invalidate();
                }
            }
            if (UpdateScrollbar != null)
                UpdateScrollbar(firstItemToDraw, _dispalyablePuzzleItems.Count, gridWidth, gridLength, (ItemsImageSize + PaddingBetweenItems) * gridWidth - PaddingBetweenItems, (ItemsImageSize + PaddingBetweenItems) * gridLength - PaddingBetweenItems);
        }
        public void scrollToRow(int row)
        {
            if (_dispalyablePuzzleItems.Count == 0)
                row = 0;
            else if (row * GridWidth >= _dispalyablePuzzleItems.Count)
                row = (_dispalyablePuzzleItems.Count - 1) / GridWidth;
            if (row * GridWidth != firstItemToDraw)
            {
                firstItemToDraw = row * GridWidth;
                Invalidate();
            }
            if (UpdateScrollbar != null)
                UpdateScrollbar(firstItemToDraw, _dispalyablePuzzleItems.Count, gridWidth, gridLength, (ItemsImageSize + PaddingBetweenItems) * gridWidth - PaddingBetweenItems, (ItemsImageSize + PaddingBetweenItems) * gridLength - PaddingBetweenItems);
        }
        public delegate void TUpdateScrollbar(int firstItemToDraw, int count, int GridWidth, int GridLength, int Width, int Height);
        public TUpdateScrollbar UpdateScrollbar;

        override public void update(Graphics e)
        {
            if (e==null)
              e = this.CreateGraphics();
            PuzzleItem aPuzzlemakerItem;
            Pen aPen = new Pen(Color.Black);
            SolidBrush aBrush;
            //draw the back of the panel area
            aBrush = new SolidBrush(background_fill);
            e.FillRectangle(aBrush, upper_left_point.X, upper_left_point.Y, Distance(upper_left_point, upper_right_point), Distance(upper_left_point, lower_left_point));
            //draw the area where items go
            aBrush = new SolidBrush(background_items_fill);
            e.FillRectangle(aBrush, InitialOffsetPoint.X, InitialOffsetPoint.Y, (ItemsImageSize + PaddingBetweenItems) * gridWidth - PaddingBetweenItems, (ItemsImageSize + PaddingBetweenItems) * gridLength - PaddingBetweenItems);
            //draw all the grid lines
            aPen.Color = background_items_lines;

            for (int i = 1; i < gridWidth; i++)
            {
                e.DrawLine(aPen, InitialOffsetPoint.X - 1 + (ItemsImageSize + PaddingBetweenItems) * i, InitialOffsetPoint.Y - 1, InitialOffsetPoint.X - 1 + (ItemsImageSize + PaddingBetweenItems) * i, InitialOffsetPoint.Y - PaddingBetweenItems * 2 + (PaddingBetweenItems + ItemsImageSize) * gridLength);
            }
            for (int i = 1; i < gridLength; i++)
            {
                e.DrawLine(aPen, InitialOffsetPoint.X - 1, InitialOffsetPoint.Y - 1 + (ItemsImageSize + PaddingBetweenItems) * i, InitialOffsetPoint.X - PaddingBetweenItems * 2 + (PaddingBetweenItems + ItemsImageSize) * gridWidth, InitialOffsetPoint.Y - 1 + (ItemsImageSize + PaddingBetweenItems) * i);
            }

            //draw both left, and bottom line, using the same pen, since they are the same color
            aPen.Color = bottom_line;
            e.DrawLine(aPen, lower_right_point, lower_left_point);
            e.DrawLine(aPen, upper_left_point, lower_left_point);

            //draw the right line
            aPen.Color = (right_line);
            e.DrawLine(aPen, lower_right_point, upper_right_point);
            //draw the top line
            aPen.Color = (top_line);
            e.DrawLine(aPen, upper_left_point, upper_right_point);


            //draw items which are within range

            rangeBegin = firstItemToDraw;
            rangeEnd = rangeBegin+gridSize;
            if (_dispalyablePuzzleItems.Count < gridSize)
                rangeEnd = _dispalyablePuzzleItems.Count;

            //for (int i = 0; i < aListOfPuzzleItems.Count; i++)

            //try
            //{
                for (int i = rangeBegin; i < rangeEnd; i++)
                {
                    if (i < _dispalyablePuzzleItems.Count)
                    {
                        aPuzzlemakerItem = _dispalyablePuzzleItems[i];
                        lock (aPuzzlemakerItem.PreviewImage)
                        {
                            e.DrawImage(aPuzzlemakerItem.PreviewImage, ((i - rangeBegin) % gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.X, ((i - rangeBegin) / gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.Y, 64f, 64f);
                        }
                        
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    File.AppendAllText("error.txt.", ex.ToString() + Environment.NewLine + Environment.NewLine);
            //}

            aPen.Color = selectedItemBoxColor;
            aPen.Width = 3;
            for (int i = 0; i < aListOfSelectedItems.Count; i++)
            {
                int location = _dispalyablePuzzleItems.IndexOf((PuzzleItem)aListOfSelectedItems[i]) - rangeBegin;
                if (location + rangeBegin >= rangeBegin && location + rangeBegin < rangeEnd)
                    e.DrawRectangle(aPen, (location % gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.X, (location / gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.Y, 64, 64);
            }
        }
        //private void recalculateGridDrawRange(object sender, ScrollEventArgs e)
        //{
        //    recalculateGridDrawRange();
        //}
        //private void recalculateGridDrawRange()
        //{


        //}
        protected int rangeBegin;
        protected int rangeEnd;

        


        /// <summary>
        /// Finds the distance between two points on a 2D surface.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public int Distance(Point A, Point B)
        {
            return (int)Math.Sqrt(Math.Pow((B.X - A.X), 2) + Math.Pow((B.Y - A.Y), 2));
            
        }
    }
}
