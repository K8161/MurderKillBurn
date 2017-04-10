using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace harjoitustyo
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    class Character
    {
        public Vector vector { get; set; }
        public Point point { get; set; }

        public int characterWidth = 30;
        public string Name { get; set; }
        public int Hitpoints { get; set; }
        public bool HasWeapon { get; set; }
        public bool UsesAmmo { get; set; }

        /*public void Move(object sender, KeyEventArgs e)
        {
            //muutetaan suuntaa näppäimistön painalluksen mukaan
            switch (e.Key)
            {
                case Key.P:
                    if (timer.IsEnabled)
                        timer.Stop();
                    else
                        timer.Start();
                    break;
                case Key.Escape:
                    if (timer.IsEnabled)
                        Engine.GameOver();
                    else
                        this.Close();
                    break;
                case Key.Left:
                        if (currentPosition.X > minimi)
                        currentPosition.X -= characterWidth / 8;
                        break;
                case Key.Up:
                        if (currentPosition.Y > minimi)
                        currentPosition.Y -= characterWidth / 8;
                        break;
                case Key.Right:
                        if (currentPosition.X < maxWidth)
                        currentPosition.X += characterWidth / 8;
                        break;
                case Key.Down:
                        if (currentPosition.Y < maxHeight)
                        currentPosition.Y += characterWidth / 8;
                        break;
            }
            // lastDirection = currentDirection;
        }*/

        public void Fight()
        {

        }

        public void Die()
        {

        }

        public void AlterHitPoints()
        {

        }
        
    }

    class Enemy : Character
    {
        public int ScoreValue { get; set; }
        public int Accuracy { get; set; }

        public Ellipse enemy = new Ellipse();

        public Ellipse monster = new Ellipse();
        Random rnd = new Random();

        //List<Object> Enemies = new List<object>();

        public void PaintMonster()
        {
            ImageBrush player = new ImageBrush();
            player.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\enemy.png", UriKind.Relative));
            monster.Fill = player;
            monster.Width = characterWidth;
            monster.Height = characterWidth;
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
            enemy.Fill = enemyImg;
            enemy.Width = 30;
            enemy.Height = 30;

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

        public Ellipse character = new Ellipse();

        public Vector startingPoint = new Vector(200, 100);
        public Vector currentPosition = new Vector();

        public void PaintPlayer()
        {
            ImageBrush player = new ImageBrush();
            player.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\player.png", UriKind.Relative));
            character.Fill = player;
            character.Width = characterWidth;
            character.Height = characterWidth;
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
        public void ExitWhenDead()
        {

        }
    }
}
