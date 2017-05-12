using System.Windows;
using Backend;
using Backend.User;
using SL;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window, PLInterface
    {
        public static SLInterface sl;
        public static SystemUser mySystemUser;

        public LoginWindow()
        {
            InitializeComponent();
            
            sl = new SLImpl();
        }

        public void Run()
        {
            this.Show();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage message = sl.Login(this.username.Text, this.password.Password);
         
            if (message.success)
            {
                mySystemUser = sl.getUserByName(this.username.Text);
                this.Hide();
                errorMessage.Text = "";
                new MainMenuWindow(this).Show();
            }
            else
            {
                this.errorMessage.Text = message.description;
                this.errorMessage.Visibility = Visibility.Visible;
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
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
