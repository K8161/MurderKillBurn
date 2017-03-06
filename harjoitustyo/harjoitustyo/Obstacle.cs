using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace harjoitustyo
{
    class Obstacle
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public int Hitpoints { get; set; }

        public void AlterHitpoints()
        {

        }

        private void btnHighScore_Click(object sender, RoutedEventArgs e)
        {
            StartWindow window1 = new StartWindow();
            window1.Show();
            this.Close();
        }
    }
}
