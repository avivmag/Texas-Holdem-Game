using System.Windows;
using CLClient;
using CLClient.Entities;

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private Window loginWindow;

        public RegisterWindow(Window loginWindow)
        {
            InitializeComponent();
            this.loginWindow = loginWindow;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            loginWindow.Show();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var user = CommClient.Register(this.username.Text, this.password.Text, this.email.Text, "");

            if (user != default(SystemUser)){
                LoginWindow.user = user;
                this.Close();
                new MainMenuWindow(loginWindow).Show();
            }
            else
            {
                this.errorMessage.Text ="Could not register at the moment.";
            }
        }
    }
}
