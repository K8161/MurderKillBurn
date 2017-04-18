using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class Character : INotifyPropertyChanged
    {

        public int characterWidth = 30;
        public string Name { get; set; }
        private int hitpoints;
        public int Hitpoints
        {
            get
            {
                return hitpoints;
            }
            set
            {
                hitpoints = value;
                // raise hitpoints property changed method
                RaisePropertyChanged();
            }
        }

        public Random rnd = new Random();

        public Ellipse character = new Ellipse();

        public Vector startingPoint = new Vector(200, 100);
        public Vector currentPosition = new Vector();
        public Vector charMove_norm;

        public RotateTransform rotate = new RotateTransform();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Move(Point targ)
        {
            try
            {
                Vector tarVec = new Vector(targ.X, targ.Y);
                Vector curVec = new Vector(currentPosition.X, currentPosition.Y);

                Vector charMove = tarVec - curVec;
                double charMove_length = Math.Sqrt(Math.Pow(charMove.X, 2) + Math.Pow(charMove.Y, 2));
                charMove_norm = charMove / charMove_length;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public double Angle(Point origin, Point target)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
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
    }

    public class Enemy : Character
    {
        public int ScoreValue { get; set; }
        private int minScoreValue = 50;
        private int maxScoreValue = 250;
        public int Damage { get; set; }
        public const int enemyCount = 20;

        public Vector EnemyMove_norm;
        public Vector EnemyPosition;
        

        public void PaintMonster()
        {
            try
            {
                ImageBrush enemy = new ImageBrush();
                enemy.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\enemy.png", UriKind.Relative));
                character.Fill = enemy;
                character.Width = characterWidth;
                character.Height = characterWidth;
                ScoreValue = rnd.Next(minScoreValue, maxScoreValue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void MonsterPositionLogic(Vector currentPosition)
        {
            try
            {
                Vector Curplay = currentPosition;
                Vector CurEnem = new Vector(EnemyPosition.X, EnemyPosition.Y);
                Vector CurPlay = new Vector(Curplay.X, Curplay.Y);

                Vector EnemyMove = CurEnem - CurPlay;
                double EnemyMove_length = Math.Sqrt(Math.Pow(EnemyMove.X, 2) + Math.Pow(EnemyMove.Y, 2));
                EnemyMove_norm = EnemyMove / EnemyMove_length;
                EnemyPosition = EnemyPosition - EnemyMove_norm * 0.7;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }

    class Player : Character
    {
        public int Ammo { get; set; }
        public int Score { get; set; }

        public void PaintPlayer()
        {
            try
            {
                ImageBrush player = new ImageBrush();
                player.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\player.png", UriKind.Relative));
                character.Fill = player;
                character.Width = characterWidth;
                character.Height = characterWidth;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
