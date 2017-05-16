using System;
using System.Windows;
using CLClient;
using CLClient.Entities;

namespace PL
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private Window MainMenuWindow;

        public OptionsWindow(Window MainMenuWindow)
        {
            InitializeComponent();
            this.MainMenuWindow = MainMenuWindow;
            SystemUser user = LoginWindow.user;
            username.Text = user.name;
            password.Text = user.password;
            email.Text = user.email;
            MoneyDeposit.Text = user.money.ToString();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainMenuWindow.Show();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int money;
            if (!Int32.TryParse(MoneyDeposit.Text,out money) || money<=0){
                errorMessage.Text = "Please enter a positive number";
            }
            else
            {
                ReturnMessage m = sl.editUserProfile(LoginWindow.user.id, username.Text, password.Text, email.Text, null);

                if (m.success)
                {
                    this.Hide();
                    MainMenuWindow.Show();
                }
                else
                {
                    errorMessage.Text = m.description;
                }
            }
        }
    }
}
