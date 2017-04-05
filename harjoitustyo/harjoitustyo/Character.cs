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

        public int characterWidth { get; set; }
        private DispatcherTimer timer;
        public string Name { get; set; }
        public string Score { get; set; }
        public int Hitpoints { get; set; }
        public bool HasWeapon { get; set; }
        public bool UsesAmmo { get; set; }

        private void PaintSnake(Point currentpoint)
        {
            Ellipse character = new Ellipse();
            ImageBrush player = new ImageBrush();
            player.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\player.png", UriKind.Relative));
            character.Fill = player;
            character.Width = characterWidth;
            character.Height = characterWidth;
            //Canvas.SetTop(snake, currentpoint.Y);
            //Canvas.SetLeft(snake, currentpoint.X);
        }

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

        public void AttackPlayer()
        {

        }
    }

    class Player : Character
    {
        List<Weapon> WeaponList { get; set; }
        public int Ammo { get; set; }
        public int Score { get; set; }

     /*   public Player (string name)
          {
              Name = name;
          } */

        public Player player = new Player();

        public void GainScore()
        {

        }

        public void ExitWhenDead()
        {

        }
    }
}
