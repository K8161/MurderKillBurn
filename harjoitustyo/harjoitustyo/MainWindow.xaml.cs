using System;
using System.Collections.Generic;
using System.IO;
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

public partial class MainWindow : Window
    {
        //variables and consts
        private const int minimi = 5;
        private const int maxHeight = 860;
        private const int maxWidth = 1560;
        private const int characterWidth = 30;
        private const int bulletWidth = 15;
        private int difficulty = 5; //timerin ajastin aika ms
        private List<Point> rocks = new List<Point>(); //kivikokoelma
        private List<Point> enemies = new List<Point>(); //viholliskokoelma
        private List<Vector> bullets = new List<Vector>();
        private const int obstacleCount = 15;
        private const int enemyCount = 12;
        //private List<Point> snakeParts = new List<Point>();
        private Vector startingPoint = new Vector(200, 100);
        private Vector currentPosition = new Vector();
        private Vector bulletPosition = new Vector();
        //private Direction lastDirection = Direction.Right;
        //private Direction currentDirection = Direction.Right; //alussa lähtee aina oikealle
        private Random rnd = new Random(); //pisteiden arvontaa varten
        private DispatcherTimer timer;
        private DispatcherTimer bulletTimer;
        Ellipse bullet = new Ellipse();
        Ellipse snake = new Ellipse();
        Ellipse enemy1 = new Ellipse();
        Ellipse enemy = new Ellipse();
        RotateTransform rotate = new RotateTransform();
        RotateTransform rotateAngle = new RotateTransform();
        Vector charMove_norm;
        Vector bulletMove_norm;
        private Vector enemySpawn = new Vector(600, 600);
        Vector enemyPoint1;
        Vector EnemyMove_norm;
        int MagazineSize = 10;
        Ellipse rock;

        Character playerone = new Character();

        public MainWindow()
        {
            InitializeComponent();

            btnOK.Visibility = Visibility.Hidden;
            txtName.Visibility = Visibility.Hidden;
            txbMag.Visibility = Visibility.Visible;
            txbScore.Visibility = Visibility.Visible;

            //tarvittavat alustukset
            timer = new DispatcherTimer();
            
            timer.Interval = new TimeSpan(0, 0, 0, 0, difficulty);
            timer.Tick += new EventHandler(timer_Tick);

            bulletTimer = new DispatcherTimer();

            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, difficulty);
            bulletTimer.Tick += new EventHandler(bulletTimer_Tick);

            //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            this.MouseMove += new MouseEventHandler(Rotate);
            this.MouseMove += new MouseEventHandler(charMove);
            this.MouseDown += new MouseButtonEventHandler(Shoot);

            //piirretään esteet ja pelaaja
            IniRocks();
            IniEnemies();
            PaintSnake(startingPoint);
            currentPosition = startingPoint;
            paintCanvas.Children.Add(snake);
            paintCanvas.Children.Add(enemy1);

            //start game
            timer.Start();
        }

        private void IniRocks()
        {
            for (int n = 0; n < obstacleCount; n++)
            {
                try
                {
                    PaintRocks(n);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void IniEnemies()
        {
            for (int n = 0; n < enemyCount; n++)
            {
                PaintEnemy(n);
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

        private void Shoot(object sender, MouseButtonEventArgs e)
        {
            try
            {
                switch (e.LeftButton)
                {
                    case MouseButtonState.Pressed:
                        
                          bulletPosition = currentPosition;
                          Point target = e.GetPosition(paintCanvas);
                          Vector targetVec = new Vector(target.X, target.Y);
                          Vector bulletVec = new Vector(currentPosition.X, currentPosition.Y); ;

                        if (MagazineSize > 0)
                        {
                            paintCanvas.Children.Add(bullet);
                            bulletTimer.Start();
                            MagazineSize--;
                            txbMag.Text = "Ammo left: " + Convert.ToString(MagazineSize);
                        }

                        else
                        {
                            txbMag.Text = " Ammo left: Reloading...";
                            MagazineSize = 10;
                        }
                          //paintCanvas.Children.Add(bullet);

                          Vector bulletMove = targetVec - bulletVec;
                          double bulletMove_length = Math.Sqrt(Math.Pow(bulletMove.X, 2) + Math.Pow(bulletMove.Y, 2)) / 3;
                          bulletMove_norm = bulletMove / bulletMove_length;
                          
                          //bulletTimer.Start();
                            break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void PaintRocks(int index)
        {
            //arvotaan kivelle piste eli X ja Y -koordinaatti
            Point point = new Point(rnd.Next(minimi, maxWidth),
                                    rnd.Next(minimi, maxHeight));

            rock = new Ellipse();
            rock.Width = rnd.Next(25, 100);
            rock.Height = rnd.Next(25, 100);
            ImageBrush rockImg = new ImageBrush();
            rockImg.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\stone.png", UriKind.Relative));
            rock.Fill = rockImg;
            Canvas.SetTop(rock, point.Y);
            Canvas.SetLeft(rock, point.X);
            paintCanvas.Children.Insert(index, rock);
            rocks.Insert(index, point);
        }

        private void PaintSnake(Vector currentpoint)
        {
            ImageBrush player = new ImageBrush();
            player.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\player.png", UriKind.Relative));
            snake.Fill = player;
            snake.Width = characterWidth;
            snake.Height = characterWidth;
            Canvas.SetTop(snake, currentpoint.Y);
            Canvas.SetLeft(snake, currentpoint.X);
        }

        private void PaintBullet(Vector bulletPoint)
        {
            try
            {
                ImageBrush cannonball = new ImageBrush();
                cannonball.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\cannonball.png", UriKind.Relative));
                bullet.Fill = cannonball;
                bullet.Width = bulletWidth;
                bullet.Height = bulletWidth;
                Canvas.SetTop(bullet, bulletPoint.Y);
                Canvas.SetLeft(bullet, bulletPoint.X);
                //paintCanvas.Children.Insert(index, bullet);
                //bullets.Insert(index, bulletPosition);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void PaintEnemy(int index)
        {
            Point enemyPoint = new Point(rnd.Next(minimi, maxWidth),
                                    rnd.Next(minimi, maxHeight));
            
            Ellipse enemy = new Ellipse();
            ImageBrush enemyImg = new ImageBrush();
            enemyImg.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\enemy.png", UriKind.Relative));
            enemy.Fill = enemyImg;
            enemy.Width = 30;
            enemy.Height = 30;
            
            Canvas.SetTop(enemy, enemyPoint.Y);
            Canvas.SetLeft(enemy, enemyPoint.X);
            paintCanvas.Children.Insert(index, enemy);
            enemies.Insert(index, enemyPoint);
        }

        private void PaintEnemy1(Vector enemyPoint1)
        {
            //PaintEnemy(0);
            //Ellipse enemy = new Ellipse();
            
            ImageBrush enemyImg = new ImageBrush();
            enemyImg.ImageSource = new BitmapImage(new Uri(@"..\..\Resources\enemy.png", UriKind.Relative));
            enemy1.Fill = enemyImg;
            enemy1.Width = 30;
            enemy1.Height = 30;
            Canvas.SetTop(enemy1, enemyPoint1.Y);
            Canvas.SetLeft(enemy1, enemyPoint1.X);
            //enemies.Insert(index, enemySpawnPoint);
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

        private void timer_Tick(object sender, EventArgs e)
        {
            PaintSnake(currentPosition);

            Vector Curplay = currentPosition;
            Vector CurEnem = new Vector(enemyPoint1.X, enemyPoint1.Y);
            Vector CurPlay = new Vector(Curplay.X, Curplay.Y);

            Vector EnemyMove = CurEnem - CurPlay;
            double EnemyMove_length = Math.Sqrt(Math.Pow(EnemyMove.X, 2) + Math.Pow(EnemyMove.Y, 2));
            EnemyMove_norm = EnemyMove / EnemyMove_length;
            enemyPoint1 = enemyPoint1 - EnemyMove_norm * 0.7;
            
            PaintEnemy1(enemyPoint1);

            if ((Math.Abs(enemyPoint1.X - currentPosition.X) < 10) &&
                (Math.Abs(enemyPoint1.Y - currentPosition.Y) < 10))
            {
                GameOver();
            }

            int n = 0;
            foreach (Point point in enemies)
            {
                if ((Math.Abs(point.X - bulletPosition.X) < 30) &&
                    (Math.Abs(point.Y - bulletPosition.Y) < 30))
                {
                    enemies.RemoveAt(n);
                    paintCanvas.Children.RemoveAt(n);
                    paintCanvas.Children.Remove(bullet);
                    bulletTimer.Stop();
                    PaintEnemy(n);
                    playerone.Score += 100;
                    txbScore.Text = "Score: " + Convert.ToString(playerone.Score);
                    break;
                }
                n++;
            }

            try
            {
                foreach (Point point in rocks)
                {
                    if ((Math.Abs(point.X - bulletPosition.X) < 30) &&
                        (Math.Abs(point.Y - bulletPosition.Y) < 30))
                    {
                        paintCanvas.Children.Remove(bullet);
                        bulletTimer.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bulletTimer_Tick(object sender, EventArgs e)
        {
            if (bulletPosition.Y > minimi && bulletPosition.Y < maxHeight 
                && bulletPosition.X > minimi && bulletPosition.X < maxWidth)
            {
                bulletPosition = bulletPosition + bulletMove_norm * difficulty;
                PaintBullet(bulletPosition);
            }
            else {
                bulletTimer.Stop();
                paintCanvas.Children.Remove(bullet);
            }
        }

        private void GameOver()
        {
            timer.Stop();
            bulletTimer.Stop();
            btnOK.Visibility = Visibility.Visible;
            txtName.Visibility = Visibility.Visible;
        }

        private void GameOverShow()
        {
            //txtMessage.Text = "Your score: " + score + "\npress Esc to quit";
            //paintCanvas.Children.Add(txtMessage);
            //animaatio joka siirtää kanvaasin
            var trs = new TranslateTransform();
            var anim = new DoubleAnimation(0, 620, TimeSpan.FromSeconds(15));
            trs.BeginAnimation(TranslateTransform.XProperty, anim);
            trs.BeginAnimation(TranslateTransform.YProperty, anim);
            paintCanvas.RenderTransform = trs;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "Give Player Name" || txtName.Text == "")
            { MessageBox.Show("Not valid name!"); }
            else
            {
                try
                {
                    playerone.Name = txtName.Text;
                    SaveScore();
                    //txtMessage.Text = "Your score: " + score + "\npress Esc to quit";
                    //paintCanvas.Children.Add(txtMessage);
                    GameOverShow();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SaveScore()
        {
            string name = playerone.Name;
            int finalscore = playerone.Score;
            try
            {
                string path = "Scoreboard.txt";
                // Tutkitaan onko tiedosto olemassa, ja jollei ole niin luodaan tiedosto, ja siihen kolme riviä
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("Name  Score  Date");
                        sw.WriteLine(name + " " + finalscore + " " + DateTime.Now.ToString("dd-MM-yyyy"));
                    }
                }

                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(name + "  " + finalscore + "  " + DateTime.Now.ToString("dd-MM-yyyy"));
                    }
                }
            }

            catch (Exception ex)
            {
                //("Some exception happened!");
                MessageBox.Show(ex.Message);
            }
        }
    }
}