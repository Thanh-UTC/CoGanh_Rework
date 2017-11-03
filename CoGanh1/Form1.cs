using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoGanh1
{
    public partial class Form1 : Form
    {
        private int[,] tempmap = CONSTANT.MAP;
        private Point destinationPoint;
        private Point startPoint;
        private PictureBox[,] listChess= new PictureBox[7,7];
        private PictureBox tempPicBox;
        private bool BlackTurn = false;

        struct Coordinates
        {
            int x;
            int y;
        }
        public Form1()
        {
            InitializeComponent();
            DrawChessMan(tempmap);
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Create pen.
            Pen blackPen = new Pen(Color.Black, 3);

            //create point
            Point[] point = new Point[2];

            // vertical lines
            for (int i = CONSTANT.FIRST_COORD; i <= CONSTANT.CHESS_SIZE; i += CONSTANT.SQUARE_SIZE)
            {
                point[0] = new Point(i, 25);
                point[1] = new Point(i, 525);
                e.Graphics.DrawLines(blackPen, point);
            }

            //horizontal lines
            for (int i = 25; i <= 525; i += 125)
            {
                point[0] = new Point(25, i);
                point[1] = new Point(525, i);
                e.Graphics.DrawLines(blackPen, point);
            }

            //italic lines
            e.Graphics.DrawLine(blackPen, 275, 25, 25, 275);
            e.Graphics.DrawLine(blackPen, 525, 25, 25, 525);
            e.Graphics.DrawLine(blackPen, 525, 275, 275, 525);
            e.Graphics.DrawLine(blackPen, 25, 25, 525, 525);
            e.Graphics.DrawLine(blackPen, 275, 25, 525, 275);
            e.Graphics.DrawLine(blackPen, 25, 275, 275, 525);
        }
        public void DrawChessMan(int [,] temp_map)
        {
            Point location;
            for (int i = 1; i <6; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    location = new Point(j * CONSTANT.SQUARE_SIZE-125, i * CONSTANT.SQUARE_SIZE-125);
                    if (temp_map[i, j] == 1)
                    {
                        listChess[i, j] = new PictureBox
                        {
                            Location = location,
                            Image = CoGanh1.Properties.Resources.QuanCoDen,
                            BackColor = Color.Transparent
                        };
                        panel1.Controls.Add(listChess[i, j]);
                        //listChess[i, j].Click += ChessClick; 
                    }
                    else if (temp_map[i, j] == 2)
                    {
                        listChess[i, j] = new PictureBox
                        {
                            Location = location,
                            BackColor = Color.Transparent,
                            Image = CoGanh1.Properties.Resources.QuanCoTrang
                        };
                        panel1.Controls.Add(listChess[i, j]);
                        listChess[i, j].Click += ChessClick;
                    }
                    else
                    {
                        listChess[i, j] = null;
                    }
                }
            }
        }

        public void WipeChessMan()
        {
            for (int i = 1; i <6; i++)
            {
                for (int j = 1; j < 6; j++)
                {
                    panel1.Controls.Remove(listChess[j, i]);
                }
            }
        }
        private void ChessClick(object sender, EventArgs e)
        {
            PictureBox chessPiece = sender as PictureBox;
            Console.WriteLine(chessPiece.Location.X + ";" + chessPiece.Location.Y);
            // find the coord in map- set coord to startPoint
            startPoint = new Point(chessPiece.Location.X, chessPiece.Location.Y);
            int i = (chessPiece.Location.X) / CONSTANT.SQUARE_SIZE+1;
            int j = (chessPiece.Location.Y) / CONSTANT.SQUARE_SIZE+1;
            Console.WriteLine(startPoint.X + ";" + startPoint.Y);
            tempPicBox = listChess[j, i];
            Console.WriteLine("current coord: " + tempPicBox.Location.X + ";" + tempPicBox.Location.Y);
            Console.WriteLine("start point coord: " + startPoint.X + ";" + startPoint.Y);
            //preset
            chessPiece.BackColor = Color.PeachPuff;
            chessPiece.Size = new Size(50, 50);
            
        }
        
        public List<Point> findMovableBlackChessMan(int[,] temp_map)
        {
            List<Point> point = new List<Point>();
            for( int i =0; i < 7; i++)
            {
                for ( int j = 0; j < 7; j++)
                {
                    if (temp_map[i, j] == 1)
                    {
                        point.Add(new Point(j, i));

                    }
                }
            }

            return point;
        }
        //TODO: viet phuong thuc di chuyen quan co mau den.

        public void MoveBlackChessMan()
        {
            List<Point> listBlackChess = findMovableBlackChessMan(tempmap);
            Random t = new Random();
            int numberOfChess = listBlackChess.Count();
            Point nextChess = listBlackChess[t.Next(1, numberOfChess)];

            int i = nextChess.X;
            int j = nextChess.Y;

            List<Point> listMove = findMove(nextChess, tempmap);
            
            Point nextMove = listMove[0];
            int x = nextMove.X/CONSTANT.SQUARE_SIZE;
            int y = nextMove.Y/CONSTANT.SQUARE_SIZE;

            panel1.Controls.Remove(listChess[j, i]);
            int m;

            m = tempmap[j, i];
            tempmap[j, i] = 0;
            tempmap[y + 1, x + 1] = m;

            listChess[y + 1, x + 1] = tempPicBox;

            tempmap = EatCheck(destinationPoint, tempmap);
            WipeChessMan();
            DrawChessMan(tempmap);


        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = (e.X) / CONSTANT.SQUARE_SIZE;
            int y = (e.Y) / CONSTANT.SQUARE_SIZE;
            Console.WriteLine(x + ";" + y);
             
            destinationPoint = new Point(x * 125, y * 125);
            if (LegalMove(startPoint, destinationPoint, tempmap))
            {
                int i = (startPoint.X) / CONSTANT.SQUARE_SIZE+1;
                int j = (startPoint.Y) / CONSTANT.SQUARE_SIZE+1;
                panel1.Controls.Remove(listChess[j, i]);
                int m;

                m = tempmap[j, i];
                tempmap[j, i] = 0;
                tempmap[y+1, x+1] = m;

                listChess[y+1, x+1] = tempPicBox;
                
                tempmap = EatCheck(destinationPoint, tempmap);
                
                //ResetChessmanImage(listChess[y, x]);
                //listChess[y, x].Location = destinationPoint;
                //this.panel1.Controls.Add(listChess[y, x]);
                WipeChessMan();
                DrawChessMan(tempmap);
                MoveBlackChessMan();
            }
            
            //BlackTurn = true;
            
        }
        public void ResetChessmanImage(PictureBox image)
        {
            image.BackColor = Color.Transparent;
            
        }
        public  bool LegalMove(Point startCoord, Point desCoord, int[,] maps)
        {
            //find the coordinates of start Point and destination Point
            int startX = (startCoord.X+125) / CONSTANT.SQUARE_SIZE;
            int startY = (startCoord.Y+125) / CONSTANT.SQUARE_SIZE;
            int desX = (desCoord.X+125) / CONSTANT.SQUARE_SIZE;
            int desY = (125+desCoord.Y) / CONSTANT.SQUARE_SIZE;
            //check the move
            if ((maps[desY, desX] != 0) || ((startX + startY) % 2 != 0) && ((desX + desY) % 2 != 0))
            {
                return false;
            }
            return true;
        }
        public int[,] EatCheck(Point Coord, int[,] tempmap)
        {
            int x = (Coord.X) / CONSTANT.SQUARE_SIZE+1;
            int y = (Coord.Y) / CONSTANT.SQUARE_SIZE+1;
            int[,] maps = tempmap;
            for (int a = 0; a < 7; a++)
            {
                for (int b = 0; b < 7; b++)
                {
                    Console.Write(tempmap[a, b] + " ");
                }
                Console.WriteLine();
            }
            if ((x == 3 && y == 4) || (x == 4 && y == 3) 
                || (x == 2 && y == 3) || (x == 3 && y == 2))
            {
                if ((maps[y, x] == 2) && (maps[y, x] != maps[y, x - 1]) 
                    && (maps[y, x] != maps[y, x + 1]) && (maps[y, x + 1] != 0) 
                    && (maps[y, x - 1] != 0) && (maps[y, x + 1] != -1) && (maps[y, x - 1] != -1))
                {
                    maps[y, x + 1] = 2; maps[y, x - 1] = 2;
                }
                else if ((maps[y, x] == 2) && (maps[y, x] != maps[y - 1, x]) 
                    && (maps[y, x] != maps[y + 1, x]) && (maps[y + 1, x] != 0) 
                    && (maps[y - 1, x] != 0) && (maps[y + 1, x] != -1) && (maps[y - 1, x] != -1))
                {
                    maps[y + 1, x] = 2; maps[y - 1, x] = 2;
                }
                else if ((maps[y, x] == 1) && (maps[y, x] != maps[y, x - 1]) 
                    && (maps[y, x] != maps[y, x + 1]) && (maps[y, x + 1] != 0) 
                    && (maps[y, x - 1] != 0) && (maps[y, x + 1] != -1) && (maps[y, x - 1] != -1))
                {
                    maps[y, x + 1] = 1; maps[y, x - 1] = 1;
                }
                else if ((maps[y, x] == 1) && (maps[y, x] != maps[y - 1, x]) 
                    && (maps[y, x] != maps[y + 1, x]) && (maps[y + 1, x] != 0) 
                    && (maps[y - 1, x] != 0) && (maps[y + 1, x] != -1) && (maps[y - 1, x] != -1))
                {
                    maps[y + 1, x] = 1; maps[y - 1, x] = 1;
                }
            }
            else
            {
                if ((maps[y, x] == 2) && (maps[y, x] != maps[y, x - 1]) 
                    && (maps[y, x] != maps[y, x + 1]) && (maps[y, x + 1] != 0) 
                    && (maps[y, x - 1] != 0) && (maps[y, x + 1] != -1) && (maps[y, x - 1] != -1))
                {
                    maps[y, x + 1] = 2; maps[y, x - 1] =2;
                }
                else if ((maps[y, x] == 2) && (maps[y, x] != maps[y - 1, x]) 
                    && (maps[y, x] != maps[y + 1, x]) && (maps[y + 1, x] != 0) 
                    && (maps[y - 1, x] != 0) && (maps[y + 1, x] != -1) && (maps[y - 1, x] != -1))
                {
                    maps[y + 1, x] =2; maps[y - 1, x] =2;
                }
                else if ((maps[y, x] == 2) && (maps[y, x] != maps[y - 1, x - 1]) 
                    && (maps[y, x] != maps[y + 1, x + 1]) && (maps[y + 1, x + 1] != 0) 
                    && (maps[y - 1, x - 1] != 0) && (maps[y + 1, x + 1] != -1) && (maps[y - 1, x - 1] != -1))
                {
                    maps[y + 1, x + 1] = 2; maps[y - 1, x - 1] = 2;
                }
                else if ((maps[y, x] == 2) && (maps[y, x] != maps[y + 1, x - 1]) 
                    && (maps[y, x] != maps[y - 1, x + 1]) && (maps[y - 1, x + 1] != 0) 
                    && (maps[y + 1, x - 1] != 0) && (maps[y - 1, x + 1] != -1) && (maps[y + 1, x - 1] != -1))
                {
                    maps[y + 1, x - 1] = 2; maps[y - 1, x + 1] = 2;
                }
                else if ((maps[y, x] == 1) && (maps[y, x] != maps[y, x - 1]) 
                    && (maps[y, x] != maps[y, x + 1]) && (maps[y, x + 1] != 0) 
                    && (maps[y, x - 1] != 0) && (maps[y, x + 1] != -1) && (maps[y, x - 1] != -1))
                {
                    maps[y, x + 1] = 1; maps[y, x - 1] = 1;
                }
                else if ((maps[y, x] == 1) && (maps[y, x] != maps[y - 1, x]) 
                    && (maps[y, x] != maps[y + 1, x]) && (maps[y + 1, x] != 0) 
                    && (maps[y - 1, x] != 0) && (maps[y + 1, x] != -1) && (maps[y - 1, x] != -1))
                {
                    maps[y + 1, x] = 1; maps[y - 1, x] = 1;
                }
                else if ((maps[y, x] == 1) && (maps[y, x] != maps[y - 1, x - 1]) 
                    && (maps[y, x] != maps[y + 1, x + 1]) && (maps[y + 1, x + 1] != 0) 
                    && (maps[y - 1, x - 1] != 0) && (maps[y + 1, x + 1] != -1) && (maps[y - 1, x - 1] != -1))
                {
                    maps[y + 1, x + 1] = 1; maps[y - 1, x - 1] = 1;
                }
                else if ((maps[y, x] == 1) && (maps[y, x] != maps[y + 1, x - 1]) 
                    && (maps[y, x] != maps[y - 1, x + 1]) && (maps[y - 1, x + 1] != 0) 
                    && (maps[y + 1, x - 1] != 0) && (maps[y - 1, x + 1] != -1) && (maps[y + 1, x - 1] != -1))
                {
                    maps[y + 1, x - 1] = 1; maps[y - 1, x + 1] = 1;
                }
            }
            //  (8)     *    (10)    (13)    (7)
            //  (15)    *     *       *       *
            //  (11)    *     *       *      (9)
            //   *     (5)   (4)      *      (12)
            //  (1)    (2)   (3)      *      (6)
            //center point
            //if (maps[y,x] == 2 && maps[y+2,x-2] ==2 && maps[y+2,x-1]==2 && maps[y+2,x]==2 && maps[y+1, x]==2 
            //    && maps[y+1, x-1]==1 && maps[y+1,x-2] == 2 && maps[y,x-2]==2 && maps[y,x-1]==2)
            //{
            //    maps[y+1, x-1] = 2;
            //}
            //else if (maps[y, x] == 2 && maps[y + 2, x + 2] == 2 && maps[y + 2, x + 1] == 2 && maps[y + 2, x] == 2 && maps[y + 1, x] == 2 
            //    && maps[y + 1, x + 1] == 1 && maps[y + 1, x + 2] == 2 && maps[y, x + 2] == 2 && maps[y, x + 1] == 2)
            //{
            //    maps[y + 1, x + 1] = 2;
            //}
            //else if(maps[y, x] == 2 && maps[y -2, x + 2] == 2 && maps[y - 2, x + 1] == 2 && maps[y - 2, x] == 2 && maps[y - 1, x] == 2
            //    && maps[y - 1, x + 1] == 1 && maps[y - 1, x + 2] == 2 && maps[y, x + 2] == 2 && maps[y, x + 1] == 2)
            //{
            //    maps[y - 1, x + 1] = 2;
            //}
            //else if(maps[y, x] == 2 && maps[y - 2, x - 2] == 2 && maps[y - 2, x - 1] == 2 && maps[y - 2, x] == 2 && maps[y - 1, x] == 2       
            //    && maps[y - 1, x - 1] == 1 && maps[y - 1, x - 2] == 2 && maps[y, x - 2] == 2 && maps[y, x - 1] == 2)
            //{
            //    maps[y - 1, x - 1] = 2;
            //}
            ////----------------------
            ////point 5
            //else if (maps[y, x] == 2 && maps[y + 2, x - 2] == 3 && maps[y + 2, x - 1] == 3 && maps[y + 2, x] == 3 && maps[y + 1, x] == 2
            //    && maps[y + 1, x - 1] == 1 && maps[y + 1, x - 2] == 2 && maps[y, x - 2] == 2 && maps[y, x - 1] == 2)
            //{
            //    maps[y + 1, x - 1] = 2;
            //}
            //else if(maps[y, x] == 2 && maps[y + 2, x + 2] == 3 && maps[y + 2, x + 1] == 2 && maps[y + 2, x] == 2 && maps[y + 1, x] == 2
            //    && maps[y + 1, x + 1] == 1 && maps[y + 1, x + 2] == 3 && maps[y, x + 2] == 3 && maps[y, x + 1] == 2)
            //{
            //    maps[y + 1, x + 1] = 2;
            //}
            //else if(maps[y, x] == 2 && maps[y - 2, x + 2] == 3 && maps[y - 2, x + 1] == 3 && maps[y - 2, x] == 3 && maps[y - 1, x] == 2
            //    && maps[y - 1, x + 1] == 1 && maps[y - 1, x + 2] == 2 && maps[y, x + 2] == 2 && maps[y, x + 1] == 2)
            //{
            //    maps[y - 1, x + 1] = 2;
            //}
            //else if (maps[y, x] == 2 && maps[y - 2, x - 2] == 3 && maps[y - 2, x - 1] == 2 && maps[y - 2, x] == 2 && maps[y - 1, x] == 2
            //    && maps[y - 1, x - 1] == 1 && maps[y - 1, x - 2] == 3 && maps[y, x - 2] == 3 && maps[y, x - 1] == 2)
            //{
            //    maps[y - 1, x - 1] = 2;
            //}
            ////---------------------
            ////point 5 with small square
            // else if (maps[y, x] == 2 && maps[y + 2, x - 2] == 3 && maps[y + 2, x - 1] == 3 && maps[y + 2, x] == 3 && maps[y + 1, x] == 2
            //    && maps[y + 1, x - 1] == 1 && maps[y + 1, x - 2] == 3 && maps[y, x - 2] == 3 && maps[y, x - 1] == 2)
            //{
            //    maps[y + 1, x - 1] = 2;
            //}else if (maps[y, x] == 2 && maps[y + 2, x + 2] == 3 && maps[y + 2, x + 1] == 3 && maps[y + 2, x] == 3 && maps[y + 1, x] == 2
            //    && maps[y + 1, x + 1] == 1 && maps[y + 1, x + 2] == 3 && maps[y, x + 2] == 3 && maps[y, x + 1] == 2)
            //{
            //    maps[y + 1, x + 1]=2;
            //}else if (maps[y, x] == 2 && maps[y - 2, x + 2] == 3 && maps[y - 2, x + 1] == 3 && maps[y - 2, x] == 3 && maps[y - 1, x] == 2
            //    && maps[y - 1, x + 1] == 1 && maps[y - 1, x + 2] == 3 && maps[y, x + 2] == 3 && maps[y, x + 1] == 2)
            //{
            //    maps[y - 1, x + 1] = 2;
            //}else if(maps[y, x] == 2 && maps[y - 2, x - 2] == 3 && maps[y - 2, x - 1] == 3 && maps[y - 2, x] == 3 && maps[y - 1, x] == 2
            //    && maps[y - 1, x - 1] == 1 && maps[y - 1, x - 2] == 3 && maps[y, x - 2] == 3 && maps[y, x - 1] == 2)
            //{
            //    maps[y - 1, x - 1] = 2;
            //}

            ////---------------------
            //// opposite player 
            //if (maps[y, x] == 1 && maps[y + 2, x - 2] == 1 && maps[y + 2, x - 1] == 1 && maps[y + 2, x] == 1 && maps[y + 1, x] == 1
            //    && maps[y + 1, x - 1] == 2 && maps[y + 1, x - 2] == 1 && maps[y, x - 2] == 1 && maps[y, x - 1] == 1)
            //{
            //    maps[y + 1, x - 1] = 1;
            //}
            //else if (maps[y, x] == 1 && maps[y + 2, x + 2] == 1 && maps[y + 2, x + 1] == 1 && maps[y + 2, x] == 1 && maps[y + 1, x] == 1
            //    && maps[y + 1, x + 1] == 2 && maps[y + 1, x + 2] == 1 && maps[y, x + 2] == 1 && maps[y, x + 1] == 1)
            //{
            //    maps[y + 1, x + 1] = 1;
            //}
            //else if (maps[y, x] == 1 && maps[y - 2, x + 2] == 1 && maps[y - 2, x + 1] == 1 && maps[y - 2, x] == 1 && maps[y - 1, x] == 1
            //    && maps[y - 1, x + 1] == 2 && maps[y - 1, x + 2] == 1 && maps[y, x + 2] == 1 && maps[y, x + 1] == 1)
            //{
            //    maps[y - 1, x + 1] = 1;
            //}
            //else if (maps[y, x] == 1 && maps[y - 2, x - 2] == 1 && maps[y - 2, x - 1] == 1 && maps[y - 2, x] == 1 && maps[y - 1, x] == 1
            //    && maps[y - 1, x - 1] == 2 && maps[y - 1, x - 2] == 1 && maps[y, x - 2] == 1 && maps[y, x - 1] == 1)
            //{
            //    maps[y - 1, x - 1] = 1;
            //}
            ////----------------------
            ////point 5
            //else if (maps[y, x] == 1 && maps[y + 2, x - 2] == 3 && maps[y + 2, x - 1] == 3 && maps[y + 2, x] == 3 && maps[y + 1, x] == 1
            //    && maps[y + 1, x - 1] == 2 && maps[y + 1, x - 2] == 1 && maps[y, x - 2] == 1 && maps[y, x - 1] == 1)
            //{
            //    maps[y + 1, x - 1] = 1;
            //}
            //else if (maps[y, x] == 1 && maps[y + 2, x + 2] == 3 && maps[y + 2, x + 1] == 1 && maps[y + 2, x] == 1 && maps[y + 1, x] == 1
            //    && maps[y + 1, x + 1] == 2 && maps[y + 1, x + 2] == 3 && maps[y, x + 2] == 3 && maps[y, x + 1] == 1)
            //{
            //    maps[y + 1, x + 1] = 1;
            //}
            //else if (maps[y, x] == 1 && maps[y - 2, x + 2] == 3 && maps[y - 2, x + 1] == 3 && maps[y - 2, x] == 3 && maps[y - 1, x] == 1
            //    && maps[y - 1, x + 1] == 2 && maps[y - 1, x + 2] == 1 && maps[y, x + 2] == 1 && maps[y, x + 1] == 1)
            //{
            //    maps[y - 1, x + 1] = 1;
            //}
            //else if (maps[y, x] == 1 && maps[y - 2, x - 2] == 3 && maps[y - 2, x - 1] == 1 && maps[y - 2, x] == 1 && maps[y - 1, x] == 1
            //    && maps[y - 1, x - 1] == 2 && maps[y - 1, x - 2] == 3 && maps[y, x - 2] == 3 && maps[y, x - 1] == 1)
            //{
            //    maps[y - 1, x - 1] = 1;
            //}
            ////---------------------
            ////point 5 with small square
            //else if (maps[y, x] == 1 && maps[y + 2, x - 2] == 3 && maps[y + 2, x - 1] == 3 && maps[y + 2, x] == 3 && maps[y + 1, x] == 1
            //   && maps[y + 1, x - 1] == 2 && maps[y + 1, x - 2] == 3 && maps[y, x - 2] == 3 && maps[y, x - 1] == 1)
            //{
            //    maps[y + 1, x - 1] = 1;
            //}
            //else if (maps[y, x] == 1 && maps[y + 2, x + 2] == 3 && maps[y + 2, x + 1] == 3 && maps[y + 2, x] == 3 && maps[y + 1, x] == 1
            //   && maps[y + 1, x + 1] == 2 && maps[y + 1, x + 2] == 3 && maps[y, x + 2] == 3 && maps[y, x + 1] == 1)
            //{
            //    maps[y + 1, x + 1] = 2;
            //}
            //else if (maps[y, x] == 1 && maps[y - 2, x + 2] == 3 && maps[y - 2, x + 1] == 3 && maps[y - 2, x] == 3 && maps[y - 1, x] == 1
            //   && maps[y - 1, x + 1] == 2 && maps[y - 1, x + 2] == 3 && maps[y, x + 2] == 3 && maps[y, x + 1] == 1)
            //{
            //    maps[y - 1, x + 1] = 1;
            //}
            //else if (maps[y, x] == 1 && maps[y - 2, x - 2] == 3 && maps[y - 2, x - 1] == 3 && maps[y - 2, x] == 3 && maps[y - 1, x] == 1
            //   && maps[y - 1, x - 1] == 2 && maps[y - 1, x - 2] == 3 && maps[y, x - 2] == 3 && maps[y, x - 1] == 1)
            //{
            //    maps[y - 1, x - 1] = 1;
            //}





            for (int a = 0; a < 7; a++)
            {
                for (int b = 0; b < 7; b++)
                {
                    Console.Write(tempmap[a, b] + " ");
                }
                Console.WriteLine();
            }
            return maps;

        }

        public bool CheckStatus(int[,] map)

        {
            int s = 0;
            for ( int i = 0; i <7; i++)
            {
                for (int j =0; j <7; j++)
                {
                    if (map[i, j] == 1)
                    {
                        s++;
                    }
                }
            }
            if(s!= 0)
            {
                return true;
            }
            return false;
        }
        
        public List<Point> findMove(Point startCoord, int[,] maps)
        {
            List<Point> PossibleMoves = new List<Point>();
            //List<Point> BestMoves = new List<Point>();
            //int i = (startCoord.X + 125) / CONSTANT.SQUARE_SIZE;
            //int j = (startCoord.Y + 125) / CONSTANT.SQUARE_SIZE;
            int i = startCoord.X;
            int j = startCoord.Y;

            //move to bottom-left point
            if ((i -1 >= 0) && (j - 1 >= 0)&&(LegalMove(startPoint, new Point((i-1)*CONSTANT.SQUARE_SIZE-125,(j-1)*CONSTANT.SQUARE_SIZE-125 ), maps)))
            {
                PossibleMoves.Add(new Point((i - 1) * CONSTANT.SQUARE_SIZE - 125, (j - 1) * CONSTANT.SQUARE_SIZE - 125));
            }
            //move to bottom-right point
            if((i + 1 <=6 ) && (j - 1 >= 0) && (LegalMove(startPoint, new Point((i - 1) * CONSTANT.SQUARE_SIZE - 125, (j + 1) * CONSTANT.SQUARE_SIZE - 125), maps)))
            {
                PossibleMoves.Add( new Point((i - 1) * CONSTANT.SQUARE_SIZE - 125, (j + 1) * CONSTANT.SQUARE_SIZE - 125));
            }
            //move to top-right point 
            if((i + 1 <= 6) && (j + 1 <= 6) && (LegalMove(startPoint, new Point((i + 1) * CONSTANT.SQUARE_SIZE - 125, (j + 1) * CONSTANT.SQUARE_SIZE - 125), maps)))
            {
                PossibleMoves.Add(new Point((i + 1) * CONSTANT.SQUARE_SIZE - 125, (j + 1) * CONSTANT.SQUARE_SIZE - 125));
            }
            //move to top-left point 
            if ((i - 1 >=0) && (j + 1 <= 6) && (LegalMove(startPoint, new Point((i - 1) * CONSTANT.SQUARE_SIZE - 125, (j + 1) * CONSTANT.SQUARE_SIZE - 125), maps)))
            {
                PossibleMoves.Add(new Point((i - 1) * CONSTANT.SQUARE_SIZE - 125, (j + 1) * CONSTANT.SQUARE_SIZE - 125));
            }

            //move to top point
            if ((j + 1 <= 6) && (LegalMove(startPoint, new Point(i * CONSTANT.SQUARE_SIZE - 125, (j + 1) * CONSTANT.SQUARE_SIZE - 125), maps)))
            {
                PossibleMoves.Add(new Point(i * CONSTANT.SQUARE_SIZE - 125, (j + 1) * CONSTANT.SQUARE_SIZE - 125));
            }
            //move to bottom point
            if ((j - 1 >=0) && (LegalMove(startPoint, new Point(i * CONSTANT.SQUARE_SIZE - 125, (j + 1) * CONSTANT.SQUARE_SIZE - 125), maps)))
            {
                PossibleMoves.Add(new Point(i * CONSTANT.SQUARE_SIZE - 125, (j - 1) * CONSTANT.SQUARE_SIZE - 125));
            }
            //move to right
            if ((i + 1 <= 6) && (LegalMove(startPoint, new Point((i+1) * CONSTANT.SQUARE_SIZE - 125, j * CONSTANT.SQUARE_SIZE - 125), maps)))
            { 
                PossibleMoves.Add(new Point((i+1) * CONSTANT.SQUARE_SIZE - 125, j  * CONSTANT.SQUARE_SIZE - 125));
            }
            //move to left
            if ((i - 1 >=0) && (LegalMove(startPoint, new Point((i-1) * CONSTANT.SQUARE_SIZE - 125, j * CONSTANT.SQUARE_SIZE - 125), maps)))
            {
                PossibleMoves.Add(new Point((i-1) * CONSTANT.SQUARE_SIZE - 125, j * CONSTANT.SQUARE_SIZE - 125));
            }

            //find the move that eat
            //foreach(var point in PossibleMoves)
            //{

            //}
            return PossibleMoves;
        }
    }
}
