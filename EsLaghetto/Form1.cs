using System;
using System.Drawing;
using System.Windows.Forms;

namespace EsLaghetto
{
    public partial class Form1 : Form
    {
        int[,] grid = new int[10, 40];
        int cellSize = 20;

        bool modalitaDisegno = true; 
        bool mousePremuto = false;

        Button btnCambia;
        Button btnPulisci;

        public Form1()
        {
            this.Text = "Laghetto Paint";
            this.Size = new Size(850, 300);
            this.DoubleBuffered = true;

            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;

            btnCambia = new Button();
            btnCambia.Text = "Passa a RIEMPI";
            btnCambia.Location = new Point(10, 210);
            btnCambia.Size = new Size(120, 40);
            btnCambia.Click += BtnCambia_Click;
            this.Controls.Add(btnCambia);
        }

        private void BtnCambia_Click(object sender, EventArgs e)
        {
            modalitaDisegno = !modalitaDisegno;

            if (modalitaDisegno)
                btnCambia.Text = "Passa a RIEMPI";
            else
                btnCambia.Text = "Passa a DISEGNO";
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mousePremuto = true;
            Disegna(e);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousePremuto)
            {
                Disegna(e);
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mousePremuto = false;
        }

    

        public void Disegna(MouseEventArgs e)
        {
            int col = e.X / cellSize;
            int row = e.Y / cellSize;

            if (row < 0 || row >= 10 || col < 0 || col >= 40)
                return;

            if (modalitaDisegno)
            {
                if (e.Button == MouseButtons.Right)
                {
                    grid[row, col] = 0;
                }
                else if (e.Button == MouseButtons.Left)
                {
                    grid[row, col] = 1;
                }
            }
            else
            {
                // riempimento tipo secchiello
                int colorePartenza = grid[row, col];
                if (colorePartenza != 2)
                {
                    FloodFill(row, col, colorePartenza);
                }
            }

            Invalidate();
        }

        void FloodFill(int r, int c, int target)
        {
            if (r < 0 || r >= 10 || c < 0 || c >= 40)
                return;

            if (grid[r, c] != target)
                return;

            grid[r, c] = 2; // acqua

            FloodFill(r + 1, c, target);
            FloodFill(r - 1, c, target);
            FloodFill(r, c + 1, target);
            FloodFill(r, c - 1, target);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 40; c++)
                {
                    int x = c * cellSize;
                    int y = r * cellSize;

                    if (grid[r, c] == 1)
                        g.FillRectangle(Brushes.SaddleBrown, x, y, cellSize, cellSize);

                    else if (grid[r, c] == 2)
                        g.FillRectangle(Brushes.Aqua, x, y, cellSize, cellSize);

                    g.DrawRectangle(Pens.Gray, x, y, cellSize, cellSize);
                }
            }
        }
    }
}