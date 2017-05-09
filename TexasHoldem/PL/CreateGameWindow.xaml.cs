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
using static Backend.Game.GamePreferences;

namespace PL
{
    /// <summary>
    /// Interaction logic for CreateGameWindow.xaml
    /// </summary>
    public partial class CreateGameWindow : Window
    {
        private SLInterface sl = LoginWindow.sl;
        private Window mainMenuWindow;

        public CreateGameWindow(Window MainMenuWindow)
        {
            InitializeComponent();
            this.mainMenuWindow = MainMenuWindow;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainMenuWindow.Show();
        }

        private void Create_Game_Click(object sender, RoutedEventArgs e)
        {
            errorMessage.Text = "";
            TexasHoldemGame game = getGame();
            if (game == null)
            {
                errorMessage.Text = "Could not create the game";
            }
            else
            {
                this.Close();
                new GameWindow(mainMenuWindow, game);
            }
        }
        private TexasHoldemGame getGame()
        {
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
                gamePolicy = (GameTypePolicy)Enum.Parse(typeof(GameTypePolicy), GameTypePolicyComboBox.Text);
            }

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

            if (spectateAllowedTextbox.Text.Equals("") || spectateAllowedTextbox.Text.Equals("none"))
            {
                spectateAllowed = null;
            }
            else
            {
                spectateAllowed = Convert.ToBoolean(spectateAllowedTextbox.Text);
            }
            //TODO: add user id.
            return sl.createGame(0,gamePolicy, buyInPolicy, startingChips, minimalBet, minimalPlayers, maximalPlayers, spectateAllowed);
        }
    }
}
