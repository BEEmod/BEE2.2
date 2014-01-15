using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using System.ComponentModel;
using System.Drawing;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Linq;
using System.Collections.ObjectModel;

namespace BEE2
{
    public class PanelItemHolder : Panel
    {
        public PanelItemHolder OverloadDumpSite;
        //protected ArrayList aListOfPuzzleItems;
        //public ArrayList unfilteredPuzzleItems;
        //public ArrayList ListOfPuzzleItems { get { return aListOfPuzzleItems; } }

        private string _filter = "All Items";
        public string Filter { get { return _filter; } set { if (value == null)_filter = "All Items"; else _filter = value; RunFiltering(); } }

        private ObservableCollection<PuzzleItem> _puzzleItems;
        public ObservableCollection<PuzzleItem> PuzzleItems
        {
            get { return _puzzleItems; }
            set
            {
                _puzzleItems = value;
                RunFiltering();
            }
        }

        private void RunFiltering()
        {
            //Size cool = this.PreferredSize;
            List<string> filteredItemss = Global.GetItemsFromFilter(_filter.Trim());
            //if (_filter != null) _dispalyablePuzzleItems = new List<PuzzleItem>(from pi in _puzzleItems where pi.Filters.Contains(_filter) select pi).ToList();
            //else _dispalyablePuzzleItems = new List<PuzzleItem>(from pi in _puzzleItems select pi).ToList();

            if (_filter != null) _dispalyablePuzzleItems = new List<PuzzleItem>(from pi in _puzzleItems where filteredItemss!=null && filteredItemss.Contains(pi.TypeName) select pi).ToList();
            else _dispalyablePuzzleItems = new List<PuzzleItem>(from pi in _puzzleItems select pi).ToList();

            Invalidate();

        }
        

        protected List<PuzzleItem> _dispalyablePuzzleItems = new List<PuzzleItem>();

        //public bool InfiniteGrid = false;
        public static int PaddingBetweenItems = 1;
        public Point InitialOffsetPoint { get; set; }
        public static int ItemsImageSize = 64;
        public int GridLength { get { return gridLength; } set { gridLength = value; gridSize = gridWidth * gridLength; } }
        protected int gridLength;
        public int GridWidth { get { return gridWidth; } set { gridWidth = value; gridSize = gridWidth * gridLength; } }
        protected int gridWidth;
        protected int gridSize;

        protected static Color selectedItemBoxColor = Color.Black;
        //public string BackgroundImage_location
        //{
        //    set
        //    {
        //        backgroundImage = new PictureBox();
        //        ((System.ComponentModel.ISupportInitialize)(backgroundImage)).BeginInit();
        //        backgroundImage.Location = new System.Drawing.Point(0, 0);
        //        backgroundImage.Name = "background";
        //        backgroundImage.Size = new System.Drawing.Size(64, 64);
        //        backgroundImage.TabIndex = 0;
        //        backgroundImage.TabStop = false;
        //        backgroundImage.SizeMode = PictureBoxSizeMode.StretchImage;
        //        backgroundImage.MouseClick += new System.Windows.Forms.MouseEventHandler(Background_MouseClick);
        //        ((System.ComponentModel.ISupportInitialize)(backgroundImage)).EndInit();
        //        backgroundImage.Image = Image.FromFile("visual_resources\\" + value);
        //    }
        //}
        //private PictureBox backgroundImage;
        //private void Background_MouseClick(object sender, MouseEventArgs e)
        //{
        //}

        void Focus()
        {
            base.Focus();
            OnGotFocus(new EventArgs());
        }

        public virtual void autoSize()
        {
            //assume default size of palette
            gridWidth = 4;
            gridLength = 8;
            gridSize = gridLength * gridWidth;
        }

        public void BuildNewPalette()
        {
            gridSize = gridLength * gridWidth;
            _puzzleItems = new ObservableCollection<PuzzleItem>();
            //_puzzleItems.
            //aListOfPuzzleItems = new ArrayList(gridSize);
            //aListOfPuzzleItems.TrimToSize();
            InitialOffsetPoint = new Point(0, 0);
            Invalidate();
        }
        public PanelItemHolder()
        {
            InitializeComponent();//not sure if needed


            DoubleBuffered = true;
            levelofdrawing = 0;
            autoSize();
            BuildNewPalette();
            aListOfSelectedItems = new ObservableCollection<PuzzleItem>();
            //unfilteredPuzzleItems = new ArrayList();

            PuzzleItems.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(PuzzleItems_CollectionChanged);
        }

        void PuzzleItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            //List<PuzzleItem> filteredItems = new List<PuzzleItem>();
            //_dispalyablePuzzleItems = (from pi in PuzzleItems where Filter.Contains(pi) select pi).ToList();

            RunFiltering();
        }
        public virtual void ChangeGridSize(int length, int width)
        {
            this.gridWidth = width;
            this.gridLength = length;
            gridSize = gridWidth * gridLength;
            // force scrolling to only allow multiples of rows
            // our code doesn't strictly need it, but it looks better
            if (gridWidth > 0)
                firstItemToDraw = (firstItemToDraw / gridWidth) * gridWidth;
            //BuildNewPalette();
        }
        public bool DeleteAtEnd = true;
        public void InsertBefore(PuzzleItem item, int slot)
        {
            if (slot >= 32)
            {
                if (OverloadDumpSite != null)
                {
                    OverloadDumpSite.InsertBefore(item, 0);
                }
            }
            else
            {
                if (DeleteAtEnd)
                {

                    if (PuzzleItems.Count >= gridSize)
                    {
                        if (OverloadDumpSite != null)
                        {
                            OverloadDumpSite.InsertBefore(PuzzleItems[gridSize - 1], 0);
                        }
                        PuzzleItems.Remove(PuzzleItems[gridSize - 1]);
                    }
                    if (PuzzleItems.Count >= gridSize)
                    {
                        try
                        {
                            PuzzleItems.RemoveAt(gridSize);
                            foreach (PuzzleItem anItem in aListOfSelectedItems)
                            {
                                if (!PuzzleItems.Contains(anItem))
                                    aListOfSelectedItems.Remove(anItem);
                            }
                        }
                        catch { /*happens when window is resized (sometimes... and things break) */}
                    }
                }
                PuzzleItems.Insert(slot, item);//todo: fix crash when Item dragged too deep onto the palette
            }
            Invalidate();
        }
        public void InsertAfter(PuzzleItem item, int slot)
        {
            if (DeleteAtEnd)
            {
                if (PuzzleItems.Count >= gridSize)
                {
                    if (OverloadDumpSite != null)
                    {
                        OverloadDumpSite.InsertBefore(PuzzleItems[gridSize - 1], 0);
                    }
                    PuzzleItems.Remove(PuzzleItems[gridSize - 1]);
                }
                if (PuzzleItems.Count >= gridSize)
                {
                    try
                    {
                        PuzzleItems.RemoveAt(gridSize);
                        foreach (PuzzleItem anItem in aListOfSelectedItems)
                        {
                            if (!PuzzleItems.Contains(anItem))
                                aListOfSelectedItems.Remove(anItem);
                        }
                    }
                    catch { /*happens when window is resized (sometimes)*/}
                }
            }
            PuzzleItems.Insert(slot + 1, item);
            Invalidate();
        }
        /// <summary>
        /// removes all items from the palette
        /// </summary>
        /// <param name="aPuzzleItem"></param>
        public void RemoveAll()
        {
            PuzzleItems.Clear();
            aListOfSelectedItems.Clear();
            PuzzleItems.Clear();
            Invalidate();
        }
        /// <summary>
        /// removes the designated item from the palette if it exists
        /// </summary>
        /// <param name="aPuzzleItem"></param>
        public void RemoveItem(PuzzleItem aPuzzleItem)
        {
            PuzzleItems.Remove(aPuzzleItem);
            aListOfSelectedItems.Remove(aPuzzleItem);//dont think this is nessesary
            Invalidate();
        }
        public void RemoveItemNumber(int index)
        {
            if (PuzzleItems.Count > 0)
                PuzzleItems.RemoveAt(index);
            if (aListOfSelectedItems.Count > 0)
                aListOfSelectedItems.Remove(PuzzleItems[index]);//dont think this is nessesary
            Invalidate();
        }
        //public int pX = -1;
        //public int pY = -1;
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            //pX = drgevent.X;
            //pY = drgevent.Y;
            base.OnDragDrop(drgevent);
        }
        public PuzzleItem getItemNumber(int value)
        {
            PuzzleItem aPuzzleItem = PuzzleItems[value];
            return aPuzzleItem.Clone();
        }



        protected int levelofdrawing;

        public virtual void update(Graphics e)
        {
            if (levelofdrawing == 0)
            {
                //bool anItemIsSelected=false;
                if (e == null)
                    e = this.CreateGraphics();
                PuzzleItem aPuzzlemakerItem;
                //if (BackgroundImage != null)
                //    e.DrawImage(BackgroundImage, 0, 0);
                for (int i = 0; i < PuzzleItems.Count; i++)
                {
                    aPuzzlemakerItem = PuzzleItems[i];
                    e.DrawImage(aPuzzlemakerItem.PreviewImage, (i % gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.X, (i / gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.Y, 64f, 64f);
                }
                //Draw selected items
                Pen aPen = new Pen(selectedItemBoxColor);
                aPen.Width = 3;
                for (int i = 0; i < aListOfSelectedItems.Count; i++)
                {
                    if (aListOfSelectedItems[i] != null)
                    {
                        int location = PuzzleItems.IndexOf((PuzzleItem)aListOfSelectedItems[i]);
                        if ((location % gridWidth) >= 0 && (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.X >= 0)//todo: these were just >0 changed to fix bug, we'll see
                        {
                            e.DrawRectangle(aPen, (location % gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.X, (location / gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.Y, 64, 64);
                        }
                    }
                }
                if (dragging_insert_slot >= -1)
                    update_insertOverlay(e);
            }
        }
        private int dragging_insert_slot = -2;
        protected int maxElements = 32;
        protected void update_insertOverlay(Graphics e)
        {
            int location = dragging_insert_slot;
            if (location < maxElements)
            {
                Pen aPen = new Pen(Color.BlueViolet, 3);
                int x = (location % gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.X;
                int y1 = (location / gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.Y;
                int y2 = y1 + 64;
                e.DrawLine(aPen, x, y1, x, y2);
            }
        }
        public string outtt { get; set; }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }


        protected ObservableCollection<PuzzleItem> aListOfSelectedItems;
        /// <summary>
        /// Adds the given PuzzleItem to the holders list of selected items
        /// </summary>
        /// <param name="value"></param>
        public virtual void AddToSelected(PuzzleItem value)
        {
            aListOfSelectedItems.Insert(0, value);
        }

        /// <summary>
        /// Adds the indicated item from the ListOfPuzzleItems
        /// </summary>
        /// <param name="index"></param>
        public virtual void AddToSelected(int index)
        {
            aListOfSelectedItems.Insert(0, (PuzzleItems[index]));
        }
        /// <summary>
        /// Adds the given PuzzleItems to the holders list of selected items
        /// </summary>
        /// <param name="value"></param>
        public virtual void AddToSelected(List<PuzzleItem> value)
        {
            foreach (PuzzleItem v in value)
            {
                aListOfSelectedItems.Add(v);
            }
        }
        /// <summary>
        /// Removes the given PuzzleItem from the holders list of selected items
        /// </summary>
        /// <param name="value"></param>
        public virtual void RemoveFromSelected(PuzzleItem value)
        {
            aListOfSelectedItems.Remove(value);
        }/// <summary>
        /// Removes the given PuzzleItems from the holders list of selected items
        /// </summary>
        /// <param name="value"></param>
        public virtual void RemoveFromSelected(List<PuzzleItem> value)
        {
            foreach (PuzzleItem v in value)
            {
                aListOfSelectedItems.Remove(v);
            }
        }
        /// <summary>
        /// Removes all PuzzleItems from the list of selected items
        /// </summary>
        public virtual void RemoveAllFromSelected()
        {
            aListOfSelectedItems = new ObservableCollection<PuzzleItem>();
        }
        public virtual ObservableCollection<PuzzleItem> GetSelected()
        {
            return aListOfSelectedItems;
        }
        public virtual PuzzleItem GetSelectedItem()
        {
            if (aListOfSelectedItems.Count > 0)
                return (PuzzleItem)aListOfSelectedItems[0];
            else return null;
        }
        public virtual int GetSelectedItemIndex()
        {
            if (aListOfSelectedItems.Count > 0)
            {
                for (int i = 0; i < PuzzleItems.Count; i++)
                {
                    if (PuzzleItems[i].Equals(aListOfSelectedItems[0]))
                        return i;
                }
            }
            return -1;
        }
        public virtual string GetSelectedItemName()
        {
            if (aListOfSelectedItems.Count != 0)
                //return aListOfSelectedItems.Count.ToString();
                return GetSelectedItem().Name_ToolTip;
            else return "";
        }
        public virtual bool UnSelectItems()
        {
            if (PuzzleItems != null && aListOfSelectedItems.Count > 0)
            {
                aListOfSelectedItems = new ObservableCollection<PuzzleItem>();
                return true;
            }
            return false;
        }

        public static bool Control_Press = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            PuzzleItem aPuzzleItem = getItemAt(new Point(e.X, e.Y));
            if (aPuzzleItem == null)
            {
                if (UnSelectItems())
                    Invalidate();
            }
            else
            {

                //if (!Control_Press)
                //    UnSelectItems();

                if (aListOfSelectedItems.Contains(aPuzzleItem))
                {
                    //aListOfSelectedItems.Remove(aPuzzleItem);
                    //Invalidate();
                }
                else
                {
                    UnSelectItems();
                    AddToSelected(aPuzzleItem);
                    Invalidate();
                }
            }

            base.OnMouseDown(e);
        }

        public int GetNumberOfItems()
        {
            return PuzzleItems.Count;
        }

        public PuzzleItem getItemByType(string t, int subitem)
        {
            //foreach (PuzzleItem item in unfilteredPuzzleItems)
            //{
            //    if (item.TypeName == t && item.SubcatagoryIndex == subitem)
            //    {
            //        return item;
            //    }
            //}
            foreach (PuzzleItem item in PuzzleItems)
            {
                if (item.TypeName == t && item.SubcatagoryIndex == subitem)
                {
                    return item;
                }
            }
            return null;
        }

        public void DropItemOnPoint(PuzzleItem aPuzzleItem, Point aPoint)
        {
            //todoFIXED: make this precise

            int i = getItemSlotBetween(aPoint);
            //if (i >= GetSelectedItemIndex())
            //    i--;
            if (i >= 0)
                InsertBefore(aPuzzleItem, i);
            else
                InsertAfter(aPuzzleItem, PuzzleItems.Count - 1);
            dragging_insert_slot = -2;
        }

        //public string outttt = "";
        //static PuzzleItem draggingPuzzleItem;
        public PuzzleItem getItemAt(Point aPoint)
        {
            //Point newPoint = aPoint;
            Point newPoint = new Point(aPoint.X - InitialOffsetPoint.X - 5, aPoint.Y - InitialOffsetPoint.Y - 5);//not sure if this should be -5... or *2
            int row = newPoint.Y / (ItemsImageSize + PaddingBetweenItems);
            int col = newPoint.X / (ItemsImageSize + PaddingBetweenItems);
            int itemNumber = gridWidth * (row) + col;
            if (itemNumber + firstItemToDraw >= _dispalyablePuzzleItems.Count || itemNumber < 0)
                return null;
            else
                return _dispalyablePuzzleItems[itemNumber + firstItemToDraw];
        }
        private int getItemNumberAt(Point aPoint)
        {
            Point newPoint = new Point(aPoint.X - InitialOffsetPoint.X - 5, aPoint.Y - InitialOffsetPoint.Y - 5);//not sure if this should be -5... or *2
            int row = newPoint.Y / (ItemsImageSize + PaddingBetweenItems);
            int col = newPoint.X / (ItemsImageSize + PaddingBetweenItems);
            int itemNumber = gridWidth * (row) + col;
            if (itemNumber + firstItemToDraw >= PuzzleItems.Count)
                return -1;
            else
                return itemNumber + firstItemToDraw;
        }
        private int dragging_insert_slot_old = -1;
        public void updateDraggingInsertElement(Point aPoint)
        {
            dragging_insert_slot_old = dragging_insert_slot;
            dragging_insert_slot = getItemSlotBetween(aPoint);
            if (dragging_insert_slot_old != dragging_insert_slot)
                Invalidate();
        }
        public void clearUpdateDraggingInsertElement()
        {
            dragging_insert_slot_old = dragging_insert_slot;
            dragging_insert_slot = -1;
            if (dragging_insert_slot_old != dragging_insert_slot)
                Invalidate();
        }
        private int getItemSlotBetween(Point aPoint)
        {
            Point newPoint = new Point(aPoint.X - InitialOffsetPoint.X - 5, aPoint.Y - InitialOffsetPoint.Y - 5);//not sure if this should be -5... or *2
            int modifier = 0;
            if (newPoint.X - (ItemsImageSize / 2) >= 0)
                newPoint.X -= ItemsImageSize / 2;
            else
            {
                newPoint.X += ItemsImageSize / 2;
                modifier--;
            }

            int row = newPoint.Y / (ItemsImageSize + PaddingBetweenItems);
            int col = newPoint.X / (ItemsImageSize + PaddingBetweenItems);
            int itemNumber = gridWidth * (row) + col;
            if (itemNumber + firstItemToDraw >= PuzzleItems.Count)
                return PuzzleItems.Count;
            else
                return itemNumber + modifier + firstItemToDraw + 1;//dont really need the  +firstItemToDraw since this will only be used in the palette
        }
        protected int firstItemToDraw = 0;
        public void refreshScrollBar()
        {
            firstItemToDraw = 0;
            autoSize();
        }

        protected override void OnPaint(PaintEventArgs e)
        {

            update(e.Graphics);
        }

        public override bool Equals(object obj)
        {
            try
            {
                if (obj is PanelItemHolder)
                {
                    PanelItemHolder aPanelItemHolder = (PanelItemHolder)obj;
                    return base.Equals(obj) && aPanelItemHolder.PuzzleItems.Equals(this.PuzzleItems);
                }
                else
                {
                    return false;
                }
            }
            catch
            { return false; }


        }

        //#region DoubleBuffer



        //private bool initializationComplete;
        //private bool isDisposing;
        //private BufferedGraphicsContext backbufferContext;
        //private BufferedGraphics backbufferGraphics;
        //private Graphics drawingGraphics;


        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    RecreateBuffers();
        //    Redraw();
        //}

        ///// <summary> 
        ///// Clean up any resources being used.
        ///// </summary>
        ///// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    isDisposing = true;
        //    if (disposing)
        //    {
        //        //if (components != null)
        //        //    components.Dispose();

        //        // We must dispose of backbufferGraphics before we dispose of backbufferContext or we will get an exception.
        //        if (backbufferGraphics != null)
        //            backbufferGraphics.Dispose();
        //        if (backbufferContext != null)
        //            backbufferContext.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private void RecreateBuffers()
        //{
        //    // Check initialization has completed so we know backbufferContext has been assigned.
        //    // Check that we aren't disposing or this could be invalid.
        //    if (!initializationComplete || isDisposing)
        //        return;

        //    // We recreate the buffer with a width and height of the control. The "+ 1" 
        //    // guarantees we never have a buffer with a width or height of 0. 
        //    backbufferContext.MaximumBuffer = new Size(this.Width + 1, this.Height + 1);

        //    // Dispose of old backbufferGraphics (if one has been created already)
        //    if (backbufferGraphics != null)
        //        backbufferGraphics.Dispose();

        //    // Create new backbufferGrpahics that matches the current size of buffer.
        //    backbufferGraphics = backbufferContext.Allocate(this.CreateGraphics(),
        //        new Rectangle(0, 0, Math.Max(this.Width, 1), Math.Max(this.Height, 1)));

        //    // Assign the Graphics object on backbufferGraphics to "drawingGraphics" for easy reference elsewhere.
        //    drawingGraphics = backbufferGraphics.Graphics;

        //    // This is a good place to assign drawingGraphics.SmoothingMode if you want a better anti-aliasing technique.

        //    // Invalidate the control so a repaint gets called somewhere down the line.
        //    this.Invalidate();
        //}

        //private void Redraw()
        //{
        //    // In this Redraw method, we simply make the control fade from black to white on a timer.
        //    // But, you can put whatever you want here and detach the timer. The trick is just making
        //    // sure redraw gets called when appropriate and only when appropriate. Examples would include
        //    // when you resize, when underlying data is changed, when any of the draqwing properties are changed
        //    // (like BackColor, Font, ForeColor, etc.)
        //    if (drawingGraphics == null)
        //        return;

        //    update();
        //    //if (color.R > 254)
        //    //    colorIncreasing = false;
        //    //if (color.R < 1)
        //    //    colorIncreasing = true;

        //    //color = colorIncreasing ? Color.FromArgb(color.R + 1, color.G + 1, color.B + 1) : Color.FromArgb(color.R - 1, color.G - 1, color.B - 1);
        //    //drawingGraphics.Clear(color);

        //    // Force the control to both invalidate and update. 
        //    this.Refresh();
        //}

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    // If we've initialized the backbuffer properly, render it on the control. 
        //    // Otherwise, do just the standard control paint.
        //    if (!isDisposing && backbufferGraphics != null)
        //        backbufferGraphics.Render(e.Graphics);
        //}

        //#endregion
        public void ToggleSelectedSubcatagory()
        {
            for (int i = 0; i < aListOfSelectedItems.Count; i++)
            {
                ((PuzzleItem)aListOfSelectedItems[i]).ToggleSubcatagory();

            }

        }




        public string[] BuildEditorItems(List<PuzzleItem> ListOfAllItems, string StyleName)
        {
            ArrayList editorItems = new ArrayList();
            string[] results;

            // All the items that aren't in the palette have to be done FIRST
            // unless we are going to remove the palette entry
            for (int i = 0; i < ListOfAllItems.Count; i++)
            {
                PuzzleItem aPuzzleItem = ListOfAllItems[i];


                if (aPuzzleItem != null)
                {
                    //check for duplicates with all previously printed items
                    bool itemHasBeenSeen = false;
                    for (int j = 0; j < i; j++)
                    {
                        if ((ListOfAllItems[j]).TypeName == aPuzzleItem.TypeName)
                        {
                            itemHasBeenSeen = true;
                            break;
                        }
                    }
                    //check to make sure it also wasn't on the palette
                    if (!itemHasBeenSeen)
                    {
                        for (int j = 0; j < PuzzleItems.Count; j++)
                        {
                            if ((PuzzleItems[j]).TypeName == aPuzzleItem.TypeName)
                            {
                                itemHasBeenSeen = true;
                                break;
                            }
                        }
                    }
                    //only do this if the item hasn't been used before
                    if (!itemHasBeenSeen)
                    {
                        results = aPuzzleItem.GetDefinition();
                        editorItems.Add(results);
                    }
                }
            }

            // Now do the items in the Palette
            for (int i = 0; i < PuzzleItems.Count; i++)
            {
                //for each item, add it
                PuzzleItem aPuzzleItem = PuzzleItems[i];


                if (aPuzzleItem != null)
                {
                    //check for duplicates with all previously printed items
                    bool itemHasBeenSeen = false;
                    for (int j = 0; j < i; j++)
                    {
                        if ((PuzzleItems[j]).TypeName == aPuzzleItem.TypeName)
                        {
                            itemHasBeenSeen = true;
                            break;
                        }
                    }
                    //only do this if the item hasn't been used before
                    if (!itemHasBeenSeen)
                    {
                        //get all later instances of this object and combine them into one item entry
                        ArrayList activeSubItems = new ArrayList();
                        ArrayList subItemLocations = new ArrayList();
                        for (int j = 0; j < PuzzleItems.Count; j++)//this should probably be int j = i  but I'm not going to try that just yet
                        {
                            if ((PuzzleItems[j]).TypeName == aPuzzleItem.TypeName)
                            {
                                activeSubItems.Add((PuzzleItems[j]).SubcatagoryIndex);
                                subItemLocations.Add(j);
                            }
                        }

                        results = aPuzzleItem.GetDefinition((int[])activeSubItems.ToArray(typeof(int)), (int[])subItemLocations.ToArray(typeof(int)));
                        editorItems.Add(results);
                    }
                }
            }

            //insert the vbsp style definition
            //find the style then grab its definition and insert into the editorItems at the beginning
            foreach (object style in Global.ListOfStyles)
            {
                if (((VBSPStyle)style).Name == StyleName)
                {
                    editorItems.Insert(0, ((VBSPStyle)style).WholeDefinition);
                    break;
                }
            }



            //collapse all entries into one string[]
            ArrayList returnValue = new ArrayList();
            string[] temp;
            int tabsDeep = 0;
            for (int i = 0; i < editorItems.Count; i++)
            {
                temp = (string[])editorItems[i];
                for (int j = 0; j < temp.Length; j++)
                {

                    if (temp[j].Trim() == "}")
                        tabsDeep--;

                    string tttemp = "";
                    for (int k = 0; k < tabsDeep; k++)
                    {
                        tttemp += "\t";
                    }
                    tttemp += temp[j];
                    returnValue.Add(tttemp);
                    if (temp[j].Trim() == "{")
                        tabsDeep++;
                }

            }
            returnValue.Add("}");

            //iterate through and find subdefinitions with palette positions not on the palette
            int indexOfPaletteOpening = -1;
            int indexOfPaletteEnding = -1;
            for (int i = 0; i < returnValue.Count; i++)
            {
                string line = (string)returnValue[i];
                if (line.Trim() == "\"Palette\"")
                    indexOfPaletteOpening = i;
                else if (line.Contains(Global.NotOnPaletteEntry))
                {
                    //found an entry to take out
                    indexOfPaletteEnding = -1;
                    for (int j = 0; indexOfPaletteEnding == -1; j++)
                    {
                        if (((string)returnValue[i + j]).Trim().StartsWith("}"))
                            indexOfPaletteEnding = i + j;
                    }
                    //now we have the beginning and ending points... either clear this data out or comment it out... I'll comment for now
                    for (int j = indexOfPaletteOpening; j <= indexOfPaletteEnding; j++)
                    {
                        returnValue[j] = ((string)returnValue[j]).Insert(0, "//");//inserting comments
                    }
                    i = indexOfPaletteEnding;//new place to continue searching (saves a little time)
                }
            }
            return (string[])returnValue.ToArray(typeof(string));
        }

        public void AddType(string t, int subitem, PanelItemHolder fromPanel)
        {
            PuzzleItem item;
            item = fromPanel.getItemByType(t, subitem);
            if (item != null)
                this.InsertBefore(item.Clone(), this.GetNumberOfItems());
        }

        public Point getOffsetOnPoint(Point aPoint)
        {
            Point newPoint = new Point(aPoint.X - InitialOffsetPoint.X - 5, aPoint.Y - InitialOffsetPoint.Y - 5);//not sure if this should be -5... or *2
            int row = newPoint.Y / (ItemsImageSize + PaddingBetweenItems);
            int col = newPoint.X / (ItemsImageSize + PaddingBetweenItems);
            int location = gridWidth * (row) + col;
            int x = (location % gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.X;
            int y = (location / gridWidth) * (ItemsImageSize + PaddingBetweenItems) + InitialOffsetPoint.Y;

            return new Point(aPoint.X - x, aPoint.Y - y);
        }

    }
}