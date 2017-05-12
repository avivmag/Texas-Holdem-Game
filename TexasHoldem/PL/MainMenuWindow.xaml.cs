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
using SL;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        private SLInterface sl;
        private Window loginWindow;

        public MainMenuWindow(Window loginWindow)
        {
            InitializeComponent();
            this.sl = LoginWindow.sl;
            this.loginWindow = loginWindow;
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            sl.Logout(LoginWindow.mySystemUser.name);
            LoginWindow.mySystemUser = null;
            this.Close();
            loginWindow.Show();
        }

        private void Options_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new OptionsWindow(this).Show();
        }

        private void Search_Game_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new SearchGameWindow(this).Show();
        }

        private void Specate_Game_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new SelectGameWindow(this,"Spectate").Show();
        }

        private void Join_Game_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new SelectGameWindow(this,"Join").Show();
        }

        private void Create_Game_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            new CreateGameWindow(this).Show();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
