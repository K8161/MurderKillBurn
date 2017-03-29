﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace harjoitustyo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /*public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }*/

public partial class MainWindow : Window
    {
        //variables and consts
        private const int minimi = 5;
        private const int maxHeight = 540;
        private const int maxWidth = 740;
        private const int characterWidth = 10;
        private int difficulty = 5; //timerin ajastin aika ms
        private List<Point> bonusPoints = new List<Point>(); //omenakokoelma
        private const int bonusCount = 20;
        private List<Point> snakeParts = new List<Point>();
        private Point startingPoint = new Point(200, 100);
        private Point currentPosition = new Point();
        //private Direction lastDirection = Direction.Right;
        //private Direction currentDirection = Direction.Right; //alussa lähtee aina oikealle
        private Random rnd = new Random(); //pisteiden arvontaa varten
        private DispatcherTimer timer;
        Rectangle snake = new Rectangle();
        RotateTransform rotate = new RotateTransform();

        public MainWindow()
        {
            InitializeComponent();

            //tarvittavat alustukset
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, difficulty);
            timer.Tick += new EventHandler(timer_Tick);

            //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            this.MouseMove += new MouseEventHandler(Rotate);

            //piirretään omenat ja käärme
            IniBonusPoints();
            PaintSnake(startingPoint);
            currentPosition = startingPoint;
            paintCanvas.Children.Add(snake);

            //start game
            timer.Start();
        }

        private void IniBonusPoints()
        {
            for (int n = 0; n < bonusCount; n++)
            {
                try
                {
                    PaintBonus(n);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private double Angle(Point origin, Point target)
        {
            Vector vector = target - origin;
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

        private void Rotate(object sender, MouseEventArgs e)
        {
            try
            {
                Point targetPoint = e.GetPosition(this);

                snake.RenderTransform = rotate;

                double y = Canvas.GetTop(snake) + snake.ActualWidth;
                double x = Canvas.GetLeft(snake) + snake.ActualHeight;
                Point originPoint = new Point(x, y);

                rotate.Angle = Angle(originPoint, targetPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PaintBonus(int index)
        {
            //arvotaan omenalle piste eli X ja Y -koordinaatti
            Point point = new Point(rnd.Next(minimi, maxWidth),
                                    rnd.Next(minimi, maxHeight));
            //omenan piirto
             Ellipse omena = new Ellipse();
             omena.Fill = Brushes.Gray;
             omena.Width = rnd.Next(10, 25);
             omena.Height = rnd.Next(10, 25);
             Canvas.SetTop(omena, point.Y);
             Canvas.SetLeft(omena, point.X);
             paintCanvas.Children.Insert(index, omena);
             bonusPoints.Insert(index, point); 
        }
        private void PaintSnake(Point currentpoint)
        {
            //Rectangle snake = new Rectangle();
            
            snake.Fill = Brushes.GhostWhite;
            snake.Width = characterWidth;
            snake.Height = characterWidth;
            Canvas.SetTop(snake, currentpoint.Y);
            Canvas.SetLeft(snake, currentpoint.X);
            int count = paintCanvas.Children.Count;
            //paintCanvas.Children.Add(snake);
            //snakeParts.Add(currentPosition);
            //rajoitetaan käärmeen pituutta
            //huom! bonusCount < snakeLength
            if (count > characterWidth)
            {
                paintCanvas.Children.RemoveAt(count - characterWidth);
                //snakeParts.RemoveAt(count - characterWidth);
            }
        }
        private void OnButtonKeyDown(object sender, KeyEventArgs e)
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
                        GameOver();
                    else
                        this.Close();
                    break;
                case Key.Left:
                    if ((currentPosition.X > 0))
                    currentPosition.X -= characterWidth / 4;
                    break;
                case Key.Up:
                    if ((currentPosition.Y > 0))
                    currentPosition.Y -= characterWidth / 4;
                    break;
                case Key.Right:
                    if ((currentPosition.X < maxWidth))
                    currentPosition.X += characterWidth / 4;
                    break;
                case Key.Down:
                    if ((currentPosition.Y < maxHeight))
                    currentPosition.Y += characterWidth / 4;
                    break;
            }
           // lastDirection = currentDirection;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            PaintSnake(currentPosition);
            
            int n = 0;
            try
            {
                foreach (Point point in bonusPoints)
                {
                    if ((Math.Abs(point.X - currentPosition.X) < characterWidth) &&
                        (Math.Abs(point.Y - currentPosition.Y) < characterWidth))
                    {
                        if (difficulty > 5)
                        {
                            difficulty--;
                            timer.Interval = new TimeSpan(0, 0, 0, 0, difficulty);
                        }
                        //bonusPoints.RemoveAt(n);
                        //paintCanvas.Children.RemoveAt(n);
                        PaintBonus(n);
                        break;
                    }
                    n++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GameOver()
        {
            timer.Stop();
            //MessageBox.Show("Your score: " + score);
            //this.Close();
            GameOverShow();
        }

        private void GameOverShow()
        {
         //   txtMessage.Text = "Your score: " + score + "\npress Esc to quit";
          //  paintCanvas.Children.Add(txtMessage);
            //animaatio joka siirtää kanvaasin
            var trs = new TranslateTransform();
            var anim = new DoubleAnimation(0, 620, TimeSpan.FromSeconds(15));
            trs.BeginAnimation(TranslateTransform.XProperty, anim);
            trs.BeginAnimation(TranslateTransform.YProperty, anim);
            paintCanvas.RenderTransform = trs;
        }
    }
}