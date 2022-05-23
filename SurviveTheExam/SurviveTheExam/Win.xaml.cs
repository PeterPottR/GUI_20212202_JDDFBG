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

namespace SurviveTheExam
{
    /// <summary>
    /// Interaction logic for Win.xaml
    /// </summary>
    public partial class Win : Window
    {
        public Win()
        {
            InitializeComponent();
        }

        private void LB(object sender, RoutedEventArgs e)
        {
            LeaderBoard ld = new LeaderBoard();
            ld.Show();
            Window.GetWindow(this).Close();
        }


        private void MN(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            Window.GetWindow(this).Close();
        }
    }
}
