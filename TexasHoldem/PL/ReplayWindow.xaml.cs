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
using System.Reflection;

namespace PL
{

    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class ReplayWindow : Window
    {
        private GameLog gameLog;

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
        private Label[] playerCoins;
        private Label[] playerCoinsGambled;
        private TextBlock messagesTextBlock;
        //private TextBox messagesTextBox;
        Button nextPlayButton;

        //// for testing
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

        public ReplayWindow(GameLog gameLog)
        {
            InitializeComponent();
            this.gameLog = gameLog;
            initializeScreen();
        }

        /// <summary>
        /// initialization of all arrays and stuff
        /// </summary>/
        private void initializeScreen()
        {
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
            playerCoins = new Label[9];
            playerCoinsGambled = new Label[9];
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
            nextPlayButton = new Button();
            messagesTextBlock = new TextBlock();
            //messagesTextBox = new TextBox();

            nextPlayButton.Click += nextPlayButton_Click;

            nextPlayButton.Content = "Next Play";

            commentPlayControlBarUg.Children.Add(nextPlayButton);

            Border border = new Border();

            border = new Border();
            
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

            playerCoins[i].Content = 0;
            playerCoins[i].HorizontalAlignment = HorizontalAlignment.Center;
            playerCoins[i].VerticalAlignment = VerticalAlignment.Center;
            playerCoins[i].HorizontalContentAlignment = HorizontalAlignment.Center;
            playerCoins[i].VerticalContentAlignment = VerticalAlignment.Center;
            playerCoins[i].Foreground = Brushes.White;

            playerCoinsGambled[i].Content = 0;
            playerCoinsGambled[i].HorizontalAlignment = HorizontalAlignment.Center;
            playerCoinsGambled[i].VerticalAlignment = VerticalAlignment.Center;
            playerCoinsGambled[i].HorizontalContentAlignment = HorizontalAlignment.Center;
            playerCoinsGambled[i].VerticalContentAlignment = VerticalAlignment.Center;
            playerCoinsGambled[i].Foreground = Brushes.Yellow;

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
        /// simple initialization of seat - just create it, does not take in account anything about the game state
        /// </summary>
        /// <param name="i"></param>
        private void initializeVariables(int i)
        {
            seatsButtons[i] = new Button();
            seatsButtons[i].IsEnabled = false;
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

        private void nextPlayButton_Click(object sender, RoutedEventArgs e)
        {
            var nextMoveAction = gameLog.peekNextMoveAction();
            if (nextMoveAction != null)
            {
                var method = typeof(ReplayWindow).GetMethod(nextMoveAction[0]);
                if (method != null)
                {
                    // Only if method is needed, call it. otherwise allow fail.
                    try
                    {
                        method.Invoke(this, new Object[] { nextMoveAction });
                    }
                    catch { }
                }
                var nextMove = gameLog.getNextMove();

                updateChatBox(nextMove);
            }
            else
            {
                this.nextPlayButton.IsEnabled = false;
            }
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

        public void updateChatBox(string appendedLine)
        {
            messagesTextBlock.Text += appendedLine + "\n";
            this.sv.ScrollToBottom();
        }

        public void Pot_Changed(string[] logLine)
        {
            this.coinsSumInHeap[0].Content = logLine[2];
        }

        public void River(string[] logLine)
        {
            var cardType = getCardType(logLine[1]);
            communityCards[4].Source = DrawCard(cardType, Int32.Parse(logLine[2]));
        }

        public void Turn(string[] logLine)
        {
            var cardType = getCardType(logLine[1]);
            communityCards[3].Source = DrawCard(cardType, Int32.Parse(logLine[2]));
        }

        public void Flop(string[] logLine)
        {
            var flopNum     = Int32.Parse(logLine[1]);
            var cardType    = getCardType(logLine[2]);
            communityCards[flopNum].Source = DrawCard(cardType, Int32.Parse(logLine[3]));
        }

        public void Player_Left(string[] logLine)
        {
            var playerIndex = Int32.Parse(logLine[2]);

            playerCoins[playerIndex].Content        = String.Empty;
            playerCoinsGambled[playerIndex].Content = String.Empty;
            playerNames[playerIndex].Content        = String.Empty;
            playersImage[playerIndex]               = new Image();
            playerStateBackground[playerIndex].Source = new BitmapImage(new Uri("pack://application:,,,/resources/gray.png"));
        }

        public void Player_Winner(string [] logLine)
        {
            var playerIndex = Int32.Parse(logLine[2]);
            playerStateBackground[playerIndex].Source = new BitmapImage(new Uri("pack://application:,,,/resources/yellow.png"));
        }

        public void Player_Join(string[] logLine)
        {
            var playerIndex = Int32.Parse(logLine[2]);

            playerCoins[playerIndex].Content        = logLine[3];
            playerCoinsGambled[playerIndex].Content = 0;
            playerNames[playerIndex].Content        = logLine[1];
            playersImage[playerIndex].Source        = new BitmapImage(new Uri("pack://application:,,,/resources/replay.jpg"));
        }

        public void Deal_Card(string[] logLine)
        {
            var playerIndex = Int32.Parse(logLine[5]);
            var cardIndex   = Int32.Parse(logLine[1]);

            var cardType = getCardType(logLine[2]);
            playerCards[playerIndex][cardIndex].Source = DrawCard(cardType, Int32.Parse(logLine[3]));
        }

        public void Action_Fold(string[] logLine)
        {
            var playerIndex = Int32.Parse(logLine[2]);
            playerStateBackground[playerIndex].Source = new BitmapImage(new Uri("pack://application:,,,/resources/red.png"));
        }

        public void Action_Bet(string[] logLine)
        {
            var playerIndex                         = Int32.Parse(logLine[3]);
            var gambleAmount                        = Int32.Parse(logLine[2]);
            playerCoinsGambled[playerIndex].Content = gambleAmount;
            playerCoins[playerIndex].Content        = Int32.Parse(playerCoins[playerIndex].Content.ToString()) - gambleAmount;
        }

        private cardType getCardType(string type)
        {
            switch (type)
            {
                case "diamond":
                    return cardType.diamond;
                case "club":
                    return cardType.club;
                case "heart":
                    return cardType.heart;
                case "spade":
                    return cardType.spade;
                default:
                    return cardType.unknown;
            }
        }
    }
}
