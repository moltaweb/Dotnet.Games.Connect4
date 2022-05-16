using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conecta4.Wpf.Models
{
    public class CellPositionModel
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public CellPositionModel(int row, int col)
        {
            Row = row;
            Column = col;
        }

    }


}
