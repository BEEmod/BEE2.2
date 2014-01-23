using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEE2
{
    public class StyledPuzzleItem
    {
        #region Properties
        public string TypeName { get; private set; }
        public string Author { get; private set; }
        public string Description { get; private set; }
        public string[] Filters { get; private set; }
        public Style Style { get; set; }
        public List<StyledSubPuzzleItem> StyledSubPuzzleItems { get; private set; }
        public int SubcatagoryIndexMax { get { return StyledSubPuzzleItems.Count - 1; } }
        public int SubcatagoryIndex
        {
            get { return _subcatagoryIndex; }
            set
            {
                _subcatagoryIndex = value > SubcatagoryIndexMax ? SubcatagoryIndexMax :
                    _subcatagoryIndex < 0 ? 0 : value;
            }
        }
        #endregion

        #region Members
        private string[] _definition;
        private int _subcatagoryIndex = 0;
        #endregion

        public StyledPuzzleItem(string[] definition)
        {
            _definition = definition;

            TypeName = ResourceFile.FindValue(_definition, "Type");
            Author = ResourceFile.FindValue(_definition, "Author");
            Description = ResourceFile.FindValue(_definition, "Description");

            //Convert visual "\n"s in the text into actual newlines
            if (Description != null)
                Description = Description.Replace("\\n", Environment.NewLine);
            string[][] subItemDefinitions = ResourceFile.FindSubValues(_definition, "SubType");

            //Get all subitems
            StyledSubPuzzleItems = subItemDefinitions == null ? null : subItemDefinitions
                .Where(item => ResourceFile.FindValue(item, "Image") != null)
                .Select(item => new StyledSubPuzzleItem(item)).ToList();

            //Get filters
            char[] parms = new char[] { ';' };
            string singleLineFilter = ResourceFile.FindValue(_definition, "Filter");
            singleLineFilter = singleLineFilter ?? "Miscellaneous";
            singleLineFilter += ";All Items";
            string[] split = singleLineFilter.Split(parms);
            Filters = (from s in split where s.Trim() != "" select s.Trim()).ToArray();


            //TODO: implement this
        }

        public bool IsInFilter(string filter)
        {
            return Filters.Contains(filter);
        }

        /// <summary> Gets the Puzzle Item definition assuming that none of the subitems are on the palette </summary>
        /// <returns></returns>
        public string[] GetDefinition()
        {
            return GetDefinition(new int[0], new int[0]);
        }

        /// <summary> Gets the Puzzle Item definition based on which subitems will be on the palette and where </summary>
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


            string[] body = new string[_definition.Length + 1];
            body[0] = "\"Item\"";
            for (int i = 1; i < body.Length; i++)
            {
                body[i] = _definition[i - 1];
            }
            string[] values = new string[SubcatagoryIndexMax + 1];
            int subItemLocations_index = 0;
            for (int i = 0; i < values.Length; i++)
            {
                int gridWidth = 4;//palette width
                if (activeSubItems.Contains(i))
                    values[i] = (subItemLocations[subItemLocations_index] % gridWidth).ToString() + " " + (subItemLocations[subItemLocations_index++] / gridWidth).ToString() + " 0";
                else
                    values[i] = Global.NotOnPaletteEntry;
            }
            body = ResourceFile.ChangeSubValues(body, "Position", values);

            //replace if nessesary, the image
            //if there is only one subitem in use, and its possible to have more than one
            if (activeSubItems.Length == 1 && SubcatagoryIndexMax > 1)
            {
                //and this item has an alternate image
                string x = Global.PreferredImages[this.TypeName];
                if (Global.PreferredImages.ContainsKey("this.TypeName"))
                    body = ResourceFile.ChangeValue(body, "Image", Global.PreferredImages[this.TypeName], activeSubItems[0]);
            }
            return body;
        }

        //private bool IntArrayContains(int[] body, int element)
        //{ foreach (int peice in body) { if (peice == element) return true; } return false; }
    }
}
