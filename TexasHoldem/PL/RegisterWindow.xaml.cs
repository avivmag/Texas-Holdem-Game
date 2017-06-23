using System.Windows;
using CLClient;
using CLClient.Entities;
using System.Drawing;
using System.Windows.Forms;
using System;
using System.Windows.Media.Imaging;

namespace PL
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private Window loginWindow;
        string filename = "resources/anonProfPic.jpg";

        public RegisterWindow(Window loginWindow)
        {
            InitializeComponent();
            this.loginWindow = loginWindow;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            loginWindow.Show();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Image img = Image.FromFile(filename);
            var user = CommClient.Register(username.Text, password.Text, email.Text, img);

            if (user != default(SystemUser)){
                LoginWindow.user = user;
                Close();
                new MainMenuWindow(loginWindow).Show();
                // Create the invisible system messages window.
                var messagesWindow = new SystemMessageWindow();
                user.Subscribe(messagesWindow);
            }
            else
            {
                errorMessage.Text ="Could not register at the moment.";
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
                filename = dlg.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filename);
                bitmap.EndInit();
                profilePictre.Source = bitmap;
            }
        }
    }
}