using System;
using System.Windows;
using CLClient;
using CLClient.Entities;
using CLClient.Entities.DecoratorPreferences;

namespace PL
{
    /// <summary>
    /// Interaction logic for CreateGameWindow.xaml
    /// </summary>
    public partial class CreateGameWindow : Window
    {
        private Window mainMenuWindow;

        public CreateGameWindow(Window MainMenuWindow)
        {
            InitializeComponent();
            mainMenuWindow = MainMenuWindow;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
            mainMenuWindow.Show();
        }

        private void Create_Game_Click(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
            var game = getGame();
            if (game == null)
            {
                errorMessage.Text = "Could not create the game";
            }
            else
            {
                Close();
                mainMenuWindow.Show();
                new GameWindow(game, LoginWindow.user.id).Show();

            }
        }
        private TexasHoldemGame getGame()
        {
            Preference.GameTypePolicy gamePolicy = Preference.GameTypePolicy.Undef;
            int limitPolicy;
            int buyInPolicy;
            int startingChips;
            int minimalBet;
            int minimalPlayers;
            int maximalPlayers;
            bool? spectateAllowed;
            bool? isLeague;

            if (GameTypePolicyComboBox.Text.Equals("none") || GameTypePolicyComboBox.Text.Equals(""))
            {
                gamePolicy = Preference.GameTypePolicy.Undef;
            }
            else
            {
                gamePolicy = (Preference.GameTypePolicy)Enum.Parse(typeof(Preference.GameTypePolicy), GameTypePolicyComboBox.Text);
            }

            if (gamePolicy == Preference.GameTypePolicy.Limit)
            {
                if (limitPolicyTextbox.Text.Equals("") || !Int32.TryParse(buyInTextbox.Text, out limitPolicy) || limitPolicy < 0)
                {
                    errorMessage.Text = "Wrong Input - limit policy should be int and positive.";
                    return null;
                }
            }
            else
                limitPolicy = 0;

            if (buyInTextbox.Text.Equals(""))
            {
                buyInPolicy = -1;
            }
            else if (!Int32.TryParse(buyInTextbox.Text, out buyInPolicy) || buyInPolicy < 0)
            {
                errorMessage.Text = "Wrong Input - buy in policy should be int and positive.";
                return null;
            }

            if (startingChipsTextbox.Text.Equals(""))
            {
                startingChips = -1;
            }
            else if (!Int32.TryParse(startingChipsTextbox.Text, out startingChips) || startingChips < 0)
            {
                errorMessage.Text = "Wrong Input - starting chips should be int and positive.";
                return null;
            }

            if (minimalBetTextbox.Text.Equals(""))
            {
                minimalBet = -1;
            }
            else if (!Int32.TryParse(minimalBetTextbox.Text, out minimalBet) || minimalBet < 0)
            {
                errorMessage.Text = "Wrong Input - minimal bet should be int and positive.";
                return null;
            }

            if (minimalPlayerTextbox.Text.Equals(""))
            {
                minimalPlayers = -1;
            }
            else if (!Int32.TryParse(minimalPlayerTextbox.Text, out minimalPlayers) || minimalPlayers < 0)
            {
                errorMessage.Text = "Wrong Input - minimal players should be int and positive.";
                return null;
            }

            if (maximalPlayerTextbox.Text.Equals(""))
            {
                maximalPlayers = -1;
            }
            else if (!Int32.TryParse(maximalPlayerTextbox.Text, out maximalPlayers) || maximalPlayers < 0)
            {
                errorMessage.Text = "Wrong Input - maximal players should be int and positive.";
                return null;
            }

            //if (spectateAllowedTextbox.Text.Equals(""))
            //{
            //    spectateAllowed = true;
            //}
            //else
            //{
                spectateAllowed = Convert.ToBoolean(spectateAllowedTextbox.Text);
            //}

            //if (isLeagueTextbox.Text.Equals(""))
            //{
            //    isLeague = false;
            //}
            //else
            //{
                isLeague = Convert.ToBoolean(spectateAllowedTextbox.Text);
            //}

            return CommClient.CreateGame(LoginWindow.user.id, gamePolicy.ToString() , limitPolicy, buyInPolicy, startingChips, minimalBet, minimalPlayers, maximalPlayers, spectateAllowed, isLeague);
            //return null;
        }

        private void GameTypePolicyComboBox_Selected(object sender, RoutedEventArgs e)
        {
            if (GameTypePolicyComboBox.Text.Equals("Limit"))
                limitPolicyTextbox.IsEnabled = true;
            else
                limitPolicyTextbox.IsEnabled = false;
        }

        private void CleaerBtn_Click(object sender, RoutedEventArgs e)
        {
            GameTypePolicyComboBox.SelectedItem = "none";
            limitPolicyTextbox.IsEnabled = false;
            limitPolicyTextbox.Text = "";
            buyInTextbox.Text = "";
            startingChipsTextbox.Text = "";
            minimalBetTextbox.Text = "";
            minimalPlayerTextbox.Text = "";
            maximalPlayerTextbox.Text = "";
            spectateAllowedTextbox.Text = "True";
            isLeagueTextbox.Text = "False";
        }
    }
}
