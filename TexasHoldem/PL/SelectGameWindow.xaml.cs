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
                    if ((joinOperation.Equals("Spectate") &&
                        game.GamePreferences.IsSpectatingAllowed.HasValue &&
                        game.GamePreferences.IsSpectatingAllowed.Value) || (joinOperation.Equals("Join")))
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
                    this.Close();
                    mainMenuWindow.Show();
                    new GameWindow(game, LoginWindow.user.id).Show();
                }
                else
                {
                    errorMessage.Text = "Could not spectate chosen game.";
                }
            }
            else
            {
                DataGridCellInfo cellValue = (selectGameGrid.SelectedCells.ElementAt(1));
                gameId = Int32.Parse(cellValue.ToString());
                var game = CommClient.joinActiveGame(LoginWindow.user.id, gameId);
                if (game != default(TexasHoldemGame))
                {
                    this.Close();
                    mainMenuWindow.Show();
                    new GameWindow(game, LoginWindow.user.id).Show();
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
