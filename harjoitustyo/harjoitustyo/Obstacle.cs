using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace harjoitustyo
{
    class Obstacle
    {
        private const int minimi = 10;
        private const int maxHeight = 860;
        private const int maxWidth = 1560;
        private const int obstacleCount = 12;
        private List<Point> rocks = new List<Point>();
        public int Height { get; set; }
        public int Width { get; set; }
        private Random rnd = new Random();

        public void PaintRocks()
        {
            //arvotaan kivelle piste eli X ja Y -koordinaatti
            Point point = new Point(rnd.Next(minimi, maxWidth),
                                    rnd.Next(minimi, maxHeight));

            Ellipse rock = new Ellipse();
            rock.Width = rnd.Next(25, 100);
            rock.Height = rnd.Next(25, 100);
            ImageBrush rockImg = new ImageBrush();
            rockImg.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\stone.png", UriKind.Relative));
            rock.Fill = rockImg;
        /*    Canvas.SetTop(rock, point.Y);
            Canvas.SetLeft(rock, point.X);
            //paintCanvas.Children.Insert(index, rock);
            rocks.Insert(index, point); */
        }

    }
}
