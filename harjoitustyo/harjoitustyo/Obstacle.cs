using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace harjoitustyo
{
    class Obstacle
    {
        private const int minimi = 10;
        private const int maxHeight = 860;
        private const int maxWidth = 1560;
        private List<Point> rocks = new List<Point>();
        public int Height { get; set; }
        public int Width { get; set; }
        private Point coordinates;

        public Point Coordinates
        {
            get { return coordinates; }
            set
            {
                if (coordinates.X < maxWidth && coordinates.Y < maxHeight ||
                    coordinates.X > minimi && coordinates.Y > minimi)
                {
                    coordinates = value;
                }
            }
        }

        public Ellipse Rock { get; set; }
        public Random rnd { get; set; }


        public void PaintObstacle(int index)
        {
            try
            {
                Coordinates = new Point(rnd.Next(minimi, maxWidth),
                                       rnd.Next(minimi, maxHeight));
                Rock = new Ellipse();
                Rock.Fill = Brushes.Gray;
                Rock.Width = rnd.Next(10, 40);
                Rock.Height = rnd.Next(10, 40);
                Canvas.SetTop(Rock, Coordinates.Y);
                Canvas.SetLeft(Rock, Coordinates.X);
                //paintCanvas.Children.Insert(index, Coordinates);
                rocks.Insert(index, Coordinates);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
