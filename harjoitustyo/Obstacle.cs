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
        public ImageBrush rockImg = new ImageBrush();
        private Random rnd = new Random();

        public Obstacle(ImageSource imgSource)
        {
            rockImg.ImageSource = imgSource;
        }

        public void PaintRock(Point point)
        {
            try
            {
                rock = new Ellipse();
                rock.Width = rnd.Next(25, 100);
                rock.Height = rnd.Next(25, 100);
                rock.Fill = rockImg;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
