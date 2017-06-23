using System;
using System.Windows;
using CLClient;
using CLClient.Entities;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Drawing;

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
            MoneyDeposit.Text = "0";
            MoneyCurrent.Text = user.money.ToString();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainMenuWindow.Show();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int money;
            if (!Int32.TryParse(MoneyDeposit.Text,out money) || money<0){
                errorMessage.Text = "Please enter a positive number";
            }
            else
            {
                bool? m = CommClient.editUserProfile(LoginWindow.user.id, username.Text, password.Text, email.Text, null, money);

                if (m.HasValue && m.Value)
                {
                    Hide();
                    LoginWindow.user.name = username.Text;
                    LoginWindow.user.password = password.Text;
                    LoginWindow.user.email = email.Text;
                    LoginWindow.user.money += money;
                    //TODO: add a profile picture
                    MainMenuWindow.Show();
                }
                else
                {
                    errorMessage.Text = "Could not edit user profile.";
                }
            }
        }

        private void selectPic_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            OpenFileDialog dlg = new OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";

            // Display OpenFileDialog by calling ShowDialog method 
            DialogResult result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                // Open document 
                string filename = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filename);
                bitmap.EndInit();
                profilePictre.Source = bitmap;
                Image img = Image.FromFile(filename);
            }
        }
    }
}
