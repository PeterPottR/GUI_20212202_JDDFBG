using System.Windows;

namespace SurviveTheExam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string name = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            this.playerName.Text = name;
        }

        public MainWindow(string playerName) : base()
        {
            name = playerName;
        }

        private void newGame(object sender, RoutedEventArgs e)
        {
            if (playerName.Text != string.Empty)
            {
                Application.Current.MainWindow.Content = new GameLogic(this.playerName.Text);
            }
            else MessageBox.Show("Enter Player Name!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void Leaderboard(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Content = new LeaderBoard();
        }
    }
}
