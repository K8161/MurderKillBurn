using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
public partial class MainWindow : Window
    {
        //variables and consts
        private const int minimi = 5;
        private const int maxHeight = 380;
        private const int maxWidth = 620;
        private const int characterWidth = 10;
        private int difficulty = 0; //timerin ajastin aika ms
        private List<Point> bonusPoints = new List<Point>(); //omenakokoelma
        private const int bonusCount = 20;
        private List<Point> snakeParts = new List<Point>();
        private Point startingPoint = new Point(100, 100);
        private Point currentPosition = new Point();
        //private Direction lastDirection = Direction.Right;
        private Direction currentDirection = Direction.Right; //alussa lähtee aina oikealle
        private Random rnd = new Random(); //pisteiden arvontaa varten
        private DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            //tarvittavat alustukset
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, difficulty);
            timer.Tick += new EventHandler(timer_Tick);
            //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            //piirretään omenat ja käärme
            IniBonusPoints();
            PaintSnake(startingPoint);
            currentPosition = startingPoint;

            //start game
            timer.Start();
        }

        private void IniBonusPoints()
        {
            for (int n = 0; n < bonusCount; n++)
            {
                PaintBonus(n);
            }
        }
        private void PaintBonus(int index)
        {
            //arvotaan omenalle piste eli X ja Y -koordinaatti
            Point point = new Point(rnd.Next(minimi, maxWidth),
                                    rnd.Next(minimi, maxHeight));
            //omenan piirto
             Ellipse omena = new Ellipse();
             omena.Fill = Brushes.HotPink;
             omena.Width = characterWidth;
             omena.Height = characterWidth;
             Canvas.SetTop(omena, point.Y);
             Canvas.SetLeft(omena, point.X);
             paintCanvas.Children.Insert(index, omena);
             bonusPoints.Insert(index, point); 
        }
        private void PaintSnake(Point currentpoint)
        {
            Rectangle snake = new Rectangle();
            snake.Fill = Brushes.Aquamarine;
            snake.Width = characterWidth;
            snake.Height = characterWidth;
            Canvas.SetTop(snake, currentpoint.Y);
            Canvas.SetLeft(snake, currentpoint.X);
            int count = paintCanvas.Children.Count;
            paintCanvas.Children.Add(snake);
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
                    currentPosition.X -= characterWidth / 4;
                    break;
                case Key.Up:
                    currentPosition.Y -= characterWidth / 4;
                    break;
                case Key.Right:
                    currentPosition.X += characterWidth / 4;
                    break;
                case Key.Down:
                    currentPosition.Y += characterWidth / 4;
                    break;
                case Key.Down & Key.Left: 
                    currentPosition.Y += characterWidth / 4;
                    currentPosition.X -= characterWidth / 4;
                    break;
                //case Key.Down & Key.Right:
            }
           // lastDirection = currentDirection;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            PaintSnake(currentPosition);
            //törmäystarkastelut 1-3
            //TT#1 tarkistetaan onko kanvaasilla
            if ((currentPosition.X > maxWidth) || (currentPosition.X < minimi) ||
                (currentPosition.Y > maxHeight) || (currentPosition.Y < minimi))
                GameOver();
            //TT#3
            //tarkistetaan osuuko omenaan
            int n = 0;
            foreach (Point point in bonusPoints)
            {
                if ((Math.Abs(point.X - currentPosition.X) < characterWidth) &&
                    (Math.Abs(point.Y - currentPosition.Y) < characterWidth))
                {
                    //syödään omena
                  //  score += 10;
                  //  snakeLength += 1;
                    //nopeutetaan peliä
                    if (difficulty > 5)
                    {
                        difficulty--;
                        timer.Interval = new TimeSpan(0, 0, 0, 0, difficulty);
                    }
                //    this.Title = "SnakeWPF your score: " + score;
                    bonusPoints.RemoveAt(n);
                    paintCanvas.Children.RemoveAt(n);
                    PaintBonus(n);
                    break;
                }
                n++;
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