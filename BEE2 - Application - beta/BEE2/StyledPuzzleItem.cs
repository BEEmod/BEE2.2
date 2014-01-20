using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEE2
{
    public class StyledPuzzleItem
    {
        public string TypeName { get; private set; }
        public string Author { get; private set; }
        public string Description { get; private set; }
        public string[] Filters { get; private set; }
        public Style Style { get; set; }

        public StyledPuzzleItem(string[] definition)
        {
            //TODO: implement this
        }
    }
}
