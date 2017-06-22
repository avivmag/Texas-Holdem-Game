using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    //Current ordrer of decorator: mustPref (canSpectate,League vals) ->gamePolicy->buyInPolicy->startingChipsAmount->minBet->minPlayers->maxPlayers
    public interface DecoratorPreferencesInterface
    {
        //This method will check if actions that are not related to a running game (such as join) can be performed
        ReturnMessage canPerformUserActions(TexasHoldemGame game, SystemUser user, string action);
        //This method will check if actions that related to a running game (such as bet) can be performed
        ReturnMessage canPerformGameActions(TexasHoldemGame game, int amount, string action);
        bool isContain(DecoratorPreferencesInterface pref);
    }
}
