using System;
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
        private const int maxHeight = 860;
        private const int maxWidth = 1560;
        private const int characterWidth = 20;
        private int difficulty = 5; //timerin ajastin aika ms
        private List<Point> rocks = new List<Point>(); //kivikokoelma
        private const int obstacleCount = 20;
        //private List<Point> snakeParts = new List<Point>();
        private Vector startingPoint = new Vector(200, 100);
        private Vector currentPosition = new Vector();
        //private Direction lastDirection = Direction.Right;
        //private Direction currentDirection = Direction.Right; //alussa lähtee aina oikealle
        private Random rnd = new Random(); //pisteiden arvontaa varten
        private DispatcherTimer timer;
        Ellipse snake = new Ellipse();
        RotateTransform rotate = new RotateTransform();
        Vector charMove_norm;
        Obstacle rock = new Obstacle();

        //Engine engine = new Engine();

        public MainWindow()
        {
            InitializeComponent();

            //tarvittavat alustukset
            timer = new DispatcherTimer();
            
            timer.Interval = new TimeSpan(0, 0, 0, 0, difficulty);
            timer.Tick += new EventHandler(timer_Tick);

            //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            /*this.KeyDown += new KeyEventHandler(UpKeyPressed);
            this.KeyDown += new KeyEventHandler(DownKeyPressed);
            this.KeyDown += new KeyEventHandler(LeftKeyPressed);
            this.KeyDown += new KeyEventHandler(RightKeyPressed);*/
            this.MouseMove += new MouseEventHandler(Rotate);
            this.MouseMove += new MouseEventHandler(charMove);

            //piirretään esteet ja pelaaja
            //IniRocks();
            PaintSnake(startingPoint);
            currentPosition = startingPoint;
            paintCanvas.Children.Add(snake);

            //start game
            timer.Start();
        }

        private void IniRocks()
        {
            for (int n = 0; n < obstacleCount; n++)
            {
                try
                {
                    rock.PaintObstacle(n);
                    paintCanvas.Children.Insert(rocks.Count, rock.Rock);
                    rocks.Insert(rocks.Count, rock.Coordinates);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void charMove(object sender, MouseEventArgs e)
        {

            Point targ = e.GetPosition(paintCanvas);
            Vector tarVec = new Vector(targ.X, targ.Y);
            Vector curVec = new Vector(currentPosition.X, currentPosition.Y);


            Vector charMove = tarVec - curVec;
            double charMove_length = Math.Sqrt(Math.Pow(charMove.X, 2) + Math.Pow(charMove.Y, 2));
            charMove_norm = charMove / charMove_length;
        }

        private double Angle(Point origin, Point target)
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

        private void Rotate(object sender, MouseEventArgs e)
        {
            try
            {
                Point targetPoint = e.GetPosition(this);

                Point charSize = new Point(snake.ActualWidth / 50, snake.ActualHeight / 50);

                snake.RenderTransformOrigin = charSize;
                snake.RenderTransform = rotate;

                double y = Canvas.GetTop(snake) + snake.ActualWidth / 2.0;
                double x = Canvas.GetLeft(snake) + snake.ActualHeight / 2.0;
                Point originPoint = new Point(x, y);

                rotate.Angle = Angle(originPoint, targetPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /*private void PaintRocks(int index)
        {
            //arvotaan kivelle piste eli X ja Y -koordinaatti
            Point point = new Point(rnd.Next(minimi, maxWidth),
                                    rnd.Next(minimi, maxHeight));
            //kiven piirto
             Ellipse rock = new Ellipse();
             rock.Fill = Brushes.Gray;
             rock.Width = rnd.Next(10, 40);
             rock.Height = rnd.Next(10, 40);
             Canvas.SetTop(rock, point.Y);
             Canvas.SetLeft(rock, point.X);
             paintCanvas.Children.Insert(index, rock);
             rocks.Insert(index, Coordinates); 
        }*/
        private void PaintSnake(Vector currentpoint)
        {
            ImageBrush player = new ImageBrush();
            player.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\player.png", UriKind.Relative));
            snake.Fill = player;
            snake.Width = characterWidth;
            snake.Height = characterWidth;
            Canvas.SetTop(snake, currentpoint.Y);
            Canvas.SetLeft(snake, currentpoint.X);
            //int count = paintCanvas.Children.Count;
            //paintCanvas.Children.Add(snake);
            //snakeParts.Add(currentPosition);
            //rajoitetaan käärmeen pituutta
            //huom! bonusCount < snakeLength
            /*if (count > characterWidth)
            {
                paintCanvas.Children.RemoveAt(count - characterWidth);
                snakeParts.RemoveAt(count - characterWidth);
            }*/
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
                    /*case Key.Left:
                        if (currentPosition.X > minimi)
                        currentPosition.X -= characterWidth / 8;
                        break;*/
                    case Key.Up:
                        if (currentPosition.Y > minimi)
                        currentPosition = currentPosition + charMove_norm * difficulty;
                        break;
                    /*case Key.Right:
                        if (currentPosition.X < maxWidth)
                        currentPosition.X += characterWidth / 8;
                        break;*/
                    case Key.Down:
                        if (currentPosition.Y < maxHeight)
                        currentPosition = currentPosition - charMove_norm * difficulty;
                    break;
            }
            // lastDirection = currentDirection;
        }

        private void UpKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    if (currentPosition.Y > minimi)
                        currentPosition.Y -= characterWidth / 8;
                    break;
            }
            
        }

        private void DownKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    if (currentPosition.Y < maxHeight)
                        currentPosition.Y += characterWidth / 8;
                    break;
            }

        }

        private void LeftKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (currentPosition.X > minimi)
                        currentPosition.X -= characterWidth / 8;
                    break;
            }

        }

        private void RightKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                    if (currentPosition.X < maxWidth)
                        currentPosition.X += characterWidth / 8;
                    break;
            }

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            PaintSnake(currentPosition);
            
            try
            {
                foreach (Point point in rocks)
                {
                    if ((Math.Abs(point.X - currentPosition.X) > characterWidth) &&
                       (Math.Abs(point.Y - currentPosition.Y) < characterWidth))
                    {

                    }
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