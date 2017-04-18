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
        public Ellipse rock;
        public ImageBrush rockImg;
        private Random rnd = new Random();

        public void PaintRock(Point point)
        {
            rock = new Ellipse();
            rock.Width = rnd.Next(25, 100);
            rock.Height = rnd.Next(25, 100);
            rockImg = new ImageBrush();
            rockImg.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\stone.png", UriKind.Relative));
            rock.Fill = rockImg;
        }
    }
}
