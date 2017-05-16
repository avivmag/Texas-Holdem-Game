using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    //Current ordrer of decorator: gamePolicy->buyInPolicy->startingChipsAmount->minBet->minPlayers->maxPlayers->spectateAloowed->league
    public interface DecoratorPreferencesInterface
    {
        ReturnMessage canPerformUserActions(Player p, SystemUser user, string action);
        ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, string action);
        bool isContain(MustPreferences pref);
    }
}
