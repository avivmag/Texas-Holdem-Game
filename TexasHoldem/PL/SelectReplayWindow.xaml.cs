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
    /// Interaction logic for SelectReplayWindow.xaml
    /// </summary>
    public partial class SelectReplayWindow : Window
    {
        private Window mainMenuWindow;
        private List<GameLog> gameLogs = new List<GameLog>();

        public SelectReplayWindow(Window mainMenuWindow)
        {
            InitializeComponent();
            this.mainMenuWindow = mainMenuWindow;
            var gameLogStrings  = CommClient.getGameLogs();

            if (gameLogStrings == null)
            {
                MessageBox.Show("No game replays found.");
            }

            else
            {
                foreach (string[] logData in gameLogStrings)
                {
                    // Create a new game log instance.
                    var gl = new GameLog(logData);

                    // Add game log instance to game logs list.
                    gameLogs.Add(gl);

                    // Pour game log meta data into view grid list.
                    selectGameGrid.Items.Add(gl.getMetaData());
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
            DataGridCellInfo cellValue  = (selectGameGrid.SelectedCells.ElementAt(0));
            var logFileIndex            = Int32.Parse(((GameLog.GameLogMetaData)cellValue.Item).row);

            Close();
            mainMenuWindow.Show();

            new ReplayWindow(gameLogs[logFileIndex]).Show();
        }

        private void selectGameReplayGrid_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (selectGameGrid.SelectedIndex != -1)
            {
                actionBtn.IsEnabled = true;
            }
        }
    }
}
