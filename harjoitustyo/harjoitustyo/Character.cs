using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace harjoitustyo
{
    public class Character
    {

        public int characterWidth = 30;
        public string Name { get; set; }
        public int Hitpoints { get; set; }

        public Random rnd = new Random();

        public Ellipse character = new Ellipse();

        public Vector startingPoint = new Vector(200, 100);
        public Vector currentPosition = new Vector();
        public Vector charMove_norm;
        RotateTransform rotate = new RotateTransform();

        public void Move(Point targ)
        {
            Vector tarVec = new Vector(targ.X, targ.Y);
            Vector curVec = new Vector(currentPosition.X, currentPosition.Y);

            Vector charMove = tarVec - curVec;
            double charMove_length = Math.Sqrt(Math.Pow(charMove.X, 2) + Math.Pow(charMove.Y, 2));
            charMove_norm = charMove / charMove_length;
        }

        public double Angle(Point origin, Point target)
        {
            Vector vector = new Vector();
            vector.X = target.X - origin.X;
            vector.Y = target.Y - origin.Y;
            vector.Normalize();
            double dotAngle = -vector.Y;
            double angle = Math.Acos(dotAngle);
            angle = angle * 180 / Math.PI;
            if (vector.X > 0)
            {
                return angle;
            }
            else
            {
                return -angle;
            }
        }

        public void Rotation(Point targetPoint)
        {
            try
            {

                Point charSize = new Point(character.ActualWidth / 50, character.ActualHeight / 50);

                character.RenderTransformOrigin = charSize;
                character.RenderTransform = rotate;

                double y = Canvas.GetTop(character) + character.ActualWidth / 2.0;
                double x = Canvas.GetLeft(character) + character.ActualHeight / 2.0;
                Point originPoint = new Point(x, y);

                rotate.Angle = Angle(originPoint, targetPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Die()
        {

        }

        public void AlterHitPoints()
        {

        }
        
    }

    public class Enemy : Character
    {
        public int ScoreValue { get; set; }
        private int minScoreValue = 50;
        private int maxScoreValue = 250;
        public int Damage { get; set; }

        public Vector EnemyPosition = new Vector();

        public Ellipse monster = new Ellipse();
        

        //List<Object> Enemies = new List<object>();

        public void PaintMonster()
        {
            ImageBrush enemy = new ImageBrush();
            enemy.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\enemy.png", UriKind.Relative));
            monster.Fill = enemy;
            monster.Width = characterWidth;
            monster.Height = characterWidth;
            ScoreValue = rnd.Next(minScoreValue, maxScoreValue);
        }

        public void CreateEnemy()
        {
        //Enemy zombie = new Enemy();
        //zombie.PaintEnemy();
        //Enemies.Add(zombie);
        }

        public void PaintEnemy()
        {
            //Enemy zombie = new Enemy();
            //enemyPoint = new Point(rnd.Next(MainWindow.minimi, MainWindow.maxWidth),
                                    //rnd.Next(MainWindow.minimi, MainWindow.maxHeight));
            //Ellipse enemy = new Ellipse();
            ImageBrush enemyImg = new ImageBrush();
            enemyImg.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\enemy.png", UriKind.Relative));
            //monster.Fill = enemyImg;
           // monster.Width = 30;
           // monster.Height = 30;

            /* Canvas.SetTop(enemy, enemyPoint.Y);
            Canvas.SetLeft(enemy, enemyPoint.X);
            paintCanvas.Children.Insert(index, enemy);
            enemies.Insert(index, enemyPoint); */
        }

    }

    class Player : Character
    {
        List<Weapon> WeaponList { get; set; }
        public int Ammo { get; set; }
        public int Score { get; set; }

        public void PaintPlayer()
        {
            ImageBrush player = new ImageBrush();
            player.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\player.png", UriKind.Relative));
            character.Fill = player;
            character.Width = characterWidth;
            character.Height = characterWidth;
        }

        
        public void ExitWhenDead()
        {

        }
    }
}
