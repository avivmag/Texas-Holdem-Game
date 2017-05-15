using System.Windows;
using Backend;
using SL;

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private SLInterface sl;
        private Window loginWindow;

        public RegisterWindow(Window loginWindow)
        {
            InitializeComponent();
            this.sl = LoginWindow.sl;
            this.loginWindow = loginWindow;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            loginWindow.Show();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage m = sl.Register(this.username.Text, this.password.Text, this.email.Text, "");

            if (m.success){
                LoginWindow.mySystemUser = sl.getUserByName(username.Text);
                this.Close();
                new MainMenuWindow(loginWindow).Show();
            }
            else
            {
                this.errorMessage.Text = m.description;
            }
        }
    }
}
