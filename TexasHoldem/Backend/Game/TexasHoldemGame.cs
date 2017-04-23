using System.Collections.Generic;
using Backend.User;
using System;

namespace Backend.Game
{
    public class TexasHoldemGame : Messages.Notification
    {
        private int availableSeats;
        public int currentDealer { get; set; }
        public int currentBig { get; set; }
        public int currentSmall { get; set; }
        public GamePreferences GamePreferences { get; }
        private Deck deck;
        public Player[] players { get; set; }
        public List<Spectator> spectators;
        private int gameCreatorUserId;
        public int id { get; set; }
        public int pot { get; set; }
        public int tempPot { get; set; }
        public int currentBet { get; set; }
        public bool active { get; set; }
        public int currentBlindBet { get; set; }
        public enum BetAction { fold, bet, call, check, raise }
        public List<Card> flop { get; set; }
        public Card turn { get; set; }
        public Card river { get; set; }
        public enum HandsRanks { HighCard, Pair, TwoPairs, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush }


        public TexasHoldemGame(int gameCreatorUserId, GamePreferences gamePreferences)
        {
            this.gameCreatorUserId = gameCreatorUserId;
            this.GamePreferences = gamePreferences;
            pot = 0;
            active = true;
            deck = new Deck();
            spectators = new List<Spectator>();
            players = new Player[GamePreferences.MaxPlayers];
            availableSeats = GamePreferences.MaxPlayers - 1;
            players[0] = new Player(gameCreatorUserId, gamePreferences.StartingChipsAmount, 0);
            for (int i = 1; i < GamePreferences.MaxPlayers; i++)
            {
                players[i] = null;
            }

            flop = new List<Card>();

            currentDealer = 0;
        }

        public virtual ReturnMessage joinGame(Player p)
        {
            if (AvailableSeats == 0)
                return new ReturnMessage(false, "There are no available seats.");

            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].systemUserID == p.systemUserID)
                    return new ReturnMessage(false, "The player is already taking part in the wanted game.");
            }

            if (spectators != null)
            {
                foreach (Spectator spec in spectators)
                    if (spec.systemUserID == p.systemUserID)
                        return new ReturnMessage(false, "Couldn't join the game because the user is already spectating the game.");
            }
            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] == null)
                {
                    players[i] = p;
                    break;
                }
            }
            return new ReturnMessage(true, "");
        }

        public int AvailableSeats
        {
            get
            {
                int ans = 0;
                for (int i = 0; i < GamePreferences.MaxPlayers; i++)
                {
                    if (players[i] == null)
                        ans++;
                }
                return ans;
            }
        }

        public ReturnMessage joinSpectate(Spectator s)
        {
            if (!GamePreferences.IsSpectatingAllowed)
                return new ReturnMessage(false, "Couldn't spectate the game because the game preferences is not alowing.");

            foreach (Player p in players)
                if (p != null)
                {
                    if (p.systemUserID == s.systemUserID)
                        return new ReturnMessage(false, "Couldn't spectate the game because the user is already playing the game.");
                }
            foreach (Spectator spec in spectators)
                if (spec.systemUserID == s.systemUserID)
                    return new ReturnMessage(false, "Couldn't spectate the game because the user is already spectating the game.");
            
            spectators.Add(s);
            return new ReturnMessage(true,"");
        }

        public void leaveGame(Player p)
        {
            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].id == p.id)
                {
                    players[i] = null;
                    break;
                }
            }
        }

        public void leaveGame(Spectator spec)
        {
            spectators.Remove(spec);
        }

        private void playGame()
        {
            deck.Shuffle();
            dealCards();
            currentSmall = setSmallBlind();
            currentBig = setBigBlind();
            betBlinds();

            //players sets their bets
            for (int i = nextToSeat(currentBig); i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                {
                    chooseBetAction(players[i], BetAction.check, 0);
                }
            }

            pot = tempPot;

            //flop
            for (int i = 0; i < 3; i++)
            {
                flop.Add(deck.Top());
            }

            //players sets their bets
            for (int i = nextToSeat(currentDealer); i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                {
                    chooseBetAction(players[i], BetAction.check, 0);
                }
            }

            pot = tempPot;

            turn = deck.Top();

            //players sets their bets
            for (int i = nextToSeat(currentDealer); i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                {
                    chooseBetAction(players[i], BetAction.check, 0);
                }
            }

            pot = tempPot;

            river = deck.Top();

            //players sets their bets
            for (int i = nextToSeat(currentDealer); i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                {
                    chooseBetAction(players[i], BetAction.check, 0);
                }
            }

            pot = tempPot;


        }

        public void setInitialState()
        {
            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null)
                    players[i].playerState = Player.PlayerState.in_round;
            }
        }


        public void dealCards()
        {
            int index = nextToSeat(currentDealer);

            //deal first card
            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[index] != null)
                {
                    players[index].playerCards.Add(deck.Top());
                }
                index = (index + 1) % GamePreferences.MaxPlayers;
            }

            index = nextToSeat(currentDealer);
            //deal second card
            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[index] != null)
                {
                    players[index].playerCards.Add(deck.Top());
                }
                index = (index + 1) % GamePreferences.MaxPlayers;
            }
        }

        public int setSmallBlind()
        {
            int i = currentDealer;
            int j = (currentDealer + 1) % GamePreferences.MaxPlayers;
            while (i != j)
            {
                if (players[j] != null && players[j].playerState == Player.PlayerState.in_round)
                    return j;
                j = (j + 1) % GamePreferences.MaxPlayers;
            }
            return -1;
        }

        public int setBigBlind()
        {
            int i = currentDealer;
            int j = (currentDealer + 1) % GamePreferences.MaxPlayers;
            while (i != j)
            {
                if (players[j] != null && players[j].playerState == Player.PlayerState.in_round)
                    break;
                j = (j + 1) % GamePreferences.MaxPlayers;
            }
            j = (j + 1) % GamePreferences.MaxPlayers;
            while (i != j)
            {
                if (players[j] != null && players[j].playerState == Player.PlayerState.in_round)
                    return j;
                j = (j + 1) % GamePreferences.MaxPlayers;
            }
            return -1;
        }

        public void betBlinds()
        {
            players[currentSmall].Tokens -= currentBlindBet / 2;
            players[currentBig].Tokens -= currentBlindBet;
            tempPot += (currentBlindBet + (currentBlindBet / 2));
        }

        public void bet(Player p, int amount)
        {
            p.Tokens -= amount;
            tempPot += amount;
            currentBet = amount;
        }

        public void call(Player p)
        {
            p.Tokens -= currentBet;
            tempPot += currentBet;
        }

        public void fold(Player p)
        {
            p.playerState = Player.PlayerState.folded;
        }

        public void check(Player p)
        {
        }

        public void raise(Player p, int amount)
        {
            p.Tokens -= (amount + currentBet);
            tempPot += (amount + currentBet);
        }

        public void chooseBetAction(Player p, BetAction betAction, int amount)
        {
            switch (betAction)
            {
                case BetAction.bet:
                    bet(p, amount);
                    break;
                case BetAction.call:
                    call(p);
                    break;
                case BetAction.check:
                    check(p);
                    break;
                case BetAction.fold:
                    fold(p);
                    break;
                case BetAction.raise:
                    raise(p, amount);
                    break;
            }
        }

        public int nextToSeat(int seat)
        {
            int i = seat;
            int j = (i + 1) % GamePreferences.MaxPlayers;
            while (i != j)
            {
                if (players[j] != null && players[j].playerState == Player.PlayerState.in_round)
                    return j;
                j = (j + 1) % GamePreferences.MaxPlayers;
            }
            return -1;
        }


        public HandsRanks checkHandRank(Player p)
        {
            List<Card> fullHand = new List<Card>();
            for (int i = 0; i < 3; i++)
            {
                fullHand.Add(flop[0]);
                flop.RemoveAt(0);
            }

            for (int i = 0; i < 2; i++)
            {
                fullHand.Add(p.playerCards[0]);
                flop.RemoveAt(0);
            }

            fullHand.Add(turn);
            fullHand.Add(river);

            fullHand.Sort();

            for (int i = 0; i < fullHand.Count; i++)
            {
                Console.Out.Write(fullHand[i].Value + "  ");
                Console.Out.Write(fullHand[i].Type.ToString() + "  ");
                Console.Out.WriteLine();
            }


            return HandsRanks.Flush;
        }


    }
}






















//for (int i = nextToBigBlind(); i<GamePreferences.MaxPlayers; i++)        //next to big
//            {
//                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
//                {
//                    int amount = 0;
//String betAction;
//Console.Out.WriteLine("Player " + players[i].id + "choose what to do:");
//                    betAction = Console.In.ReadLine();


//                    switch (betAction)
//                    {
//                        case "bet":
//                            Console.Out.WriteLine("Choose amount: ");
//                            amount = Convert.ToInt32(Console.In.ReadLine());
//                            chooseBetAction(players[i], BetAction.bet, amount);
//                            break;
//                        case "call":
//                            chooseBetAction(players[i], BetAction.call, amount);
//                            break;
//                        case "check":
//                            chooseBetAction(players[i], BetAction.check, amount);
//                            break;
//                        case "fold":
//                            chooseBetAction(players[i], BetAction.fold, amount);
//                            break;
//                        case "raise":
//                            Console.Out.WriteLine("Choose amount: ");
//                            amount = Convert.ToInt32(Console.In.ReadLine());
//                            chooseBetAction(players[i], BetAction.raise, amount);
//                            break;
//                    }
//                }
//            }
