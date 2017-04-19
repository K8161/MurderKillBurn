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
        private const int minBorder = 20;
        private const int maxHeight = 880;
        private const int maxWidth = 1580;

        //enemies
        private int minDamage = 5; //minimum attack damage from an enemy
        private int maxDamage = 25; //maximum attack damage from an enemy
        private int enemyCounter = 0;
        private int enemyCount = 2; //amount of enemies
        
        //timers
        private DispatcherTimer timer;
        private DispatcherTimer bulletTimer;
        private int difficulty = 5; //used as milliseconds-part in timer

        //projectiles
        private List<Weapon> bullets = new List<Weapon>();
        private List<Point> targets = new List<Point>();
        private int magazineSlot = 0;

        //rocks
        private List<Point> rocks = new List<Point>(); //collection of rocks
        private const int obstacleCount = 15;

        //objects derived from classes
        List<Enemy> monsters = new List<Enemy>(); //list for enemymonsters
        Player playerone = new Player();
        Obstacle stone = new Obstacle();
        Weapon bullet = new Weapon();

        //randomizer
        private Random rnd = new Random(); //for randomizing sizes and positions of rocks and enemies
        #endregion

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                InitializeStuff();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializeStuff()
        {
            //set element visibility
            btnOK.Visibility = Visibility.Hidden;
            txtName.Visibility = Visibility.Hidden;
            txbMag.Visibility = Visibility.Visible;
            txbScore.Visibility = Visibility.Visible;

            //initialize timers
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, difficulty);
            timer.Tick += new EventHandler(timer_Tick);

            bulletTimer = new DispatcherTimer();
            bulletTimer.Interval = new TimeSpan(0, 0, 0, 0, difficulty);
            bulletTimer.Tick += new EventHandler(bulletTimer_Tick);

            //initialize event handlers for using keyboard, mouse movement and mouse button
            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);
            this.MouseMove += new MouseEventHandler(Rotate);
            this.MouseMove += new MouseEventHandler(charMove);
            this.MouseDown += new MouseButtonEventHandler(Shoot1);

            //draw rocks, initial enemies and player
            IniRocks();
            PaintPlayerOne(playerone.startingPoint);
            playerone.currentPosition = playerone.startingPoint;
            paintCanvas.Children.Add(playerone.character);
            IniEnemies();

            //initialize health and ammo for player
            playerone.Hitpoints = 100;
            playerone.Ammo = 10;

            //project health into a progress bar stationed on the canvas
            pgbHealth.DataContext = playerone;

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

        public void IniEnemies()
        {
            try
            {
                for (int i = 0; i < enemyCount; i++) //creates a predetermined amount of enemies
                {
                    Enemy monster = new Enemy();
                    monsters.Add(monster);
                }

                for (int i = 0; i < enemyCount; i++)
                {
                    monsters[i].PaintMonster();
                    monsters[i].Damage = rnd.Next(minDamage, maxDamage); //randomizes enemy attack damage to between min and max values
                    monsters[i].EnemyPosition = new Vector(rnd.Next(minBorder, maxWidth), rnd.Next(minBorder, maxHeight));
                    paintCanvas.Children.Add(monsters[i].character);
                }
                PaintMovingMonsters(new Vector(rnd.Next(minBorder, maxWidth), rnd.Next(minBorder, maxHeight)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

                            Point target = e.GetPosition(paintCanvas);
                            bullet.bulletPosition = playerone.currentPosition;
                            bullet.Fire(target, bullet.bulletPosition);

                            if (playerone.Ammo > 0)
                            {
                                paintCanvas.Children.Add(bullet.bullet);
                                bulletTimer.Start();
                                playerone.Ammo--;
                                txbMag.Text = "Ammo left: " + Convert.ToString(playerone.Ammo);
                            }

                            else
                            {
                                txbMag.Text = " Ammo left: Reloading...";
                                playerone.Ammo = 10;
                            }
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
            if (timer.IsEnabled)
            {
                try
                {
                    switch (e.LeftButton)
                    {
                        case MouseButtonState.Pressed:
                            if (playerone.Ammo > 0)
                            {
                                bullet = new Weapon();
                                bullets.Add(bullet);
                                bullet.BulletVisual();

                                Point target = e.GetPosition(paintCanvas);
                                targets.Add(target);
                                bullets[magazineSlot].bulletPosition = playerone.currentPosition;
                                bullets[magazineSlot].Fire(targets[magazineSlot], bullet.bulletPosition);

                            
                                paintCanvas.Children.Add(bullets[magazineSlot].bullet);
                                bulletTimer.Start();
                                playerone.Ammo--;
                                txbMag.Text = "Ammo left: " + Convert.ToString(playerone.Ammo);
                                magazineSlot++;
                            }

                            else
                            {
                                txbMag.Text = " Ammo left: Reloading...";
                                playerone.Ammo = 10;
                                magazineSlot = 0;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
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
            try
            {
                //randomizes coordinates for a rock
                Point point = new Point(rnd.Next(minBorder, maxWidth),
                                        rnd.Next(minBorder, maxHeight));
                stone.PaintRock(point);
                Canvas.SetTop(stone.rock, point.Y);
                Canvas.SetLeft(stone.rock, point.X);
                paintCanvas.Children.Insert(index, stone.rock);
                rocks.Insert(index, point);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PaintPlayerOne(Vector currentpoint)
        {
            try
            {
                playerone.PaintPlayer();
                Canvas.SetTop(playerone.character, currentpoint.Y);
                Canvas.SetLeft(playerone.character, currentpoint.X);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PaintBullet(Vector bulletPoint)
        {
            try
            {
                bullets[magazineSlot].BulletVisual();
                Canvas.SetTop(bullets[magazineSlot].bullet, bulletPoint.Y);
                Canvas.SetLeft(bullets[magazineSlot].bullet, bulletPoint.X);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MonsterFollow()
        {
            //sets enemies to home on to player's current position
            monsters[enemyCounter].MonsterPositionLogic(playerone.currentPosition);
        }

        private void MonsterCollisionDetection()
        {
            try
            {
                if ((Math.Abs(monsters[enemyCounter].EnemyPosition.X - playerone.currentPosition.X) < 5) &&
                    (Math.Abs(monsters[enemyCounter].EnemyPosition.Y - playerone.currentPosition.Y) < 5))
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

                foreach (Weapon projectile in bullets)
                {

                    if ((Math.Abs(monsters[enemyCounter].EnemyPosition.X - bullets[magazineSlot].bulletPosition.X) < playerone.characterWidth) &&
                        (Math.Abs(monsters[enemyCounter].EnemyPosition.Y - bullets[magazineSlot].bulletPosition.Y) < playerone.characterWidth))
                    {
                        //if bullet proximity to enemy less than characterWidth, kill enemy
                        bullets[magazineSlot].DiscardBullet();
                        paintCanvas.Children.Remove(bullets[magazineSlot].bullet);
                        KillMonster();
                        SpawnMonster();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Recoil()
        {
            try
            {
                //player is staggered away from the direction of the attack
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RestrictMovement()
        {
            try
            {
                //character is pushed away from boundaries
                if (playerone.currentPosition.X <= minBorder)
                {
                    playerone.currentPosition.X += 5;
                }

                if (maxWidth - playerone.currentPosition.X <= 20)
                {
                    playerone.currentPosition.X -= 5;
                }

                if (playerone.currentPosition.Y < minBorder + 50)
                {
                    playerone.currentPosition.Y += 5;
                }

                if (playerone.currentPosition.Y >= maxHeight)
                {
                    playerone.currentPosition.Y -= 5;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PaintMovingMonsters(Vector enemyPoint1)
        {
            try
            {
                monsters[enemyCounter].PaintMonster();
                Canvas.SetTop(monsters[enemyCounter].character, monsters[enemyCounter].EnemyPosition.Y);
                Canvas.SetLeft(monsters[enemyCounter].character, monsters[enemyCounter].EnemyPosition.X);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                    RestrictMovement();
                    playerone.currentPosition = playerone.currentPosition + playerone.charMove_norm * difficulty;
                    break;

                case Key.W:
                    RestrictMovement();
                    playerone.currentPosition = playerone.currentPosition + playerone.charMove_norm * difficulty;
                    break;

                case Key.Down:
                    RestrictMovement();
                        playerone.currentPosition = playerone.currentPosition - playerone.charMove_norm * difficulty;
                    break;

                case Key.S:
                    RestrictMovement();
                    playerone.currentPosition = playerone.currentPosition - playerone.charMove_norm * difficulty;
                    break;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                PaintPlayerOne(playerone.currentPosition);
                for (enemyCounter = 0; enemyCounter < enemyCount; enemyCounter++)
                {
                    MonsterFollow();
                    PaintMovingMonsters(monsters[enemyCounter].EnemyPosition);
                }
                    MonsterCollisionDetection();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void bulletTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (bullet.bulletPosition.Y > minBorder && bullet.bulletPosition.Y < maxHeight
                    && bullet.bulletPosition.X > minBorder && bullet.bulletPosition.X < maxWidth)
                {
                    bullet.bulletPosition = bullet.bulletPosition + bullet.bulletMove_norm * difficulty;
                    PaintBullet(bullet.bulletPosition);
                }
                else
                {
                    bulletTimer.Stop();
                    paintCanvas.Children.Remove(bullet.bullet);
                }
                foreach (Point point in rocks)
                {
                    if ((Math.Abs(point.X - bullet.bulletPosition.X) < stone.rock.ActualWidth - stone.rock.ActualWidth / 2) &&
                        (Math.Abs(point.Y - bullet.bulletPosition.Y) < stone.rock.ActualHeight - stone.rock.ActualHeight / 2))
                    {
                        paintCanvas.Children.Remove(bullet.bullet);
                        bullet.DiscardBullet();
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
            //add scorevalue to score when an enemy is dispatched from the monsters array
            playerone.Score += monsters[enemyCounter].ScoreValue; 

            paintCanvas.Children.Remove(bullet.bullet);
            txbScore.Text = "Score: " + Convert.ToString(playerone.Score);
            bulletTimer.Stop();           
        }

        private void SpawnMonster()
        {
            try
            {
                enemyCount++;

                Enemy monster = new Enemy(); //inserts an instance of class Enemy, into an objectarray derived from Enemy
                monsters.Add(monster);

                monster.PaintMonster();
                monster.Damage = rnd.Next(minDamage, maxDamage); //randomizes enemy attack damage to between min and max values
                monster.EnemyPosition = new Vector(rnd.Next(minBorder, maxWidth), rnd.Next(minBorder, maxHeight));
                paintCanvas.Children.Add(monster.character);
                monsters[enemyCounter].EnemyPosition = new Vector(rnd.Next(minBorder, maxWidth), rnd.Next(minBorder, maxHeight));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void GameOver()
        {
            try
            {
                timer.Stop();
                bulletTimer.Stop();
                btnOK.Visibility = Visibility.Visible;
                txtName.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GameOverShow()
        {
            try
            {
                var trs = new TranslateTransform();
                var anim = new DoubleAnimation(0, 1000, TimeSpan.FromSeconds(1));
                trs.BeginAnimation(TranslateTransform.XProperty, anim);
                trs.BeginAnimation(TranslateTransform.YProperty, anim);
                paintCanvas.RenderTransform = trs;

                StartWindow start = new StartWindow();
                if (!start.IsVisible)
                {
                    start.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "Insert Player Name" || txtName.Text == "")
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