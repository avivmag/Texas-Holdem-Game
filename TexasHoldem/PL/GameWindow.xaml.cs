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

namespace PL
{

    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private int gameId;
        private TexasHoldemGame game;
        private Window mainMenuWindow;
        private Button[] seatsButtons;
        private Label[] playerNames;
        private Image[] playerStateBackground;
        private Image[] playersImage;
        private Image[][] playerCards;
        private Image[] communityCards;
        private int CARD_TYPE = 5;
        private Label[] coinsSumInHeap;
        private Image[] coinsImagesInHeap;
        private Image[] allInIcons;
        private Image[] dealerIcons;
        private Image[] bigIcons;
        private Image[] smallIcons;
        private Label[] playerCoins;
        private Label[] playerCoinsGambled;


        // for testing
        public GameWindow()
        {
            game = new TexasHoldemGame();
            game.players = new Player[9];
            InitializeComponent();
            initializeScreen();
            placePlayer(3, "profile_pic", "gil", 100);
            placePlayer(4, "profile_pic", "aviv", 100);
            removePlayer(4);
            SetDealerBigSmallIcons(0, 8, 7);
            SetDealerBigSmallIcons(1, 2, 3);
            movePlayersCoinsToHeap(0);

        }

        public GameWindow(Window mainMenuWindow, TexasHoldemGame game)
        {
            this.mainMenuWindow = mainMenuWindow;
            this.game = game;
        }

        private void initializeScreen()
        {
            seatsButtons = new Button[game.players.Length];
            playerNames = new Label[game.players.Length];
            playerStateBackground = new Image[game.players.Length];
            playersImage = new Image[game.players.Length];
            playerCards = new Image[game.players.Length][];
            currentButtonForThisTwo = new Dictionary<Button, Image>();
            allInIcons = new Image[game.players.Length];
            dealerIcons = new Image[game.players.Length];
            bigIcons = new Image[game.players.Length];
            smallIcons = new Image[game.players.Length];
            playerCoins = new Label[game.players.Length];
            playerCoinsGambled = new Label[game.players.Length];

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
            UniformGrid mainControlBar = new UniformGrid();
            UniformGrid checkFoldbetControlBar = new UniformGrid();
            UniformGrid ControlBar = new UniformGrid();

            BottomRow.Children.Add(mainControlBar);
        }

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

        private UniformGrid makePlayerUniformGrid(int i)
        {
            initializePlayerIcon(i);

            playerNames[i].Content = "free seat";
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

        private void initializePlayerIcon(int i)
        {
            currentButtonForThisTwo[seatsButtons[i]] = playerStateBackground[i];
            seatsButtons[i].Width = 90;
            seatsButtons[i].Height = 90;
            changeSeat(i, true, "free_seat", "free_seat_icon", "free_seat", 0);
        }

        private IDictionary<Button, Image> currentButtonForThisTwo;
        private void FreeSeatEventMouseEnter(object sender, EventArgs e)
        {
            currentButtonForThisTwo[(Button) sender].Source = new BitmapImage(new Uri("pack://application:,,,/resources/in_game.png"));
        }

        private void FreeSeatEventMouseLeave(object sender, EventArgs e)
        {
            currentButtonForThisTwo[(Button) sender].Source = new BitmapImage(new Uri("pack://application:,,,/resources/free_seat.png"));
        }

        private void placePlayer(int i, string playerImageUrl, string playerName, int coins)
        {
            changeSeat(i, false, "in_game", playerImageUrl, playerName, coins);
        }

        private void removePlayer(int i)
        {
            changeSeat(i, true, "free_seat", "free_seat_icon", "free seat", 0);
        }

        private void changeSeat(int i, Boolean addMouseEvents, string playerStateImageUrl, string playerImageUrl, string playerName, int playerCoinsNumber)
        {
            if(addMouseEvents)
            {
                seatsButtons[i].MouseEnter += FreeSeatEventMouseEnter;
                seatsButtons[i].MouseLeave += FreeSeatEventMouseLeave;
            }
            else
            {
                seatsButtons[i].MouseEnter -= FreeSeatEventMouseEnter;
                seatsButtons[i].MouseLeave -= FreeSeatEventMouseLeave;
            }

            playerStateBackground[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/" + playerStateImageUrl + ".png"));
            playersImage[i].Source = new BitmapImage(new Uri("pack://application:,,,/resources/" + playerImageUrl + ".png"));



            playerNames[i].Content = playerName;
            playerCoins[i].Content = playerCoinsNumber;
            playerCoinsGambled[i].Content = 0;
        }

        enum cardType { spade, heart, club, diamond, unknown };
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
            for(int i = 0; i < communityCards.Length; i++)
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
            for(int i = 0; i < game.players.Length; i++)
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
        public void raiseBet(int playerIndex, int coins)
        {
            int playerCoinsNum, playerCoinsGambledNum;
            int.TryParse(playerCoins[playerIndex].Content.ToString(), out playerCoinsNum);
            int.TryParse(playerCoinsGambled[playerIndex].Content.ToString(), out playerCoinsGambledNum);

            playerCoinsNum -= coins;
            playerCoinsGambledNum += coins;

            playerCoins[playerIndex].Content = playerCoinsNum;
            playerCoinsGambled[playerIndex].Content = playerCoinsGambledNum;

            if(playerCoinsNum <= 0)
                allInIcons[playerIndex].Visibility = Visibility.Visible;

            CommClient.raiseBet(gameId, playerIndex, coins);
        }
    }
}
