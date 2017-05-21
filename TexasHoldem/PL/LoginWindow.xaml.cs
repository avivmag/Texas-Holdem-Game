using System.Windows;
using CLClient;
using CLClient.Entities;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, PLInterface
    {
        internal static SystemUser user;

        public LoginWindow()
        {
            InitializeComponent();            
        }

        public void Run()
        {
            Show();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            user = CommClient.Login(username.Text, password.Password);

            if (user == null)
            {
                MessageBox.Show("Wrong input.");
                return;
            }
            Hide();
            errorMessage.Text = "";
            new MainMenuWindow(this).Show();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            errorMessage.Text = "";
            new RegisterWindow(this).Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
