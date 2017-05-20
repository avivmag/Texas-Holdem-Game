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
using System.Windows.Controls.Primitives;
using CLClient;
using CLClient.Entities;
using static CLClient.Entities.Card;

namespace PL
{

    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private int playerSeatIndex;
        private int systemUserId;
        private TexasHoldemGame game;
        private Player mePlayer;


        private Button[] seatsButtons;
        private Label[] playerNames;
        private Image[] playerStateBackground;
        private Image[] playersImage;
        private Image[][] playerCards;
        private Image[] communityCards;
        private int CARD_TYPE = 5;
        private Label[] coinsSumInHeap;
        private Image[] coinsImagesInHeap;
        private bool[] alreadyAddedMouseEvents;
        private Image[] allInIcons;
        private Image[] dealerIcons;
        private Image[] bigIcons;
        private Image[] smallIcons;
        private Label[] playerCoins;
        private Label[] playerCoinsGambled;
        private TextBox betTextBox;
        private TextBlock messagesTextBlock;
        private TextBox messagesTextBox;
        Button betButton;
        Button checkButton;
        Button foldButton;
        
        // for testing
        //public GameWindow()
        //{
        //    InitializeComponent();

        //    game = new TexasHoldemGame();
        //    game.players = new Player[9];
        //    playerSeatIndex = 3;

        //    initializeScreen();
        //    placePlayer(playerSeatIndex, "profile_pic", "gil", 1000);
        //    placePlayer(4, "profile_pic", "aviv", 100);
        //    removePlayer(4);
        //    SetDealerBigSmallIcons(0, 8, 7);
        //    SetDealerBigSmallIcons(1, 2, 3);
        //    movePlayersCoinsToHeap(0);
        //}

        public GameWindow(TexasHoldemGame game, int systemUserId)
        {
            InitializeComponent();
            this.game = game;
            this.systemUserId = systemUserId;
            initializeScreen();
        }

        /// <summary>
        /// initialization of all arrays and stuff
        /// </summary>
        private void initializeScreen()
        {
            seatsButtons = new Button[game.players.Length];
            playerNames = new Label[game.players.Length];
            playerStateBackground = new Image[game.players.Length];
            playersImage = new Image[game.players.Length];
            playerCards = new Image[game.players.Length][];
            seatButtonToImageDictionary = new Dictionary<Button, Image>();
            seatButtonToSeatIndex = new Dictionary<Button, int>();
            allInIcons = new Image[game.players.Length];
            dealerIcons = new Image[game.players.Length];
            bigIcons = new Image[game.players.Length];
            smallIcons = new Image[game.players.Length];
            playerCoins = new Label[game.players.Length];
            playerCoinsGambled = new Label[game.players.Length];
            alreadyAddedMouseEvents = new bool[game.players.Length];

            for (int i = 0; i < seatsButtons.Length; i++)
            {
                initializeVariables(i);
                positionElementsOnScreen(i);
                if (i == 7)
                    addControlBar();
            }
        }

        private void addControlBar()
        {
            UniformGrid mainControlBarUg = new UniformGrid();
            UniformGrid checkFoldBetCommentControlBarUg = new UniformGrid();
            UniformGrid checkFoldControlBarUg = new UniformGrid();
            UniformGrid betControlBarUg = new UniformGrid();
            mainControlBarUg.Columns = 1;
            checkFoldBetCommentControlBarUg.Columns = 1;
            checkFoldControlBarUg.Rows = 1;
            betControlBarUg.Rows = 1;
            betButton = new Button();
            checkButton = new Button();
            foldButton = new Button();
            Button commentButton = new Button();
            messagesTextBlock = new TextBlock();
            messagesTextBox = new TextBox();
            betTextBox = new TextBox();

            betButton.Click += BetButton_Click;
            checkButton.Click += CheckButton_Click;
            foldButton.Click += FoldButton_Click;
            commentButton.Click += CommentButton_Click;

            betButton.Content = "Bet";
            checkButton.Content = "Check";
            foldButton.Content = "Fold";
            commentButton.Content = "Send";

            checkFoldControlBarUg.Children.Add(checkButton);
            checkFoldControlBarUg.Children.Add(foldButton);
            betControlBarUg.Children.Add(betButton);

            Border border = new Border();
            border.Background = Brushes.SkyBlue;
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(1);
            border.Child = betTextBox;
            betControlBarUg.Children.Add(border);

            border = new Border();
            border.Background = Brushes.SkyBlue;
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(1);
            border.Child = messagesTextBox;

            checkFoldBetCommentControlBarUg.Children.Add(border);
            checkFoldBetCommentControlBarUg.Children.Add(commentButton);
            checkFoldBetCommentControlBarUg.Children.Add(checkFoldControlBarUg);
            checkFoldBetCommentControlBarUg.Children.Add(betControlBarUg);

            ScrollViewer sv = new ScrollViewer();
            sv.Content = messagesTextBlock;
            border = new Border();
            border.Background = Brushes.SkyBlue;
            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(1);
            border.Child = sv;

            mainControlBarUg.Children.Add(border);
            mainControlBarUg.Children.Add(checkFoldBetCommentControlBarUg);

            BottomRow.Children.Add(mainControlBarUg);
        }

        /// <summary>
        /// Adds the element to the appropriate grid on the screen
        /// </summary>
        /// <param name="i"></param>
        private void positionElementsOnScreen(int i)
        {
            UniformGrid playerUg = makePlayerUniformGrid(i);

            if (i < 3)
                TopRow.Children.Add(playerUg);
            else if (i == 3)
            {
                UpperMiddleRow.Children.Add(playerUg);
                AddCommunityCards();
            }
            else if (i == 4)
                UpperMiddleRow.Children.Add(playerUg);
            else if (i == 5)
            {
                LowerMiddleRow.Children.Add(playerUg);
                AddCoins();
            }
            else if (i == 6)
                LowerMiddleRow.Children.Add(playerUg);
            else
                BottomRow.Children.Add(playerUg);
        }

        /// <summary>
        /// creates and returns a grid for the seat i including everything - cards, 4 icons, name, image, sum of tokens, tokens which he bets on in this round.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private UniformGrid makePlayerUniformGrid(int i)
        {
            initializePlayerIcon(i);

            playerNames[i].HorizontalAlignment = HorizontalAlignment.Center;
            playerNames[i].VerticalAlignment = VerticalAlignment.Center;
            playerNames[i].HorizontalContentAlignment = HorizontalAlignment.Center;
            playerNames[i].VerticalContentAlignment = VerticalAlignment.Center;

            playerCoins[i].Content = 0;
            playerCoins[i].HorizontalAlignment = HorizontalAlignment.Center;
            playerCoins[i].VerticalAlignment = VerticalAlignment.Center;
            playerCoins[i].HorizontalContentAlignment = HorizontalAlignment.Center;
            playerCoins[i].VerticalContentAlignment = VerticalAlignment.Center;
            playerCoins[i].Foreground = Brushes.Green;

            playerCoinsGambled[i].Content = 0;
            playerCoinsGambled[i].HorizontalAlignment = HorizontalAlignment.Center;
            playerCoinsGambled[i].VerticalAlignment = VerticalAlignment.Center;
            playerCoinsGambled[i].HorizontalContentAlignment = HorizontalAlignment.Center;
            playerCoinsGambled[i].VerticalContentAlignment = VerticalAlignment.Center;
            playerCoinsGambled[i].Foreground = Brushes.Blue;

            UniformGrid cardsUg = new UniformGrid();
            cardsUg.HorizontalAlignment = HorizontalAlignment.Center;
            cardsUg.Rows = 1;
            cardsUg.Children.Add(playerCards[i][0]);
            cardsUg.Children.Add(playerCards[i][1]);

            UniformGrid playerIconsUg = new UniformGrid();
            playerIconsUg.Rows = 1;
            playerIconsUg.HorizontalAlignment = HorizontalAlignment.Center;

            UniformGrid dealerAllInIconsUg = new UniformGrid();
            dealerAllInIconsUg.Columns = 1;
            dealerAllInIconsUg.Children.Add(dealerIcons[i]);
            dealerAllInIconsUg.Children.Add(allInIcons[i]);

            UniformGrid bigSmallIconsUg = new UniformGrid();
            bigSmallIconsUg.Columns = 1;
            bigSmallIconsUg.Children.Add(bigIcons[i]);
            bigSmallIconsUg.Children.Add(smallIcons[i]);

            UniformGrid playerInfoUg = new UniformGrid();
            playerInfoUg.Columns = 1;
            playerInfoUg.Children.Add(playerNames[i]);
            playerInfoUg.Children.Add(playerCoins[i]);
            playerInfoUg.Children.Add(playerCoinsGambled[i]);

            playerIconsUg.Children.Add(dealerAllInIconsUg);
            playerIconsUg.Children.Add(seatsButtons[i]);
            playerIconsUg.Children.Add(playerInfoUg);
            playerIconsUg.Children.Add(bigSmallIconsUg);

            UniformGrid playerUg = new UniformGrid();
            playerUg.Columns = 1;
            playerUg.Children.Add(cardsUg);
            playerUg.Children.Add(playerIconsUg);
            playerUg.Margin = new Thickness(10);
            return playerUg;
        }

        /// <summary>
        /// simple initialization of seat - just create it on the screen, does not take in account anything about the game state
        /// </summary>
        /// <param name="i"></param>
        private void initializeVariables(int i)
        {
            seatsButtons[i] = new Button();
            playerCards[i] = new Image[2];
            playerCards[i][0] = DrawCard(cardType.unknown, CARD_TYPE);
            playerCards[i][0].Margin = new Thickness(5);
            playerCards[i][1] = DrawCard(cardType.unknown, CARD_TYPE);
            playerCards[i][1].Margin = new Thickness(5);
            playerNames[i] = new Label();
            allInIcons[i] = new Image();
            dealerIcons[i] = new Image();
            bigIcons[i] = new Image();
            smallIcons[i] = new Image();
            allInIcons[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/all_in_icon.png"));
            allInIcons[i].Visibility = Visibility.Hidden;
            dealerIcons[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/dealer_icon.png"));
            dealerIcons[i].Visibility = Visibility.Hidden;
            bigIcons[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/big_icon.png"));
            bigIcons[i].Visibility = Visibility.Hidden;
            smallIcons[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/small_icon.png"));
            smallIcons[i].Visibility = Visibility.Hidden;
            playerCoins[i] = new Label();
            playerCoinsGambled[i] = new Label();
            playerStateBackground[i] = new Image();
            playersImage[i] = new Image();

            Canvas canvas = new Canvas();
            canvas.Height = 90;
            canvas.Width = 90;
            playerStateBackground[i].Width = 90;
            playersImage[i].Width = 80;
            playerStateBackground[i].Height = 90;
            playersImage[i].Height = 90;
            playersImage[i].Margin = new Thickness(3, -2, 0, 0);
            Canvas.SetLeft(playerStateBackground[i], -2);
            Canvas.SetTop(playerStateBackground[i], -2);
            canvas.Children.Add(playerStateBackground[i]);
            canvas.Children.Add(playersImage[i]);
            seatsButtons[i].Content = canvas;
        }

        /// <summary>
        /// initiates the player which is playing in index i of the game
        /// </summary>
        /// <param name="i"></param>
        private void initializePlayerIcon(int i)
        {
            seatButtonToImageDictionary[seatsButtons[i]] = playerStateBackground[i];
            seatButtonToSeatIndex[seatsButtons[i]] = i;
            seatsButtons[i].Width = 90;
            seatsButtons[i].Height = 90;
            if (game.players[i] == null)
                changeSeat(i, true, "gray", "free_seat_icon", "free seat", 0, 0);
            else
            {
                string background = null;
                switch (game.players[i].playerState)
                {
                    case Player.PlayerState.folded:
                        background = "red";
                        break;
                    case Player.PlayerState.in_round:
                        background = "green";
                        break;
                    case Player.PlayerState.my_turn:
                        background = "blue";
                        break;
                    case Player.PlayerState.not_in_round:
                        background = "gray";
                        break;
                }
                changeSeat(i, false, background, game.players[i].imageUrl, game.players[i].name, 0, 0);
            }
        }

        private IDictionary<Button, Image> seatButtonToImageDictionary;
        private IDictionary<Button, int> seatButtonToSeatIndex;
        private void FreeSeatEventMouseEnter(object sender, EventArgs e)
        {
            seatButtonToImageDictionary[(Button)sender].Source = new BitmapImage(new Uri("pack://application:,,,/resources/green.png"));
        }

        private void FreeSeatEventMouseLeave(object sender, EventArgs e)
        {
            seatButtonToImageDictionary[(Button)sender].Source = new BitmapImage(new Uri("pack://application:,,,/resources/gray.png"));
        }
        
        private void GameWindow_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.ChoosePlayerSeat(this.game.gameId, seatButtonToSeatIndex[(Button)sender]);

            if(!returnMessage.success)
                MessageBox.Show(returnMessage.description);
        }

        private void placePlayer(int i, string playerImageUrl, string playerName, int coins)
        {
            changeSeat(i, false, "green", playerImageUrl, playerName, coins, 0);
        }

        private void removePlayer(int i)
        {
            changeSeat(i, true, "gray", "free_seat_icon", "free seat", 0, 0);
        }

        private void changeSeat(int i, Boolean addMouseEvents, string playerStateImageUrl, string playerImageUrl, string playerName, int playerCoinsNumber, int playerCoinsNumberGambled)
        {
            if (addMouseEvents && !alreadyAddedMouseEvents[i])
            {
                seatsButtons[i].MouseEnter += FreeSeatEventMouseEnter;
                seatsButtons[i].MouseLeave += FreeSeatEventMouseLeave;
                seatsButtons[i].Click += GameWindow_Click;
                alreadyAddedMouseEvents[i] = true;
            }
            if (!addMouseEvents && alreadyAddedMouseEvents[i])
            {
                seatsButtons[i].MouseEnter -= FreeSeatEventMouseEnter;
                seatsButtons[i].MouseLeave -= FreeSeatEventMouseLeave;
                seatsButtons[i].Click -= GameWindow_Click;
                alreadyAddedMouseEvents[i] = false;
            }

            playerStateBackground[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/" + playerStateImageUrl + ".png"));
            playersImage[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/" + playerImageUrl + ".png"));

            playerNames[i].Content = playerName;
            playerCoins[i].Content = playerCoinsNumber;
            playerCoinsGambled[i].Content = playerCoinsNumberGambled;
        }
        
        private Image DrawCard(cardType type, int cardNumber)
        {
            // matching to the sprite
            int col = cardNumber - 1;
            int row = 0;
            switch (type)
            {
                case cardType.club:
                    row = 2;
                    break;
                case cardType.diamond:
                    row = 1;
                    break;
                case cardType.heart:
                    row = 0;
                    break;
                case cardType.spade:
                    row = 3;
                    break;
                case cardType.unknown:
                    row = 4;
                    break;
            }
            // Create an Image element.
            Image croppedImage = new Image();
            //croppedImage.Width = 72;
            //croppedImage.Height = 100;
            //croppedImage.Margin = new Thickness(5);

            // Create a CroppedBitmap based off of a xaml defined resource.
            CroppedBitmap cb = new CroppedBitmap(
               new BitmapImage(new Uri("pack://application:,,,/resources/cards_sprite.gif")),
               new Int32Rect(col * 72, row * 100, 72, 100));       //select region rect
            croppedImage.Source = cb;                 //set image source to cropped
            return croppedImage;
        }

        private void AddCommunityCards()
        {
            UniformGrid ug = new UniformGrid();
            ug.Rows = 1;
            communityCards = new Image[5];
            for (int i = 0; i < communityCards.Length; i++)
            {
                communityCards[i] = DrawCard(cardType.unknown, CARD_TYPE);
                communityCards[i].Margin = new Thickness(5);
                ug.Children.Add(communityCards[i]);
            }
            UpperMiddleRow.Children.Add(ug);
        }

        private void AddCoins()
        {
            UniformGrid bigUg = new UniformGrid();
            bigUg.Rows = 1;
            UniformGrid[] ug = new UniformGrid[5];
            coinsImagesInHeap = new Image[5];
            coinsSumInHeap = new Label[5];
            for (int i = 0; i < communityCards.Length; i++)
            {
                coinsImagesInHeap[i] = new Image();
                coinsImagesInHeap[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/coins.png"));
                //coinsImages[i].Margin = new Thickness(15);
                coinsSumInHeap[i] = new Label();
                coinsSumInHeap[i].Content = 0;
                coinsSumInHeap[i].HorizontalAlignment = HorizontalAlignment.Center;
                coinsSumInHeap[i].VerticalAlignment = VerticalAlignment.Center;
                coinsSumInHeap[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                coinsSumInHeap[i].VerticalContentAlignment = VerticalAlignment.Center;

                //if (i != 0)
                //{
                //    coinsImagesInHeap[i].Visibility = Visibility.Hidden;
                //    coinsSumInHeap[i].Visibility = Visibility.Hidden;
                //}
                ug[i] = new UniformGrid();
                ug[i].Columns = 1;
                ug[i].Children.Add(coinsImagesInHeap[i]);
                ug[i].Children.Add(coinsSumInHeap[i]);
            }
            bigUg.Children.Add(ug[4]);
            bigUg.Children.Add(ug[2]);
            bigUg.Children.Add(ug[0]);
            bigUg.Children.Add(ug[1]);
            bigUg.Children.Add(ug[3]);
            LowerMiddleRow.Children.Add(bigUg);
        }

        // change the dealer, big and small
        public void SetDealerBigSmallIcons(int dealerIndex, int bigIndex, int smallIndex)
        {
            for (int i = 0; i < game.players.Length; i++)
            {
                if (i == dealerIndex)
                    dealerIcons[i].Visibility = Visibility.Visible;
                else
                    dealerIcons[i].Visibility = Visibility.Hidden;

                if (i == bigIndex)
                    bigIcons[i].Visibility = Visibility.Visible;
                else
                    bigIcons[i].Visibility = Visibility.Hidden;

                if (i == smallIndex)
                    smallIcons[i].Visibility = Visibility.Visible;
                else
                    smallIcons[i].Visibility = Visibility.Hidden;
            }
        }

        // move all players bet to the heap
        public void movePlayersCoinsToHeap(int heapIndex)
        {
            int sum, temp;
            int.TryParse(coinsSumInHeap[heapIndex].Content.ToString(), out sum);
            for (int i = 0; i < game.players.Length; i++)
            {
                int.TryParse(playerCoinsGambled[i].Content.ToString(), out temp);
                sum += temp;
                playerCoinsGambled[i].Content = 0;
            }
            coinsSumInHeap[heapIndex].Content = sum;
        }

        // mov heap coins to specific player
        public void moveHeapToPlayerCoins(int heapIndex, int playerIndex)
        {
            int sum, temp;
            int.TryParse(coinsSumInHeap[heapIndex].Content.ToString(), out sum);
            coinsSumInHeap[heapIndex].Content = 0;

            int.TryParse(playerCoins[playerIndex].Content.ToString(), out temp);
            temp += sum;
            playerCoins[playerIndex].Content = temp;
        }

        // raise bet of some player
        private void BetButton_Click(object sender, RoutedEventArgs e)
        {
            int playerCoinsNum, playerCoinsGambledNum, coins;
            int.TryParse(playerCoins[playerSeatIndex].Content.ToString(), out playerCoinsNum);
            int.TryParse(playerCoinsGambled[playerSeatIndex].Content.ToString(), out playerCoinsGambledNum);
            //          int inserted                           put the minimal bet at least and not all in              tried to put more coins that he had
            if (!Int32.TryParse(betTextBox.Text, out coins) || (coins < getMinimumBet() && coins != playerCoinsNum) || playerCoinsNum < coins)
            {
                MessageBox.Show("bad bet number entered");
                return;
            }

            ReturnMessage returnMessage = CommClient.Bet(game.gameId, playerSeatIndex, coins);
            if (returnMessage.success)
            {
                playerCoinsNum -= coins;
                playerCoinsGambledNum += coins;

                playerCoins[playerSeatIndex].Content = playerCoinsNum;
                playerCoinsGambled[playerSeatIndex].Content = playerCoinsGambledNum;

                if (playerCoinsNum <= 0)
                    allInIcons[playerSeatIndex].Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show(returnMessage.description);
                return;
            }
        }

        private int getMinimumBet()
        {
            int temp, ans = 0;
            for (int i = 0; i < seatsButtons.Length; i++)
            {
                int.TryParse(playerCoinsGambled[i].Content.ToString(), out temp);
                ans = Math.Max(temp, ans);
            }
            return ans;
        }

        // send a comment
        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.AddMessage(game.gameId, playerSeatIndex, messagesTextBox.Text);
            if (returnMessage.success)
                messagesTextBox.Text = "";
            else
                MessageBox.Show(returnMessage.description);
        }

        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.Fold(game.gameId, playerSeatIndex);

            if (returnMessage.success)
                seatButtonToImageDictionary[seatsButtons[playerSeatIndex]].Source = new BitmapImage(new Uri("pack://application:,,,/resources/red.png"));
            else
                MessageBox.Show(returnMessage.description);
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.Check(game.gameId, playerSeatIndex);

            if (returnMessage.success)
                seatButtonToImageDictionary[seatsButtons[playerSeatIndex]].Source = new BitmapImage(new Uri("pack://application:,,,/resources/green.png"));
            else
                MessageBox.Show(returnMessage.description);
        }
        
        private void getPlayer()
        {
            mePlayer = CommClient.GetPlayer(this.game.gameId, playerSeatIndex);
        }

        // TODO: Gili or Or, this is the function that needs to be called when updating the cards of the player
        private void updatePlayerCards()
        {
            Card[] cards = CommClient.GetPlayerCards(this.game.gameId, playerSeatIndex);
            for(int i = 0; i < playerCards[playerSeatIndex].Length; i++)
                playerCards[playerSeatIndex][i] = DrawCard(cards[i].Type, cards[i].Value);
        }

        private void cardsShowOff()
        {
            IDictionary<int, Card[]> seatIndexToCards = CommClient.GetShowOff(this.game.gameId);
            foreach (KeyValuePair<int, Card[]> entry in seatIndexToCards)
            {
                for (int i = 0; i < entry.Value.Length; i++)
                    playerCards[entry.Key][i] = DrawCard(entry.Value[i].Type, entry.Value[i].Value);
            }
        }

        // TODO: Gili or Or, this is the function that needs to be called when updating the state of the game
        public void updateGame()
        {
            TexasHoldemGame game = CommClient.GetGameState(this.game.gameId);
            //int pot;
            coinsSumInHeap[0].Content = game.pot;

            //List<Card> flop;
            for(int i = 0; i < game.flop.Count; i++)
                communityCards[i] = DrawCard(game.flop[i].Type, game.flop[i].Value);
            //Card turn;
            communityCards[3] = DrawCard(game.turn.Type, game.turn.Value);
            //Card river;
            communityCards[4] = DrawCard(game.river.Type, game.river.Value);

            //Player[] players;
            //int currentDealer;
            //int currentBig;
            //int currentSmall;
            for (int i = 0; i < game.players.Length; i++)
            {
                if(playerSeatIndex == -1 && game.players[i] == null)
                    changeSeat(i, true, "gray", "free_seat_icon", "free seat", 0, 0);
                else if (playerSeatIndex != -1 && game.players[i] == null)
                    changeSeat(i, false, "gray", "free_seat_icon", "free seat", 0, 0);
                else //if (game.players[i] != null)
                {
                    switch (game.players[i].playerState)
                    {
                        case Player.PlayerState.folded:
                            changeSeat(i, false, "red", game.players[i].imageUrl, game.players[i].name, game.players[i].Tokens, game.players[i].TokensInBet);
                            break;
                        case Player.PlayerState.in_round:
                            changeSeat(i, false, "green", game.players[i].imageUrl, game.players[i].name, game.players[i].Tokens, game.players[i].TokensInBet);
                            break;
                        case Player.PlayerState.not_in_round:
                            changeSeat(i, false, "gray", game.players[i].imageUrl, game.players[i].name, game.players[i].Tokens, game.players[i].TokensInBet);
                            break;
                        case Player.PlayerState.my_turn:
                            changeSeat(i, false, "blue", game.players[i].imageUrl, game.players[i].name, game.players[i].Tokens, game.players[i].TokensInBet);
                            if(systemUserId == game.players[i].systemUserID)
                            {
                                betButton.IsEnabled = true;
                                foldButton.IsEnabled = true;
                                checkButton.IsEnabled = true;
                            }
                            else
                            {
                                betButton.IsEnabled = false;
                                foldButton.IsEnabled = false;
                                checkButton.IsEnabled = false;
                            }
                            break;
                    }
                }
                if (i == game.currentDealer)
                    dealerIcons[i].Visibility = Visibility.Visible;
                else
                    dealerIcons[i].Visibility = Visibility.Hidden;
                if (i == game.currentBig)
                    bigIcons[i].Visibility = Visibility.Visible;
                else
                    bigIcons[i].Visibility = Visibility.Hidden;
                if (i == game.currentSmall)
                    smallIcons[i].Visibility = Visibility.Visible;
                else
                    smallIcons[i].Visibility = Visibility.Hidden;
            }
        }

        // TODO: Gili or Or, this is the function that needs to be called when someone sends a chat message
        public void updateChatBox(string appendedLine)
        {
            messagesTextBlock.Text += appendedLine + "\n";
        }
    }
}
