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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace harjoitustyo
{
    /// <summary>
    /// Interaction logic for HighScoreWindow.xaml
    /// </summary>
    public partial class HighScoreWindow : Window
    {
        public HighScoreWindow()
        {
            InitializeComponent();
        }

        private void btnHighScore_Click(object sender, RoutedEventArgs e)
        {
            StartWindow window1 = new StartWindow();
            window1.Show();
            this.Close();
        }
    }
}
