using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Game
{
    public class GameObserver
    {
        private List<Player> playersSubscribed;
        public enum ObserverType { Player, Spactate, Game }
        private ObserverType observerType;

        public GameObserver(ObserverType ot)
        {
            playersSubscribed = new List<Player>();
            observerType = ot;
        }

        public void subscribe(Player p)
        {
            playersSubscribed.Add(p);
        }

        public void unsubscribe(Player p)
        {
            playersSubscribed.Remove(p);
        }

        public void update(string s)
        {
            foreach (Player p in playersSubscribed)
            {
                p.ShowUpdate(observerType, s);
            }
        }

    }
}