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
using Backend;
using Backend.User;
using SL;

namespace PL
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private SLInterface sl;
        private Window MainMenuWindow;

        public OptionsWindow(Window MainMenuWindow)
        {
            InitializeComponent();
            this.MainMenuWindow = MainMenuWindow;
            sl = LoginWindow.sl;
            SystemUser user = LoginWindow.mySystemUser;
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
                ReturnMessage m = sl.editUserProfile(LoginWindow.mySystemUser.id, username.Text, password.Text, email.Text, null);

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
