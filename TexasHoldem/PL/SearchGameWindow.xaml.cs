using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Backend;
using Backend.Game;
using SL;
using static Backend.Game.GamePreferences;

namespace PL
{

    /// <summary>
    /// Interaction logic for SearchGameWindow.xaml
    /// </summary>
    public partial class SearchGameWindow : Window
    {
        private SLInterface sl;
        private Window mainMenuWindow;
        private string selectedCheckBox;

        public SearchGameWindow(Window mainMenuWindow)
        {
            InitializeComponent();

            selectedCheckBox = playerNameCheckBox.Name;
            this.mainMenuWindow = mainMenuWindow;
            sl = LoginWindow.sl;
        }

        private void enableNameCheckBox(object sender, RoutedEventArgs e)
        {
            if (potSizeCheckBox != null)
            {
                //filter by pot size
                this.potSizeCheckBox.IsChecked = false;
                this.potSizeTextbox.IsEnabled = false;
                this.potSizeTextbox.Clear();

                //filter by preferences
                this.gamePrefCheckBox.IsChecked = false;
                setGamePrefElement(false);

                //filter by player name
                this.playerNameTextbox.IsEnabled = true;
                selectedCheckBox = playerNameCheckBox.Name;
            }

        }

        private void enablePotSizeText(object sender, RoutedEventArgs e)
        {
            //filter by pot
            this.playerNameCheckBox.IsChecked = false;
            this.playerNameTextbox.IsEnabled = false;
            this.playerNameTextbox.Clear();

            //filter by preferences
            this.gamePrefCheckBox.IsChecked = false;
            setGamePrefElement(false);

            //filter by pot size
            selectedCheckBox = potSizeCheckBox.Name;
            this.potSizeTextbox.IsEnabled = true;
        }

        private void enableGamePrefTexts(object sender, RoutedEventArgs e)
        {
            //filter by player
            this.playerNameCheckBox.IsChecked = false;
            this.playerNameTextbox.IsEnabled = false;
            this.playerNameTextbox.Clear();

            //filter by pot size
            this.potSizeCheckBox.IsChecked = false;
            this.potSizeTextbox.IsEnabled = false;
            this.potSizeTextbox.Clear();

            //filter by game pref
            selectedCheckBox = gamePrefCheckBox.Name;
            this.gamePrefCheckBox.IsChecked = true;
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
            this.Close();
            mainMenuWindow.Show();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            this.playerNameCheckBox.IsChecked = true;
            selectedCheckBox = playerNameCheckBox.Name;
            this.playerNameTextbox.Clear();
            this.potSizeTextbox.Clear();
            setGamePrefElement(false);
            this.joinGameBtn.IsEnabled = false;
            this.spectateGameBtn.IsEnabled = false;
            this.searchResultGrid.Items.Clear();
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
                    gamesFound = sl.filterActiveGamesByPlayerName(playerNameTextbox.Text);
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
                    gamesFound = sl.filterActiveGamesByPotSize(potSize);
                }
            }
            else
            {
                gamesFound = getFilteredGameByPreferences();
            }
            if (gamesFound.Count == 0)
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
            GameTypePolicy gamePolicy;
            int buyInPolicy;
            int startingChips;
            int minimalBet;
            int minimalPlayers;
            int maximalPlayers;
            bool? spectateAllowed;

            if (GameTypePolicyComboBox.Text.Equals("none") || GameTypePolicyComboBox.Text.Equals(""))
            {
                gamePolicy = GameTypePolicy.Undef;
            }
            else
            {
                gamePolicy = (GameTypePolicy) Enum.Parse(typeof(GameTypePolicy), GameTypePolicyComboBox.Text);
            }

            if (buyInTextbox.Text.Equals(""))
            {
                buyInPolicy = -1;
            }
            else if (!Int32.TryParse(buyInTextbox.Text, out buyInPolicy) || buyInPolicy < 0)
            {
                errorMessage.Text = "Wrong Input - buy in policy should be int and positive.";
                return ans;
            }

            if (startingChipsTextbox.Text.Equals(""))
            {
                startingChips = -1;
            }
            else if (!Int32.TryParse(startingChipsTextbox.Text, out startingChips) || startingChips < 0)
            {
                errorMessage.Text = "Wrong Input - starting chips should be int and positive.";
                return ans;
            }

            if (minimalBetTextbox.Text.Equals(""))
            {
                minimalBet = -1;
            }
            else if (!Int32.TryParse(minimalBetTextbox.Text, out minimalBet) || minimalBet < 0)
            {
                errorMessage.Text = "Wrong Input - minimal bet should be int and positive.";
                return ans;
            }

            if (minimalPlayerTextbox.Text.Equals(""))
            {
                minimalPlayers= -1;
            }
            else if (!Int32.TryParse(minimalPlayerTextbox.Text, out minimalPlayers) || minimalPlayers < 0)
            {
                errorMessage.Text = "Wrong Input - minimal players should be int and positive.";
                return ans;
            }

            if (maximalPlayerTextbox.Text.Equals(""))
            {
                maximalPlayers = -1;
            }
            else if (!Int32.TryParse(maximalPlayerTextbox.Text, out maximalPlayers) || maximalPlayers < 0)
            {
                errorMessage.Text = "Wrong Input - maximal players should be int and positive.";
                return ans;
            }

            if (spectateAllowedTextbox.Text.Equals("") || spectateAllowedTextbox.Text.Equals("none"))
            {
                spectateAllowed = null;
            }
            else
            {
                spectateAllowed = Convert.ToBoolean(spectateAllowedTextbox.Text);
            }

            return sl.filterActiveGamesByGamePreferences(gamePolicy, buyInPolicy, startingChips, minimalBet, minimalPlayers, maximalPlayers, spectateAllowed);
        }

        private void Join_Game_Click(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
            int gameId;
            DataGridCellInfo cellValue = (searchResultGrid.SelectedCells.ElementAt(1));
            gameId = Int32.Parse(cellValue.ToString());
            ReturnMessage m = sl.joinActiveGame(LoginWindow.mySystemUser, gameId);
            if (m.success)
            {
                TexasHoldemGame game = sl.getGameById(gameId);
                this.Close();
                new GameWindow(mainMenuWindow,game).Show();
            }
            else
            {
                errorMessage.Text = m.description;
            }
        }

        private void Spectate_Game_Click(object sender, RoutedEventArgs e)
        {
            int gameId;
            DataGridCellInfo cellValue = (searchResultGrid.SelectedCells.ElementAt(1));
            gameId = Int32.Parse(((TexasHoldemGameStrings)cellValue.Item).gameId);
            ReturnMessage m = sl.spectateActiveGame(LoginWindow.mySystemUser, gameId);
            if (m.success)
            {
                TexasHoldemGame game = sl.getGameById(gameId);
                this.Close();
                new GameWindow(mainMenuWindow,game).Show();
            }
            else
            {
                errorMessage.Text = m.description;
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

            public TexasHoldemGameStrings(int row,TexasHoldemGame game)
            {
                GamePreferences pref = game.GamePreferences;
                this.row = row.ToString();
                this.gameId = game.gameId.ToString();
                this.GamePolicy = pref.GamePolicy.ToString();
                this.BuyInPolicy = pref.BuyInPolicy.ToString();
                this.StartingChipsAmount = pref.StartingChipsAmount.ToString();
                this.MinimalBet= pref.MinimalBet.ToString();
                this.MinPlayers = pref.MinPlayers.ToString();
                this.MaxPlayers = pref.MaxPlayers.ToString();
                this.IsSpectatingAllowed = pref.IsSpectatingAllowed.ToString();
            }
        }

        private void searchResultGrid_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (searchResultGrid.SelectedIndex != -1)
            {
                this.joinGameBtn.IsEnabled = true;
                this.spectateGameBtn.IsEnabled = true;
            }
        }
    }
}
