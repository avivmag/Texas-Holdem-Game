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
        public int gameId { get; set; }
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
            Random rnd = new Random();
            this.gameId = rnd.Next(0, 999999);
            GameLog.setLog(gameId, DateTime.Now);
            GameLog.logLine(gameId, GameLog.Actions.Game_Start, DateTime.Now.ToString());
            var m = joinGame(new Player(gameCreatorUserId, gamePreferences.StartingChipsAmount, 0));
            for (int i = 1; i < GamePreferences.MaxPlayers; i++)
            {
                players[i] = null;
            }

            flop = new List<Card>();

            currentDealer = 0;
        }

        public virtual Message joinGame(Player p)
        {
            if (AvailableSeats == 0)
                return new Message(false, "There are no available seats.");

            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].systemUserID == p.systemUserID)
                    return new Message(false, "The player is already taking part in the wanted game.");
            }

            if (spectators != null)
            {
                foreach (Spectator spec in spectators)
                    if (spec.systemUserID == p.systemUserID)
                        return new Message(false, "Couldn't join the game because the user is already spectating the game.");
            }
            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] == null)
                {
                    players[i] = p;
                    GameLog.logLine(gameId, GameLog.Actions.Player_Join, p.id.ToString());
                    break;
                }
            }
            return new Message(true, "");
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

        public Message joinSpectate(Spectator s)
        {
            if (!GamePreferences.IsSpectatingAllowed)
                return new Message(false, "Couldn't spectate the game because the game preferences is not alowing.");

            foreach (Player p in players)
                if (p != null)
                {
                    if (p.systemUserID == s.systemUserID)
                        return new Message(false, "Couldn't spectate the game because the user is already playing the game.");
                }
            foreach (Spectator spec in spectators)
                if (spec.systemUserID == s.systemUserID)
                    return new Message(false, "Couldn't spectate the game because the user is already spectating the game.");
            
            spectators.Add(s);
            GameLog.logLine(gameId, GameLog.Actions.Spectate_Join, s.systemUserID.ToString());
            return new Message(true,"");
        }

        public void leaveGame(Player p)
        {
            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].id == p.id)
                {
                    players[i] = null;
                    GameLog.logLine(gameId, GameLog.Actions.Player_Left, p.id.ToString());
                    break;
                }
            }
        }

        public void leaveGame(Spectator spec)
        {
            GameLog.logLine(gameId, GameLog.Actions.Spectate_Left, spec.systemUserID.ToString());
            spectators.Remove(spec);
        }

        private void playGame()
        {
            GameLog.logLine(gameId, GameLog.Actions.Game_Start, DateTime.Now.ToString());
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
                    BetAction action = BetAction.check;
                    chooseBetAction(players[i], action, 0);
                }
            }

            addToPot(tempPot);

            //flop
            for (int i = 0; i < 3; i++)
            {
                Card flopCard = deck.Top();
                flop.Add(flopCard);
                GameLog.logLine(gameId, GameLog.Actions.Flop, i.ToString(), flopCard.ToString());
            }

            //players sets their bets
            for (int i = nextToSeat(currentDealer); i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                {
                    chooseBetAction(players[i], BetAction.check, 0);
                }
            }

            addToPot(tempPot);

            turn = deck.Top();
            GameLog.logLine(
                gameId,
                GameLog.Actions.Turn,
                turn.ToString());

            //players sets their bets
            for (int i = nextToSeat(currentDealer); i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                {
                    chooseBetAction(players[i], BetAction.check, 0);
                }
            }

            addToPot(tempPot);

            river = deck.Top();
            GameLog.logLine(
                gameId,
                GameLog.Actions.River,
                turn.ToString());

            //players sets their bets
            for (int i = nextToSeat(currentDealer); i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                {
                    chooseBetAction(players[i], BetAction.check, 0);
                }
            }

            addToPot(tempPot);
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
                    Card newCard = deck.Top();
                    players[index].playerCards.Add(newCard);
                    GameLog.logLine(gameId, GameLog.Actions.Deal_Card, newCard.ToString());
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
                {
                    GameLog.logLine(gameId, GameLog.Actions.Small_Blind, players[j].id.ToString());
                    return j;
                }
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
                {
                    GameLog.logLine(gameId, GameLog.Actions.Big_Blind, players[j].id.ToString());
                    return j;
                }
                j = (j + 1) % GamePreferences.MaxPlayers;
            }
            return -1;
        }

        public void betBlinds()
        {
            players[currentSmall].Tokens -= currentBlindBet / 2;
            GameLog.logLine(
                gameId, 
                GameLog.Actions.Action_Bet, 
                players[currentSmall].id.ToString(),
                (currentBlindBet / 2).ToString());

            players[currentBig].Tokens -= currentBlindBet;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Bet,
                players[currentBig].id.ToString(),
                (currentBlindBet / 2).ToString());

            tempPot += ((currentBlindBet + (currentBlindBet / 2)));
        }

        public void bet(Player p, int amount)
        {
            p.Tokens -= amount;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Bet,
                p.id.ToString(),
                amount.ToString());

            tempPot += amount;
            currentBet = amount;
        }

        public void call(Player p)
        {
            p.Tokens -= currentBet;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Bet,
                p.id.ToString(),
                currentBet.ToString());
            tempPot += currentBet;
        }

        public void fold(Player p)
        {
            p.playerState = Player.PlayerState.folded;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Fold,
                p.id.ToString());
        }

        public void check(Player p)
        {
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Check,
                p.id.ToString());
        }

        public void raise(Player p, int amount)
        {
            p.Tokens -= (amount + currentBet);
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Raise,
                p.id.ToString(),
                amount.ToString());
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

        private void addToPot(int sum)
        {
            pot += sum;
            GameLog.logLine(gameId, GameLog.Actions.Pot_Changed, sum.ToString(), pot.ToString());
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
