using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BEE2
{
    public class StyledSubPuzzleItem
    {
        public string Name { get; private set; }
        public string Tooltip { get; private set; }
        public Image PreviewImage { get; private set; }

        private string[] _subDefinition;

        public StyledSubPuzzleItem(string[] subDefinition)
        {

        }

        public StyledSubPuzzleItem Clone()
        {
            return new StyledSubPuzzleItem(_subDefinition);
        }
    }
}
