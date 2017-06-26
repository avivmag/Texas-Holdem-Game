using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CLClient;
using CLClient.Entities;
using static PL.SearchGameWindow;

namespace PL
{
    /// <summary>
    /// Interaction logic for SelectGameWindow.xaml
    /// </summary>
    public partial class SelectGameWindow : Window
    {
        private Window mainMenuWindow;

        public SelectGameWindow(Window mainMenuWindow, string joinOperation)
        {

            InitializeComponent();
            this.mainMenuWindow = mainMenuWindow;
            actionBtn.Content = joinOperation;
            List<TexasHoldemGame> allGames = CommClient.findAllActiveAvailableGames();

            int i = 0;

            if (allGames == null)
            {
                MessageBox.Show("No active games.");
            }

            else
            {
                foreach (TexasHoldemGame game in allGames)
                {
                    if (joinOperation.Equals("Spectate") &&
                        (game.gamePreferences.isSpectateAllowed) || (joinOperation.Equals("Join")))
                    {
                        selectGameGrid.Items.Add(new TexasHoldemGameStrings(i, game));
                        i++;
                    }
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
                var game = CommClient.spectateActiveGame(LoginWindow.user.id, gameId);
                if (game != default(TexasHoldemGame))
                {
                    Close();
                    mainMenuWindow.Show();
                    var gw = new GameWindow(game, LoginWindow.user.id, true);
                    game.Subscribe(gw);
                    gw.Show();
                }
                else
                {
                    errorMessage.Text = "Could not spectate chosen game.";
                }
            }
            else
            {
                DataGridCellInfo cellValue = (selectGameGrid.SelectedCells.ElementAt(1));
                gameId = int.Parse(((TexasHoldemGameStrings)cellValue.Item).gameId);
                var game = CommClient.GetGameInstance(gameId, LoginWindow.user.id);

                if (game != default(TexasHoldemGame))
                {
                    Close();
                    mainMenuWindow.Show();
                    var gameWindow = new GameWindow(game, LoginWindow.user.id,false);
                    game.Subscribe(gameWindow);
                    gameWindow.Show();
                }
                else
                {
                    errorMessage.Text = "Could not join chosen game.";
                }
            }
        }

        private void selectGameGrid_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (selectGameGrid.SelectedIndex != -1)
            {
                actionBtn.IsEnabled = true;   
            }
        }
    }
}
