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
using Backend.Game;
using SL;

namespace PL
{

    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private SLInterface sl = LoginWindow.sl;
        private Window mainMenuWindow;
        private TexasHoldemGame game;

        public GameWindow(Window mainMenuWindow, TexasHoldemGame game)
        {
            this.mainMenuWindow = mainMenuWindow;
            this.game = game;
        }
    }
}
