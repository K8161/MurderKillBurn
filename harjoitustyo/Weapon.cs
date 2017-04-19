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
        public const int bulletWidth = 10;
        public int bulletcount = 0;
        public int Damage { get; set; }
        public int ClipSize { get; set; }
        public Ellipse bullet = new Ellipse();

        public Vector bulletPosition = new Vector();

        public Vector targetVec = new Vector();
        public Vector bulletVec = new Vector();
        public Vector bulletMove_norm;
        public MediaPlayer fireSound = new MediaPlayer();
        public MediaPlayer fireSound2 = new MediaPlayer();
        public bool sound = true;

        public void Fire(Point target, Vector currentPosition)
        {
            try
            {
                targetVec = new Vector(target.X, target.Y);
                bulletVec = new Vector(currentPosition.X, currentPosition.Y); ;

                Vector bulletMove = targetVec - bulletVec;
                double bulletMove_length = Math.Sqrt(Math.Pow(bulletMove.X, 2) + Math.Pow(bulletMove.Y, 2)) / 4;
                bulletMove_norm = bulletMove / bulletMove_length;
                if (sound == true)
                {
                    fireSound.Open(new Uri(@"..\..\Resources\fire.wav", UriKind.Relative));
                    fireSound.Position = TimeSpan.Zero;
                    fireSound.Play();
                    sound = false;
                }
                else if (sound == false)
                {
                    fireSound2.Open(new Uri(@"..\..\Resources\fire.wav", UriKind.Relative));
                    fireSound2.Position = TimeSpan.Zero;
                    fireSound2.Play();
                    sound = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void BulletVisual()
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

        public void DiscardBullet()
        {
            Vector nullVector = new Vector(1900, 1200);
            bulletPosition = nullVector;
        }
    }
}
