using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conecta4.Wpf.Models
{
    public class CellModel
    {
        public CellType CellType { get; set; }
    }

    public enum CellType
    {
        Empty,
        PlayerChip,
        ComputerChip,
        Winner
    }
}
