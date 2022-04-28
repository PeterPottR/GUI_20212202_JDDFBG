using System.Windows;

namespace SurviveTheExam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void newGame(object sender, RoutedEventArgs e)
        {
            //ha a playername.Text != string.empty-vel akk
            //Application.Current.MainWindow.Content = new fő idító meghívása(playerName.Text)
            //else MessageBox.Show("Enter Player Name!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Leaderboard(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new LeaderBoard();
        }
    }
}
