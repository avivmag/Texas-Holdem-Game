using System.Collections.Generic;
using System;

namespace Backend.Game
{
    public class LeagueTexasHoldemGame : TexasHoldemGame
    {
        //public int MinRank { get; }
        //public int MaxRank { get; }

        //public LeagueTexasHoldemGame(int gameCreatorUserId, GamePreferences gamePreferences, League league) :
        //		base(gameCreatorUserId, gamePreferences)
        //{
        //          this.MinRank = league.minRank;
        //	this.MaxRank = league.maxRank;
        //}

        //      public override ReturnMessage joinGame(Player p)
        //      {
        //          if (AvailableSeats == 0)
        //              return new ReturnMessage(false, "There are no available seats.");
        //          if (p.userRank < MinRank || p.userRank > MaxRank)
        //              return new ReturnMessage(false, "The rank of the user is not standing in the league game policy.");
        //          foreach (Player player in players)
        //              if (player != null)
        //              {
        //                  if (player.systemUserID == p.systemUserID)
        //                      return new ReturnMessage(false, "The player is already taking part in the wanted game.");
        //              }
        //          if (spectators != null)
        //          {
        //              foreach (Player spec in spectators)
        //                  if (spec.systemUserID == p.systemUserID)
        //                      return new ReturnMessage(false, "Couldn't join the game because the user is already spectating the game.");
        //          }
        //          //players.Add(p);
        //          return new ReturnMessage(true,"");
        //      }
        public LeagueTexasHoldemGame(int gameCreatorUserId, GamePreferences gamePreferences) : base(gameCreatorUserId, gamePreferences)
        {
        }
    }


}
