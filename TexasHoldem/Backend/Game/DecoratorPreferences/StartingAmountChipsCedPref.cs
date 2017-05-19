﻿using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class StartingAmountChipsCedPref : OptionalPreferences
    {
        private int startingChipsPolicy;

        public StartingAmountChipsCedPref(int startingChipsPolicy, OptionalPreferences nextDecPref) : base(nextDecPref)
        {
            this.startingChipsPolicy = startingChipsPolicy;
        }

        public override ReturnMessage canPerformUserActions(TexasHoldemGame game, Player p, SystemUser user, string action)
        {
            if (nextDecPref != null)
                return nextDecPref.canPerformUserActions(game, p, user, action);
            return new ReturnMessage(true,"");
        }

        public override ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, int amount, string action)
        {
            if (nextDecPref != null)
                return nextDecPref.canPerformGameActions(game, user, amount, action);
            return new ReturnMessage(true, "");
        }

        public override bool isContain(DecoratorPreferencesInterface pref)
        {
            if (pref.GetType() != typeof(OptionalPreferences))
                return false;
            OptionalPreferences opPref = ((OptionalPreferences)pref);
            StartingAmountChipsCedPref matchingPref = (StartingAmountChipsCedPref)getMatchingOptionalPref(opPref);
            //if we found matchig optinal pref and he have the same policy
            if (matchingPref != null && matchingPref.startingChipsPolicy== startingChipsPolicy)
                //if we still need to check the rest of the chain
                if (nextDecPref != null)
                    //return its result
                    return nextDecPref.isContain(pref);
                //if we don't have anything else to check return true.
                else return true;
            //if we couldent found or if we found pref with different value.
            else
                return false;
        }
    }
}