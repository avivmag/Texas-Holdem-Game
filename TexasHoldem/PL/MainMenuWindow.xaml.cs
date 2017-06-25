using System.Windows;
using CLClient;
using CLClient.Entities;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        private Window loginWindow;

        public MainMenuWindow(Window loginWindow)
        {
            InitializeComponent();
            this.loginWindow = loginWindow;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            CommClient.Logout(LoginWindow.user.id);
            LoginWindow.user = null;
            CommClient.closeConnection();
            Close();
            loginWindow.Show();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new OptionsWindow(this).Show();
        }

        private void Search_Game_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new SearchGameWindow(this).Show();
        }

        private void Specate_Game_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new SelectGameWindow(this,"Spectate").Show();
        }

        private void Join_Game_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new SelectGameWindow(this,"Join").Show();
        }

        private void Create_Game_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new CreateGameWindow(this).Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            CommClient.Logout(LoginWindow.user.id);
            Application.Current.Shutdown();
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new SelectReplayWindow(this).Show();
        }
    }
}
