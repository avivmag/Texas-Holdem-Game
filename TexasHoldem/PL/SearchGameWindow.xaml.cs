using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CLClient;
using CLClient.Entities;
using CLClient.Entities.DecoratorPreferences;

namespace PL
{

    /// <summary>
    /// Interaction logic for SearchGameWindow.xaml
    /// </summary>
    public partial class SearchGameWindow : Window
    {
        private Window mainMenuWindow;
        private string selectedCheckBox;

        public SearchGameWindow(Window mainMenuWindow)
        {
            InitializeComponent();

            selectedCheckBox = playerNameCheckBox.Name;
            this.mainMenuWindow = mainMenuWindow;
        }

        private void enableNameCheckBox(object sender, RoutedEventArgs e)
        {
            if (potSizeCheckBox != null)
            {
                //filter by pot size
                potSizeCheckBox.IsChecked = false;
                potSizeTextbox.IsEnabled = false;
                potSizeTextbox.Clear();

                //filter by preferences
                gamePrefCheckBox.IsChecked = false;
                setGamePrefElement(false);

                //filter by player name
                playerNameTextbox.IsEnabled = true;
                selectedCheckBox = playerNameCheckBox.Name;
            }

        }

        private void enablePotSizeText(object sender, RoutedEventArgs e)
        {
            //filter by pot
            playerNameCheckBox.IsChecked = false;
            playerNameTextbox.IsEnabled = false;
            playerNameTextbox.Clear();

            //filter by preferences
            gamePrefCheckBox.IsChecked = false;
            setGamePrefElement(false);

            //filter by pot size
            selectedCheckBox = potSizeCheckBox.Name;
            potSizeTextbox.IsEnabled = true;
        }

        private void enableGamePrefTexts(object sender, RoutedEventArgs e)
        {
            //filter by player
            playerNameCheckBox.IsChecked = false;
            playerNameTextbox.IsEnabled = false;
            playerNameTextbox.Clear();

            //filter by pot size
            potSizeCheckBox.IsChecked = false;
            potSizeTextbox.IsEnabled = false;
            potSizeTextbox.Clear();

            //filter by game pref
            selectedCheckBox = gamePrefCheckBox.Name;
            gamePrefCheckBox.IsChecked = true;
            setGamePrefElement(true);
        }

        //This method disable/enable all the preferences search components
        private void setGamePrefElement(bool enable)
        {
            GameTypePolicyComboBox.IsEnabled = enable;
            buyInTextbox.IsEnabled = enable;
            startingChipsTextbox.IsEnabled = enable;
            minimalBetTextbox.IsEnabled = enable;
            minimalPlayerTextbox.IsEnabled = enable;
            maximalPlayerTextbox.IsEnabled = enable;
            spectateAllowedTextbox.IsEnabled = enable;
            isLeagueTextbox.IsEnabled = enable;

            //delete all values if needed
            if (!enable)
            {
                buyInTextbox.Clear();
                startingChipsTextbox.Clear();
                minimalBetTextbox.Clear();
                minimalPlayerTextbox.Clear();
                maximalPlayerTextbox.Clear();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
            mainMenuWindow.Show();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            playerNameCheckBox.IsChecked = true;
            selectedCheckBox = playerNameCheckBox.Name;
            playerNameTextbox.Clear();
            potSizeTextbox.Clear();
            setGamePrefElement(false);
            joinGameBtn.IsEnabled = false;
            spectateGameBtn.IsEnabled = false;
            searchResultGrid.Items.Clear();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            List<TexasHoldemGame> gamesFound = new List<TexasHoldemGame>();
            if (playerNameCheckBox.IsChecked.HasValue && playerNameCheckBox.IsChecked.Value)
            {
                if (playerNameTextbox.Text.Equals(""))
                {
                    errorMessage.Text = "Wrong Input - empty player name.";
                }
                else
                {
                    gamesFound = CommClient.filterActiveGamesByPlayerName(playerNameTextbox.Text);
                }
            }
            else if (potSizeCheckBox.IsChecked.HasValue && potSizeCheckBox.IsChecked.Value)
            {
                int potSize;
                if (potSizeTextbox.Text.Equals(""))
                {
                    errorMessage.Text = "Wrong Input - empty pot size.";
                }
                else if (!Int32.TryParse(potSizeTextbox.Text, out potSize) || potSize < 0)
                {
                    errorMessage.Text = "Wrong Input - pot size should be int and positive.";
                }
                else
                {
                    gamesFound = CommClient.filterActiveGamesByPotSize(potSize);
                }
            }
            else
            {
                gamesFound = getFilteredGameByPreferences();
            }
            if (gamesFound == null ||gamesFound.Count == 0)
            {
                errorMessage.Text = "Couldn't find any games try to change your criterya.";
            }
            else
            {
                int i = 0;
                foreach (TexasHoldemGame game in gamesFound)
                {
                    searchResultGrid.Items.Add(new TexasHoldemGameStrings(i,game));
                    i++;
                }

            }
        }

        private List<TexasHoldemGame> getFilteredGameByPreferences()
        {
            List<TexasHoldemGame> ans = new List<TexasHoldemGame>();
            string gamePolicy;
            int? gameLimitPolicy;
            int? buyInPolicy;
            int? startingChips;
            int? minimalBet;
            int? minimalPlayers;
            int? maximalPlayers;
            bool? spectateAllowed;
            bool? isLeague;

            int value;
            //set the game type policy
            if (GameTypePolicyComboBox.Text.Equals("none") || GameTypePolicyComboBox.Text.Equals(""))
            {
                gamePolicy = "No_Limit";
            }
            else
            {
                gamePolicy = GameTypePolicyComboBox.Text;
            }

            // Set the game limit policy.
            if (LimitPolicyTextbox.Text.Equals(""))
            {
                gameLimitPolicy = null;
            }
            else if (!Int32.TryParse(LimitPolicyTextbox.Text, out value) || value < 0)
            {
                errorMessage.Text = "Wrong Input - limit policy text box should be int and positive.";
                return ans;
            }
            else
            {
                gameLimitPolicy = value;
            }

            //set the buy in
            if (buyInTextbox.Text.Equals(""))
                buyInPolicy = null;
            else if (!Int32.TryParse(buyInTextbox.Text, out value) || value < 0)
            {
                errorMessage.Text = "Wrong Input - buy in policy should be int and positive.";
                return ans;
            }
            else
                buyInPolicy = value;

            // set the starting chips amount
            if (startingChipsTextbox.Text.Equals(""))
                startingChips = null;
            else if (!Int32.TryParse(startingChipsTextbox.Text, out value) || value < 0)
            {
                errorMessage.Text = "Wrong Input - starting chips should be int and positive.";
                return ans;
            }
            else
                startingChips = value;

            // set the minimal bet
            if (minimalBetTextbox.Text.Equals(""))
                minimalBet = null;
            else if (!Int32.TryParse(minimalBetTextbox.Text, out value) || value < 0)
            {
                errorMessage.Text = "Wrong Input - minimal bet should be int and positive.";
                return ans;
            }
            else
                minimalBet = value;

            //set the minimal Players
            if (minimalPlayerTextbox.Text.Equals(""))
                minimalPlayers = null;
            else if (!Int32.TryParse(minimalPlayerTextbox.Text, out value) || value < 0)
            {
                errorMessage.Text = "Wrong Input - minimal players should be int and positive.";
                return ans;
            }
            else
                minimalPlayers = value;

            //set maximal players
            if (maximalPlayerTextbox.Text.Equals(""))
                maximalPlayers = null;
            else if (!Int32.TryParse(maximalPlayerTextbox.Text, out value) || value < 0)
            {
                errorMessage.Text = "Wrong Input - maximal players should be int and positive.";
                return ans;
            }
            else
                maximalPlayers = value;

            //set spectator
            spectateAllowed = Convert.ToBoolean(spectateAllowedTextbox.Text);

            //set leagues
            //if (isLeagueTextbox.Text.Equals(""))
            //    isLeague = false;
            //else
           
            isLeague = Convert.ToBoolean(isLeagueTextbox.Text);

            return CommClient.filterActiveGamesByGamePreferences(gamePolicy, gameLimitPolicy, buyInPolicy, startingChips, minimalBet, minimalPlayers, maximalPlayers, spectateAllowed, isLeague);

        }

        private void Join_Game_Click(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
            int gameId;
            DataGridCellInfo cellValue = (searchResultGrid.SelectedCells.ElementAt(1));
            gameId = Int32.Parse(((TexasHoldemGameStrings)cellValue.Item).gameId);
            var game = CommClient.GetGameInstance(gameId, LoginWindow.user.id);
            if (game != default(TexasHoldemGame))
            {
                Close();
                mainMenuWindow.Show();
                new GameWindow(game,LoginWindow.user.id).Show();
            }
            else
            {
                errorMessage.Text = "Could not join chosen game.";
            }
        }

        private void Spectate_Game_Click(object sender, RoutedEventArgs e)
        {
            int gameId;
            DataGridCellInfo cellValue = (searchResultGrid.SelectedCells.ElementAt(1));
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

        public class TexasHoldemGameStrings{
            public string row { get; }
            public string gameId { get;  }
            public string GamePolicy { get; }
            public string BuyInPolicy { get; }
            public string StartingChipsAmount { get; }
            public string MinimalBet { get; }
            public string MinPlayers { get; }
            public string MaxPlayers { get; }
            public string IsSpectatingAllowed { get; }
            public string isLeague { get; }

            public TexasHoldemGameStrings(int row,TexasHoldemGame game)
            {
                Preference pref = game.gamePreferences;
                this.row = row.ToString();
                gameId = game.gameId.ToString();
                GamePolicy = pref.gamePolicy.ToString();
                BuyInPolicy = pref.buyInPolicy.ToString();
                StartingChipsAmount = pref.startingChipsPolicy.ToString();
                MinimalBet = pref.minimalBet.ToString();
                MinPlayers = pref.minPlayers.ToString();
                MaxPlayers = pref.maxPlayers.ToString();
                IsSpectatingAllowed = pref.isSpectateAllowed.ToString();
                isLeague = pref.isLeague.ToString();
            }
        }

        private void searchResultGrid_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (searchResultGrid.SelectedIndex != -1)
            {
                joinGameBtn.IsEnabled = true;
                spectateGameBtn.IsEnabled = true;
            }
        }

        private void GameTypePolicyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameTypePolicyComboBox.SelectedValue.ToString().Equals("System.Windows.Controls.ComboBoxItem: Limit"))

            {
                LimitPolicyTextbox.IsEnabled = true;
                LimitPolicyTextbox.Text = "0";
            }
            else if (LimitPolicyTextbox != null)
            {
                LimitPolicyTextbox.IsEnabled = false;
                LimitPolicyTextbox.Text = "0";
            }
        }
    }
}
