using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoGanh1
{
    class CONSTANT
    {
        public static int BLACK = 1;
        public static int WHITE = 2;
        public static int EMPTY = 0;
         
        public static int SQUARE_SIZE = 125;
        public static int CHESS_SIZE = 525;
        public static int FIRST_COORD = 25;
        public static int[,] MAP =
        {
            { -1,-1,-1,-1,-1,-1,-1},
            {-1,1,1,1,1,1,-1 },
            { -1,1,0,0,0,1,-1},
            { -1,1,0,0,0,2,-1},
            { -1,2,0,0,0,2,-1},
            { -1,2,2,2,2,2,-1},
            {-1,-1,-1,-1,-1,-1,-1 }
        };
        public static PictureBox BLACK_CHESS = new PictureBox { Image = CoGanh1.Properties.Resources.QuanCoDen };
        public static PictureBox WHITE_CHESS = new PictureBox { Image = CoGanh1.Properties.Resources.QuanCoTrang };
    }
}
