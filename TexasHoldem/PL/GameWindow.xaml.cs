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
using System.IO;

namespace PL
{

    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window, IObserver
    {
        private int playerSeatIndex;
        private int systemUserId;
        private TexasHoldemGame game;
        
        private ScrollViewer sv;
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
        private Label[] playerCash;
        private Label[][] playerChips;
        private Label[] playerAmountGambled;
        private TextBox betTextBox;
        private TextBlock messagesTextBlock;
        private TextBox messagesTextBox;
        Button betButton;
        Button checkButton;
        Button callButton;
        Button foldButton;
        Button playButton;
        
        public GameWindow(TexasHoldemGame game, int systemUserId)
        {
            InitializeComponent();
            this.game = game;
            this.systemUserId = systemUserId;
            initializeScreen();
            updateGame(game);
        }

        /// <summary>
        /// initialization of all arrays and stuff
        /// </summary>/
        private void initializeScreen()
        {
            playerSeatIndex = -1;
            seatsButtons = new Button[9];
            playerNames = new Label[9];
            playerStateBackground = new Image[9];
            playersImage = new Image[9];
            playerCards = new Image[9][];
            seatButtonToImageDictionary = new Dictionary<Button, Image>();
            seatButtonToSeatIndex = new Dictionary<Button, int>();
            allInIcons = new Image[9];
            dealerIcons = new Image[9];
            bigIcons = new Image[9];
            smallIcons = new Image[9];
            playerCash = new Label[9];
            playerChips = new Label[9][];
            playerAmountGambled = new Label[9];
            alreadyAddedMouseEvents = new bool[9];
            for (int i = 0; i < alreadyAddedMouseEvents.Length; i++)
                alreadyAddedMouseEvents[i] = false;

            for (int i = 0; i < seatsButtons.Length; i++)
            {
                initializeVariables(i);
            }
            positionElementsOnScreen(8);
            positionElementsOnScreen(0);
            positionElementsOnScreen(1);
            positionElementsOnScreen(7);
            positionElementsOnScreen(2);
            positionElementsOnScreen(6);
            positionElementsOnScreen(3);
            positionElementsOnScreen(5);
            positionElementsOnScreen(4);
        }

        private void addControlBar()
        {
            UniformGrid mainControlBarUg = new UniformGrid();
            UniformGrid checkFoldBetCommentControlBarUg = new UniformGrid();
            UniformGrid checkCallFoldControlBarUg = new UniformGrid();
            UniformGrid commentPlayControlBarUg = new UniformGrid();
            UniformGrid betControlBarUg = new UniformGrid();
            mainControlBarUg.Columns = 1;
            checkFoldBetCommentControlBarUg.Columns = 1;
            checkCallFoldControlBarUg.Rows = 1;
            commentPlayControlBarUg.Rows = 1;
            betControlBarUg.Rows = 1;
            betButton = new Button();
            checkButton = new Button();
            callButton = new Button();
            foldButton = new Button();
            Button commentButton = new Button();
            playButton = new Button();
            messagesTextBlock = new TextBlock();
            messagesTextBox = new TextBox();
            betTextBox = new TextBox();

            betButton.Click += BetButton_Click;
            checkButton.Click += CheckButton_Click;
            callButton.Click += CallButton_Click;
            foldButton.Click += FoldButton_Click;
            commentButton.Click += CommentButton_Click;
            playButton.Click += Play_Click;
            Closed += GameWindow_Closed;

            betButton.Content = "Bet";
            checkButton.Content = "Check";
            callButton.Content = "Call";
            foldButton.Content = "Fold";
            commentButton.Content = "Send";
            playButton.Content = "Play";

            commentPlayControlBarUg.Children.Add(commentButton);
            commentPlayControlBarUg.Children.Add(playButton);
            checkCallFoldControlBarUg.Children.Add(checkButton);
            checkCallFoldControlBarUg.Children.Add(callButton);
            checkCallFoldControlBarUg.Children.Add(foldButton);
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
            checkFoldBetCommentControlBarUg.Children.Add(commentPlayControlBarUg);
            checkFoldBetCommentControlBarUg.Children.Add(checkCallFoldControlBarUg);
            checkFoldBetCommentControlBarUg.Children.Add(betControlBarUg);

            this.sv = new ScrollViewer();
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

            if (i == 0 || i == 1 || i == 8)
                TopRow.Children.Add(playerUg);
            else if (i == 2 || i == 7)
                UpperMiddleRow.Children.Add(playerUg);
            else if (i == 3 || i == 6)
                LowerMiddleRow.Children.Add(playerUg);
            else
                BottomRow.Children.Add(playerUg);

            if (i == 6)
                AddCoins();
            if (i == 7)
                AddCommunityCards();
            if (i == 5)
                addControlBar();
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

            if(game.gameOnChips)
            {
                for(int j = 0; j < 4; i++)
                {
                    playerChips[i][j].Content = 0;
                    playerChips[i][j].HorizontalAlignment = HorizontalAlignment.Center;
                    playerChips[i][j].VerticalAlignment = VerticalAlignment.Center;
                    playerChips[i][j].HorizontalContentAlignment = HorizontalAlignment.Center;
                    playerChips[i][j].VerticalContentAlignment = VerticalAlignment.Center;
                }
                playerChips[i][0].Foreground = Brushes.White;
                playerChips[i][0].Foreground = Brushes.Red;
                playerChips[i][0].Foreground = Brushes.Blue;
                playerChips[i][0].Foreground = Brushes.Green;
            }
            else
            {
                playerCash[i].Content = 0;
                playerCash[i].HorizontalAlignment = HorizontalAlignment.Center;
                playerCash[i].VerticalAlignment = VerticalAlignment.Center;
                playerCash[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                playerCash[i].VerticalContentAlignment = VerticalAlignment.Center;
                playerCash[i].Foreground = Brushes.White;
            }

            playerAmountGambled[i].Content = 0;
            playerAmountGambled[i].HorizontalAlignment = HorizontalAlignment.Center;
            playerAmountGambled[i].VerticalAlignment = VerticalAlignment.Center;
            playerAmountGambled[i].HorizontalContentAlignment = HorizontalAlignment.Center;
            playerAmountGambled[i].VerticalContentAlignment = VerticalAlignment.Center;
            playerAmountGambled[i].Foreground = Brushes.Yellow;

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
            if(game.gameOnChips)
            {
                playerInfoUg.Children.Add(playerChips[i][0]);
                playerInfoUg.Children.Add(playerChips[i][1]);
                playerInfoUg.Children.Add(playerChips[i][2]);
                playerInfoUg.Children.Add(playerChips[i][3]);
            }
            else
                playerInfoUg.Children.Add(playerCash[i]);
            playerInfoUg.Children.Add(playerAmountGambled[i]);

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
        /// simple initialization of seat - just create it, does not take in account anything about the game state
        /// </summary>
        /// <param name="i"></param>
        private void initializeVariables(int i)
        {
            seatsButtons[i] = new Button();
            playerCards[i] = new Image[2];
            playerCards[i][0] = new Image();
            playerCards[i][0].Source = DrawCard(cardType.unknown, CARD_TYPE);
            playerCards[i][0].Margin = new Thickness(5);
            playerCards[i][1] = new Image();
            playerCards[i][1].Source = DrawCard(cardType.unknown, CARD_TYPE);
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
            if(game.gameOnChips)
            {
                playerChips[i] = new Label[4];
                playerChips[i][0] = new Label();
                playerChips[i][1] = new Label();
                playerChips[i][2] = new Label();
                playerChips[i][3] = new Label();
            }
            else
                playerCash[i] = new Label();
            playerAmountGambled[i] = new Label();
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
            //if (game.players[i] == null)
            //    changeSeat(i, true, "gray", "free_seat_icon", "free seat", 0, 0);
            //else
            //{
            //    string background = null;
            //    switch (game.players[i].playerState)
            //    {
            //        case Player.PlayerState.folded:
            //            background = "red";
            //            break;
            //        case Player.PlayerState.in_round:
            //            background = "green";
            //            break;
            //        case Player.PlayerState.my_turn:
            //            background = "blue";
            //            break;
            //        case Player.PlayerState.not_in_round:
            //            background = "gray";
            //            break;
            //        case Player.PlayerState.winner:
            //            background = "yellow";
            //            break;
            //    }
            //    changeSeat(i, false, background, game.players[i].imageUrl, game.players[i].name, 0, 0);
            //}
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

        private void Sit_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.JoinGame(LoginWindow.user.id, this.game.gameId, seatButtonToSeatIndex[(Button)sender]);

            if (returnMessage.success)
                playerSeatIndex = seatButtonToSeatIndex[(Button)sender];
            else
                MessageBox.Show(returnMessage.description);
        }

        //private void placePlayer(int i, string playerImageUrl, string playerName, int coins)
        //{
        //    changeSeat(i, false, "green", playerImageUrl, playerName, coins, 0);
        //}

        //private void removePlayer(int i)
        //{
        //    changeSeat(i, true, "gray", "free_seat_icon", "free seat", 0, 0);
        //}

        private void changeSeat(int i, Boolean addMouseEvents, string playerStateImageUrl, System.Drawing.Image playerImage, string playerName, int playerCoinsNumber, int playerCoinsNumberGambled)
        {
            if (addMouseEvents && !alreadyAddedMouseEvents[i])
            {
                seatsButtons[i].MouseEnter += FreeSeatEventMouseEnter;
                seatsButtons[i].MouseLeave += FreeSeatEventMouseLeave;
                seatsButtons[i].Click += Sit_Click;
                alreadyAddedMouseEvents[i] = true;
            }
            if (!addMouseEvents && alreadyAddedMouseEvents[i])
            {
                seatsButtons[i].MouseEnter -= FreeSeatEventMouseEnter;
                seatsButtons[i].MouseLeave -= FreeSeatEventMouseLeave;
                seatsButtons[i].Click -= Sit_Click;
                alreadyAddedMouseEvents[i] = false;
            }

            playerStateBackground[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/" + playerStateImageUrl + ".png"));
            if (playerImage != null)
            {
                try
                {
                    // Converting system.drawing.image to imageSource.
                    var bi = new BitmapImage();

                    using (var ms = new MemoryStream())
                    {
                        playerImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        ms.Position = 0;

                        bi.BeginInit();
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.StreamSource = ms;
                        bi.EndInit();
                    }

                    playersImage[i].Source = bi;
                }
                catch (Exception e)
                {
                    playersImage[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/profile_pic.png"));
                }
            }
            else
            {
                playersImage[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/" + "free_seat_icon.png"));
            }

            playerNames[i].Content = playerName;
            if(game.gameOnChips)
            {
                playerChips[i][3].Content = playerCoinsNumber / 20;
                playerChips[i][2].Content = (playerCoinsNumber % 20) / 10;
                playerChips[i][1].Content = (playerCoinsNumber % 10) / 5;
                playerChips[i][2].Content = (playerCoinsNumber % 5) / 1;
            }
            else
                playerCash[i].Content = playerCoinsNumber;
            playerAmountGambled[i].Content = playerCoinsNumberGambled;
        }

        private CroppedBitmap DrawCard(cardType type, int cardNumber)
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
            //Image croppedImage = new Image();
            //croppedImage.Width = 72;
            //croppedImage.Height = 100;
            //croppedImage.Margin = new Thickness(5);

            // Create a CroppedBitmap based off of a xaml defined resource.
            CroppedBitmap cb = new CroppedBitmap(
               new BitmapImage(new Uri("pack://application:,,,/resources/cards_sprite.gif")),
               new Int32Rect(col * 72, row * 100, 72, 100));       //select region rect
            return cb;
        }

        private void AddCommunityCards()
        {
            UniformGrid ug = new UniformGrid();
            ug.Rows = 1;
            communityCards = new Image[5];
            for (int i = 0; i < communityCards.Length; i++)
            {
                communityCards[i] = new Image();
                communityCards[i].Source = DrawCard(cardType.unknown, CARD_TYPE);
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
            for (int i = 0; i < coinsSumInHeap.Length; i++)
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

                if (i != 0)
                {
                    coinsImagesInHeap[i].Visibility = Visibility.Hidden;
                    coinsSumInHeap[i].Visibility = Visibility.Hidden;
                }
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


        // raise bet of some player
        private void BetButton_Click(object sender, RoutedEventArgs e)
        {
            int playerCoinsNum = 0, playerCoinsGambledNum, coins;
            if(game.gameOnChips)
            {
                int temp = 0;
                int.TryParse(playerChips[playerSeatIndex][3].Content.ToString(), out temp);
                playerCoinsNum += (temp * 20);
                int.TryParse(playerChips[playerSeatIndex][2].Content.ToString(), out temp);
                playerCoinsNum += (temp * 10);
                int.TryParse(playerChips[playerSeatIndex][1].Content.ToString(), out temp);
                playerCoinsNum += (temp * 5);
                int.TryParse(playerChips[playerSeatIndex][0].Content.ToString(), out temp);
                playerCoinsNum += (temp * 1);
            }
            else
                int.TryParse(playerCash[playerSeatIndex].Content.ToString(), out playerCoinsNum);
            int.TryParse(playerAmountGambled[playerSeatIndex].Content.ToString(), out playerCoinsGambledNum);
            //          int inserted                           put the minimal bet at least and not all in              tried to put more coins that he had
            if (!Int32.TryParse(betTextBox.Text, out coins) || coins < getMinimumBet() || playerCoinsNum < coins)
            {
                MessageBox.Show("bad bet number entered");
                return;
            }

            ReturnMessage returnMessage = CommClient.Bet(game.gameId, playerSeatIndex, coins);

            if (!returnMessage.success)
                MessageBox.Show(returnMessage.description);
        }

        private int getMinimumBet()
        {
            int temp, ans = 0, myCoins = 0;
            for (int i = 0; i < seatsButtons.Length; i++)
            {
                int.TryParse(playerAmountGambled[i].Content.ToString(), out temp);
                ans = Math.Max(temp, ans);
                if (i == playerSeatIndex)
                    myCoins = temp;
            }
            int t = ans - myCoins;
            ans = 0;
            myCoins = 0;
            return t;
        }

        // send a comment
        private void CommentButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.AddMessage(game.gameId, systemUserId, messagesTextBox.Text);
            if ((returnMessage != null) && (returnMessage.success))
                messagesTextBox.Text = "";
            if ((returnMessage != null) && (!returnMessage.success))
                MessageBox.Show(returnMessage.description);
        }

        private void FoldButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.Fold(game.gameId, playerSeatIndex);

            //if (returnMessage.success)
            //    seatButtonToImageDictionary[seatsButtons[playerSeatIndex]].Source = new BitmapImage(new Uri("pack://application:,,,/resources/red.png"));
           if (!returnMessage.success)
                MessageBox.Show(returnMessage.description);
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.Check(game.gameId, playerSeatIndex);

            //if (returnMessage.success)
            //    seatButtonToImageDictionary[seatsButtons[playerSeatIndex]].Source = new BitmapImage(new Uri("pack://application:,,,/resources/green.png"));
            if (!returnMessage.success)
                MessageBox.Show(returnMessage.description);
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.Call(game.gameId, playerSeatIndex, getMinimumBet());
            
            if (!returnMessage.success)
                MessageBox.Show(returnMessage.description);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            ReturnMessage returnMessage = CommClient.playGame(game.gameId);

            if (returnMessage == null)
                return;

            if (!returnMessage.success)
                MessageBox.Show(returnMessage.description);
            else
                playButton.IsEnabled = false;
        }

        private void GameWindow_Closed(object sender, EventArgs e)
        {
            ReturnMessage returnMessage = CommClient.RemoveUser(game.gameId, systemUserId);

            if (!returnMessage.success)
                MessageBox.Show(returnMessage.description);
        }

        //private void getPlayer()
        //{
        //    mePlayer = CommClient.GetPlayer(this.game.gameId, playerSeatIndex);
        //}

        // TODO: Gili or Or, this is the function that needs to be called when updating the cards of the player
        public void updatePlayerCards()
        {
            Dictionary<int, List<Card>> cards = CommClient.GetPlayerCards(this.game.gameId, systemUserId);
            List<Card> c;
            if (cards != null)
                for (int i = 0; i < playerCards.Length; i++)
                {
                    if(cards.TryGetValue(i, out c) && c.Count == 2)
                    {
                        playerCards[i][0].Source = DrawCard(c[0].Type, c[0].Value);
                        playerCards[i][1].Source = DrawCard(c[1].Type, c[1].Value);
                    }
                    else
                    {
                        playerCards[i][0].Source = DrawCard(cardType.unknown, CARD_TYPE);
                        playerCards[i][1].Source = DrawCard(cardType.unknown, CARD_TYPE);
                    }
                }

            //for (int i = 0; i < cards.Length; i++)
            //{
            //    if (cards[i] == null)
            //        playerCards[playerSeatIndex][i].Source = DrawCard(cards[i].Type, cards[i].Value);
            //    else
            //        playerCards[playerSeatIndex][i].Source = DrawCard(cardType.unknown, CARD_TYPE);
            //}
        }

        //public void cardsShowOff()
        //{
        //    IDictionary<int, Card[]> seatIndexToCards = CommClient.GetShowOff(this.game.gameId);
        //    foreach (KeyValuePair<int, Card[]> entry in seatIndexToCards)
        //    {
        //        for (int i = 0; i < entry.Value.Length; i++)
        //            playerCards[entry.Key][i].Source = DrawCard(entry.Value[i].Type, entry.Value[i].Value);
        //    }
        //}

        public void updateGame(TexasHoldemGame updatedGame)
        {
            this.game = updatedGame;
            for (int i = 0; i < game.players.Length; i++)
                if (game.players[i] != null && game.players[i].systemUserID == systemUserId)
                    playerSeatIndex = i;

            updatePlayerCards();
            //int pot;
            coinsSumInHeap[0].Content = game.pot;

            //List<Card> flop;
            for (int i = 0; i < 3; i++)
            {
                if (game.flop == null || game.flop[i] == null)
                    communityCards[i].Source = DrawCard(cardType.unknown, CARD_TYPE);
                else
                    communityCards[i].Source = DrawCard(game.flop[i].Type, game.flop[i].Value);
            }
            //Card turn;
            if (game.turn == null)
                communityCards[3].Source = DrawCard(cardType.unknown, CARD_TYPE);
            else
                communityCards[3].Source = DrawCard(game.turn.Type, game.turn.Value);
            //Card river;
            if (game.river == null)
                communityCards[4].Source = DrawCard(cardType.unknown, CARD_TYPE);
            else
                communityCards[4].Source = DrawCard(game.river.Type, game.river.Value);

            //Player[] players;
            //int currentDealer;
            //int currentBig;
            //int currentSmall;
            for (int i = 0; i < game.players.Length; i++)
            {
                if (playerSeatIndex == -1 && game.players[i] == null)
                    changeSeat(i, true, "gray", null, "free seat", 0, 0);
                else if (playerSeatIndex != -1 && game.players[i] == null)
                    changeSeat(i, false, "gray", null, "free seat", 0, 0);
                else //if (game.players[i] != null)
                {
                    switch (game.players[i].playerState)
                    {
                        case Player.PlayerState.folded:
                            changeSeat(i, false, "red", game.players[i].profilePic, game.players[i].name, game.players[i].Tokens, game.players[i].TokensInBet);
                            break;
                        case Player.PlayerState.in_round:
                            changeSeat(i, false, "green", game.players[i].profilePic, game.players[i].name, game.players[i].Tokens, game.players[i].TokensInBet);
                            break;
                        case Player.PlayerState.not_in_round:
                            changeSeat(i, false, "gray", game.players[i].profilePic, game.players[i].name, game.players[i].Tokens, game.players[i].TokensInBet);
                            break;
                        case Player.PlayerState.my_turn:
                            changeSeat(i, false, "blue", game.players[i].profilePic, game.players[i].name, game.players[i].Tokens, game.players[i].TokensInBet);
                            playButton.IsEnabled = false;
                            if (systemUserId == game.players[i].systemUserID)
                            {
                                betButton.IsEnabled = true;
                                foldButton.IsEnabled = true;
                                MessageBox.Show("minimalBet: " + getMinimumBet());
                                if (getMinimumBet() == 0)
                                {
                                    checkButton.IsEnabled = true;
                                    callButton.IsEnabled = false;
                                }
                                else
                                {
                                    checkButton.IsEnabled = false;
                                    callButton.IsEnabled = true;
                                }
                            }
                            else
                            {
                                betButton.IsEnabled = false;
                                foldButton.IsEnabled = false;
                                checkButton.IsEnabled = false;
                                callButton.IsEnabled = false;
                            }
                            break;
                        case Player.PlayerState.winner:
                            changeSeat(i, false, "yellow", game.players[i].profilePic, game.players[i].name, game.players[i].Tokens, game.players[i].TokensInBet);
                            playButton.IsEnabled = true;
                            betButton.IsEnabled = false;
                            foldButton.IsEnabled = false;
                            checkButton.IsEnabled = false;
                            callButton.IsEnabled = false;
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



        public void updateChatBox(string appendedLine)
        {
            messagesTextBlock.Text += appendedLine + "\n";
            this.sv.ScrollToBottom();
        }

        public void update(object obj)
        {
            if (obj.GetType() == typeof(TexasHoldemGame))
            {
                this.Dispatcher.Invoke(() =>
                {
                    updateGame((TexasHoldemGame)obj);
                });
            }
            else if (obj.GetType() == typeof(string))
            {
                this.Dispatcher.Invoke(() =>
                {
                    updateChatBox(((string)obj));
                });
            }
        }
    }
}
