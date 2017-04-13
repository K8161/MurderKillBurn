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
        //constants, variables, objects and lists/arrays
        #region
        //map boundaries
        public const int minimi = 20;
        public const int maxHeight = 880;
        public const int maxWidth = 1580;

        //enemies
        private int minDamage = 5; //minimum attack damage from an enemy
        private int maxDamage = 25; //maximum attack damage from an enemy
        private int enemyCounter = 0;
        private List<Point> enemies = new List<Point>(); //viholliskokoelma
        private List<Vector> monsterVectors = new List<Vector>(); //lista liikkuvien vihollisten sijainteja varten
        private Vector enemySpawn = new Vector(600, 600);
        private List<Enemy> Enemies = new List<Enemy>();
        private const int enemyCount = 20; // vihollisten maara
        
        //timers
        private DispatcherTimer timer;
        private DispatcherTimer bulletTimer;
        private int difficulty = 5; //timerin ajastin aika ms

        private const int bulletWidth = 10;
        Ellipse bullet = new Ellipse();
        Vector bulletMove_norm;
        private List<Weapon> bullets = new List<Weapon>();
        private int bulletcount = 0;
        private Vector bulletPosition = new Vector();
        
        Ellipse rock;
        private List<Point> rocks = new List<Point>(); //kivikokoelma
        private const int obstacleCount = 15;

        //objects derived from classes
        Enemy[] monsters = new Enemy[enemyCount];
        Player playerone = new Player();
        Enemy movingMonster = new Enemy();
        Obstacle stone = new Obstacle();

        //randomizer
        private Random rnd = new Random(); //pisteiden arvontaa varten
        #endregion

        public MainWindow()
        {
            try
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
                movingMonster.ScoreValue = 200;

                //määritellään ikkunalle tapahtumankäsittelijä näppäimistön kuuntelua varten
                this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
                this.MouseMove += new MouseEventHandler(Rotate);
                this.MouseMove += new MouseEventHandler(charMove);
                this.MouseDown += new MouseButtonEventHandler(Shoot);

                //piirretään kivet, pelaaja ja viholliset
                IniRocks();
                PaintPlayerOne(playerone.startingPoint);
                playerone.currentPosition = playerone.startingPoint;
                playerone.Hitpoints = 100;
                playerone.Ammo = 10;
                paintCanvas.Children.Add(playerone.character);
                IniEnemies();
                pgbHealth.DataContext = playerone;

                //start game
                timer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        public void IniEnemies()
        {

            for (int i = 0; i < enemyCount - 1; i++) // luodaan vihollisia ennaltamääritelty määrä
            {
                monsters[i] = new Enemy(); //inserts an instance of class Enemy, into an objectarray derived from Enemy
            }

            for (int i = 0; i < enemyCount - 1; i++)
            {
                monsters[i].PaintMonster();
                monsters[i].Damage = rnd.Next(minDamage, maxDamage); //randomizes enemy attack damage to between min and max values
                monsters[i].EnemyPosition = new Vector(rnd.Next(minimi, maxWidth),
                                                       rnd.Next(minimi, maxHeight));
                paintCanvas.Children.Add(monsters[i].character);
            }
            PaintMovingMonsters(new Vector(rnd.Next(minimi, maxWidth),
                                           rnd.Next(minimi, maxHeight)));
        } 

        public void charMove(object sender, MouseEventArgs e)
        {
            //gets the mousepointer coordinates from the window and relays them into a Point variable
            Point targ = e.GetPosition(paintCanvas);
            playerone.Move(targ);
        }

        private void Shoot(object sender, MouseButtonEventArgs e)
        {
            if (timer.IsEnabled)
            {
                try
                {
                    switch (e.LeftButton)
                    {
                        case MouseButtonState.Pressed:

                            bulletPosition = playerone.currentPosition;
                            Point target = e.GetPosition(paintCanvas);
                            Vector targetVec = new Vector(target.X, target.Y);
                            Vector bulletVec = new Vector(playerone.currentPosition.X, playerone.currentPosition.Y); ;

                            if (playerone.Ammo > 0)
                            {
                                paintCanvas.Children.Add(bullet);
                                bulletTimer.Start();
                                playerone.Ammo--;
                                txbMag.Text = "Ammo left: " + Convert.ToString(playerone.Ammo);
                            }

                            else
                            {
                                txbMag.Text = " Ammo left: Reloading...";
                                playerone.Ammo = 10;
                            }
                            Vector bulletMove = targetVec - bulletVec;
                            double bulletMove_length = Math.Sqrt(Math.Pow(bulletMove.X, 2) + Math.Pow(bulletMove.Y, 2)) / 4;
                            bulletMove_norm = bulletMove / bulletMove_length;
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Shoot1(object sender, MouseButtonEventArgs e)
        {
            Weapon bullet1 = new Weapon();
            bullets.Add(bullet1);
            bullet1.PaintBullet();
        }

        private void Rotate(object sender, MouseEventArgs e)
        {
            try
            {
                //gets the mousepointer coordinates from the window and relays them into a Point variable
                Point targetPoint = e.GetPosition(this);
                playerone.Rotation(targetPoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PaintRocks(int index)
        {
            //randomizes coordinates for a rock
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

        private void PaintPlayerOne(Vector currentpoint)
        {
            playerone.PaintPlayer();
            Canvas.SetTop(playerone.character, currentpoint.Y);
            Canvas.SetLeft(playerone.character, currentpoint.X);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MonsterFollow()
        {
            monsters[enemyCounter].MonsterPositionLogic(playerone.currentPosition); //sets enemies to home on to player's current position
        }

        private void MonsterCollisionDetection()
        {
            if ((Math.Abs(monsters[enemyCounter].EnemyPosition.X - playerone.currentPosition.X) < 10) &&
                (Math.Abs(monsters[enemyCounter].EnemyPosition.Y - playerone.currentPosition.Y) < 10))
            {
                if (playerone.Hitpoints - monsters[enemyCounter].Damage > 0)
                {
                    //if enemy does damage to player with more than zero hitpoints left, decrease hitpoints accordingly to dealt damage
                    playerone.Hitpoints -= monsters[enemyCounter].Damage;
                    Recoil();
                }
                else
                {
                    GameOver();
                }
            }

            if ((Math.Abs(monsters[enemyCounter].EnemyPosition.X - bulletPosition.X) < playerone.characterWidth) &&
                (Math.Abs(monsters[enemyCounter].EnemyPosition.Y - bulletPosition.Y) < playerone.characterWidth))
            {
                //if bullet proximity to enemy less than characterWidth, kill enemy
                KillMonster();
            }
        }

        private void Recoil()
        {
            //player is bounced opposite from the direction of the attack
            if (playerone.currentPosition.X - monsters[enemyCounter].EnemyPosition.X > 5)
            {
                playerone.currentPosition.X += 10;
            }

            if (playerone.currentPosition.X - monsters[enemyCounter].EnemyPosition.X < 5)
            {
                playerone.currentPosition.X -= 10;
            }
            if (playerone.currentPosition.Y - monsters[enemyCounter].EnemyPosition.X > 5)
            {
                playerone.currentPosition.Y += 10;
            }

            if (playerone.currentPosition.Y - monsters[enemyCounter].EnemyPosition.X < 5)
            {
                playerone.currentPosition.Y -= 10;
            }
        }

        private void PaintMovingMonsters(Vector enemyPoint1)
        {
                monsters[enemyCounter].PaintMonster();
                Canvas.SetTop(monsters[enemyCounter].character, monsters[enemyCounter].EnemyPosition.Y);
                Canvas.SetLeft(monsters[enemyCounter].character, monsters[enemyCounter].EnemyPosition.X);
        } 

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            //keyboard controls
            switch (e.Key)
            {
                case Key.P:
                    if (timer.IsEnabled)
                        timer.Stop(); //pause game
                    else
                        timer.Start(); //resume game if previously paused
                    break;

                case Key.Escape:
                    if (timer.IsEnabled)
                        GameOver(); //forfeit game
                    else
                        this.Close();
                    break;

                    case Key.Up:
                        if (playerone.currentPosition.Y > minimi)
                        playerone.currentPosition = playerone.currentPosition + playerone.charMove_norm * difficulty;
                        break;

                    case Key.Down:
                        if (playerone.currentPosition.Y < maxHeight)
                        playerone.currentPosition = playerone.currentPosition - playerone.charMove_norm * difficulty;
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            PaintPlayerOne(playerone.currentPosition);
            for (enemyCounter = 0; enemyCounter < enemyCount - 1; enemyCounter++)
            {
                MonsterFollow();
                PaintMovingMonsters(monsters[enemyCounter].EnemyPosition);
                MonsterCollisionDetection();
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
            else
            {
                bulletTimer.Stop();
                paintCanvas.Children.Remove(bullet);
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

        private void KillMonster()
        {
            playerone.Score += monsters[enemyCounter].ScoreValue; //add scorevalue to score when an enemy is dispatched from the monsters array
            paintCanvas.Children.Remove(bullet);
            txbScore.Text = "Score: " + Convert.ToString(playerone.Score);
            bulletTimer.Stop();
            monsters[enemyCounter].EnemyPosition = new Vector(rnd.Next(minimi, maxWidth), //spawn new enemies and randomize their spawnpoints
                                                              rnd.Next(minimi, maxHeight));
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
            var trs = new TranslateTransform();
            var anim = new DoubleAnimation(0, 620, TimeSpan.FromSeconds(10));
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
                //probes if the above mentioned file exists
                if (!File.Exists(path))
                {
                    //if not, create a new file to write to
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine("Name  Score  Date");
                        sw.WriteLine(name + " " + finalscore + " " + DateTime.Now.ToString("dd-MM-yyyy"));
                    }
                }
                else
                {
                    //if yes, then write into it
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(name + "  " + finalscore + "  " + DateTime.Now.ToString("dd-MM-yyyy"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}