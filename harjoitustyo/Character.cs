using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
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
    public abstract class Character : INotifyPropertyChanged
    {

        public int characterWidth = 20;
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

        public ImageBrush characterFill = new ImageBrush();

        public Vector startingPoint = new Vector(200, 100);
        public Vector currentPosition = new Vector();
        public Vector charMove_norm;

        public RotateTransform rotate = new RotateTransform();

        public event PropertyChangedEventHandler PropertyChanged;

        public Character(ImageSource imgSource)
        {
            characterFill.ImageSource = imgSource;
        }

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

        public void Rotation(Point targetPoint, Shape shape)
        {
            try
            {
                Point charSize = new Point(shape.ActualWidth / 80, shape.ActualHeight / 80);

                shape.RenderTransformOrigin = charSize;
                shape.RenderTransform = rotate;

                double y = Canvas.GetTop(shape) + shape.ActualWidth / 2.0;
                double x = Canvas.GetLeft(shape) + shape.ActualHeight / 2.0;
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

        public Enemy(ImageSource imgSource) : base(imgSource)
        {
        }

        public void PaintMonster()
        {
            try
            {
                character.Fill = characterFill;
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

        public Rectangle tank = new Rectangle();

        public Player(ImageSource imgSource) : base(imgSource)
        {
        }

        public void PaintPlayer()
        {
            try
            {
                tank.Fill = characterFill;
                tank.Width = 40;
                tank.Height = 60;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
