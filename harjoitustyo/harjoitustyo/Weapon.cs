using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace harjoitustyo
{
    class Weapon
    {
        public string Name { get; set; }
        public const int bulletWidth = 15;
        public int Damage { get; set; }
        public int ClipSize { get; set; }
        public Ellipse bullet = new Ellipse();

        Vector targetVec;
        Vector bulletVec;

        public void Reload()
        {

        }

        public void PaintBullet()
        {
            try
            {
                ImageBrush cannonball = new ImageBrush();
                cannonball.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\cannonball.png", UriKind.Relative));
                bullet.Fill = cannonball;
                bullet.Width = bulletWidth;
                bullet.Height = bulletWidth;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
