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
using Backend.Game;
using SL;
using static PL.SearchGameWindow;

namespace PL
{
    /// <summary>
    /// Interaction logic for SelectGameWindow.xaml
    /// </summary>
    public partial class SelectGameWindow : Window
    {
        private Window mainMenuWindow;
        private SLInterface sl = LoginWindow.sl;

        public SelectGameWindow(Window mainMenuWindow, string joinOperation)
        {
            InitializeComponent();
            this.mainMenuWindow = mainMenuWindow;
            this.actionBtn.Content = joinOperation;
            List<TexasHoldemGame> allGames = sl.findAllActiveAvailableGames();

            int i = 0;
            foreach (TexasHoldemGame game in allGames)
            {
                if ((joinOperation.Equals("Spectate") && game.GamePreferences.IsSpectatingAllowed.HasValue && game.GamePreferences.IsSpectatingAllowed.Value) || (joinOperation.Equals("Join"))){
                    selectGameGrid.Items.Add(new TexasHoldemGameStrings(i, game));
                    i++;
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainMenuWindow.Show();
        }

        private void action_Click(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
            int gameId;
            if (actionBtn.Content.Equals("Spectate"))
            {
                DataGridCellInfo cellValue = (selectGameGrid.SelectedCells.ElementAt(1));
                gameId = Int32.Parse(((TexasHoldemGameStrings)cellValue.Item).gameId);
                ReturnMessage m = sl.spectateActiveGame(LoginWindow.mySystemUser, gameId);
                if (m.success)
                {
                    TexasHoldemGame game = sl.getGameById(gameId);
                    this.Close();
                    new GameWindow(mainMenuWindow, game).Show();
                }
                else
                {
                    errorMessage.Text = m.description;
                }
            }
            else
            {
                DataGridCellInfo cellValue = (selectGameGrid.SelectedCells.ElementAt(1));
                gameId = Int32.Parse(cellValue.ToString());
                ReturnMessage m = sl.joinActiveGame(LoginWindow.mySystemUser, gameId);
                if (m.success)
                {
                    TexasHoldemGame game = sl.getGameById(gameId);
                    this.Close();
                    new GameWindow(mainMenuWindow, game).Show();
                }
                else
                {
                    errorMessage.Text = m.description;
                }
            }
        }

        private void selectGameGrid_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (selectGameGrid.SelectedIndex != -1)
            {
                this.actionBtn.IsEnabled = true;   
            }
        }
    }
}
