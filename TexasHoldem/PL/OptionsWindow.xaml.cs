using System;
using System.Windows;
using CLClient;
using CLClient.Entities;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using System.Windows.Controls;

namespace PL
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        private Window MainMenuWindow;
        private System.Drawing.Image tempImage = LoginWindow.user.profilePicture;

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
            RankCurrent.Text = user.rank.ToString();

            // Converting system.drawing.image to imageSource.
            var bi = new BitmapImage();

            using (var ms = new MemoryStream())
            {
                user.profilePicture.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ms.Position = 0;

                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.StreamSource = ms;
                bi.EndInit();
            }

            profilePictre.Source = bi;

            //string filePath = String.Join("_", Guid.NewGuid(), user.name);
            //string imagesDirectory = Path.Combine(Environment.CurrentDirectory, "UserImages", filePath);

            //// Save image to disc. (produces error but saves it anyway. we will just wrap it with a 'try' clause.
            //try
            //{
            //    user.profilePicture.Save(imagesDirectory);
            //}
            //catch { }

            //Uri u = new Uri(imagesDirectory);
            //BitmapImage bi = new BitmapImage(u);
            //profilePictre.Source = new BitmapImage(new Uri("pack://application:,,,/resources/profile_pic.png"));
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
                bool? m = CommClient.editUserProfile(LoginWindow.user.id, username.Text, password.Text, email.Text, tempImage, money);

                if (m.HasValue && m.Value)
                {
                    Hide();
                    LoginWindow.user.name = username.Text;
                    LoginWindow.user.password = password.Text;
                    LoginWindow.user.email = email.Text;
                    LoginWindow.user.money += money;
                    LoginWindow.user.profilePicture = tempImage;
                    LoginWindow.user.userImageByteArray = imageToByteArray(tempImage);
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
                tempImage = System.Drawing.Image.FromFile(filename);
            }
        }

        private static byte[] imageToByteArray(System.Drawing.Image image)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] byteArray = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));
            return byteArray;
        }
    }
}
