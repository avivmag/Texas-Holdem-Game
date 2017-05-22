using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Game
{
    public class GameObserver
    {
        public enum ObserverType { PlayersChat, SpectateChat, GameStates }
        private ObserverType ot;
        private List<Player> playersList;

        public GameObserver(ObserverType type)
        {
            ot = type;
        }

        public void Subscribe(Player p)
        {
            playersList.Add(p);
        }

        public void Unsubscribe(Player p)
        {
            playersList.Remove(p);
        }

        public void Update()
        {
            //UPDATE COMMUNICATION!!!!!!!!!!
        }
    }
}
