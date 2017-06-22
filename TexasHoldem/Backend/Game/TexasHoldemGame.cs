using System.Collections.Generic;
using Backend.User;
using System;
using static Backend.Game.Player;
using Backend.Game.DecoratorPreferences;

namespace Backend.Game
{
    public class TexasHoldemGame : Messages.Notification
    {
        public enum HandsRanks { HighCard = 8, Pair = 7, TwoPairs = 6, ThreeOfAKind = 5, Straight, Flush = 4, FullHouse = 3, FourOfAKind = 2, StraightFlush = 1, RoyalFlush = 0 }
        public enum BetAction { fold, bet, call, check, raise }
        public enum GameState { bFlop = 0, bTurn = 1, bRiver = 2, aRiver = 3, empty = 4}
        private GameState gameState;
        private Action<int[]> rankMoneyUpdateCallback;
        private Action<int[]> leaderBoardUpdateCallback;

        private bool isGameIsOver;

        public int gameId { get; set; }
        public int currentDealer { get; set; }
        public int currentBig { get; set; }
        public int currentSmall { get; set; }
        public int currentBlindBet { get; set; }
        public int pot { get; set; }
        public int currentBet { get; set; }
        private int gameCreatorUserId;
        private int availableSeats;

        //public GamePreferences GamePreferences { get; }
        public MustPreferences gamePreferences { get; set; }
        public Deck deck { get; }
        private int maxPlayers = 9;
        public Player[] players { get; set; }
        public List<SystemUser> spectators;

        public bool active { get; set; }
        private bool firstJoin;

        public List<Card> flop { get; set; }
        public Card turn { get; set; }
        public Card river { get; set; }

        public GameObserver gameStatesObserver { get; set; }
        public GameObserver spectateObserver;

        private LeaderboardsStats[] playersStats;

        private int minNumberOfPlayerRounds;

        // TODO: Gili - notice Gil decorator pattern and Aviv player.TokensInBet - you should use them in your logic
        static int currentId;
        static int getNextId()
        {
            return ++currentId;
        }
        
        public TexasHoldemGame(SystemUser user, MustPreferences gamePreferences, Action<int[]> rankMoneyUpdateCallback, Action<int[]>  leaderBoardUpdateCallback)
        {
            firstJoin = true;
            gameCreatorUserId = user.id;
            this.gamePreferences = gamePreferences;
            pot = 0;
            active = true;
            deck = new Deck();
            spectators = new List<SystemUser>();
            gameStatesObserver = new GameObserver();
            spectateObserver = new GameObserver();
            //setting the players array according to the max players pref if entered else 9 players is the max.
            maxPlayers = 9;
            MaxPlayersDecPref maxPlayersDec = (MaxPlayersDecPref)gamePreferences.getOptionalPref(new MaxPlayersDecPref(0, null));
            if (maxPlayersDec != null)
                maxPlayers = maxPlayersDec.maxPlayers;
            players = new Player[maxPlayers];
            playersStats = new LeaderboardsStats[maxPlayers];
            availableSeats = maxPlayers - 1;
            this.rankMoneyUpdateCallback = rankMoneyUpdateCallback;
            flop = null;
            currentBlindBet = 20;
            
            this.gameId = TexasHoldemGame.getNextId();
            GameLog.setLog(gameId, DateTime.Now);
            GameLog.logLine(gameId, GameLog.Actions.Game_Start, DateTime.Now.ToString());
            for (int i = 1; i < maxPlayers; i++)
            {
                players[i] = null;
            }

        }

        public ReturnMessage removeUser(int userId)
        {

            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].systemUserID == userId)
                {
                    //get the game policy if exist to the rank update
                    int buyIn = 0;
                    BuyInPolicyDecPref buyInPref = (BuyInPolicyDecPref)gamePreferences.getOptionalPref(new BuyInPolicyDecPref(0, null));
                    if (buyInPref != null)
                        buyIn = buyInPref.buyInPolicy;
                    // updates the money and rank of the user.
                    //user.money += players[i].Tokens;

                    //user.updateRank(players[i].Tokens - buyIn);
                    rankMoneyUpdateCallback(new int[] { userId, players[i].Tokens - buyIn, players[i].Tokens });
                    players[i] = null;
                    gameStatesObserver.Update(this);
                    spectateObserver.Update(this);
                    return new ReturnMessage(true, "");
                }
            }
            foreach (SystemUser u in spectators)
            {
                if (u.id == userId)
                {
                    spectators.Remove(u);
                    u.spectatingGame.Remove(this);
                    return new ReturnMessage(true, "");
                }
            }
            return new ReturnMessage(true, "");
            // Aviv - I've changed it to true because there can be players who are standing
            //return new ReturnMessage(false, "");
        }

        public ReturnMessage getGameForPlayer(SystemUser user)
        {
            ReturnMessage m = gamePreferences.canPerformUserActions(this, user, "join");
            if (!m.success)
                return m;

            //check that the player is not already in the game
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].systemUserID == user.id)
                    return new ReturnMessage(false, "The player is already taking part in the wanted game.");
            }

            //check that the player is not spectating
            foreach (SystemUser u in spectators)
                if (u.id == user.id)
                    return new ReturnMessage(false, "Couldn't join the game because the user is already spectating the game.");

            return new ReturnMessage(true, "");
        }
 
        public ReturnMessage joinGame(SystemUser user, int seatIndex)
        {
            ReturnMessage m = gamePreferences.canPerformUserActions(this, user, "join");
            if (!m.success)
                return m;

            if(seatIndex > players.Length)
                return new ReturnMessage(false, "cannot seat here");
            if (players[seatIndex] != null)
                return new ReturnMessage(false, "seat is taken");

            //getting the buy in policy if exists to pay for the chips else getting 1000 for free.
            int startingChips = 1000;
            BuyInPolicyDecPref buyInPref = (BuyInPolicyDecPref)gamePreferences.getOptionalPref(new BuyInPolicyDecPref(0, null));
            if (buyInPref != null)
            {
                startingChips = buyInPref.buyInPolicy;
            }
            if (buyInPref != null)
                user.money -= buyInPref.buyInPolicy;

            Player p = new Player(user.id, user.name, startingChips, user.rank);
            players[seatIndex] = p;

            playersStats[seatIndex] = new LeaderboardsStats();

            if (firstJoin)
            {
                currentDealer = seatIndex;
                firstJoin = false;
            }

            GameLog.logLine(gameId, GameLog.Actions.Player_Join, user.id.ToString());

            spectateObserver.Update(this);
            gameStatesObserver.Update(this);
            return new ReturnMessage(true, "");
        }

        public void addMessage(SystemUser user, string message)
        {
            message = String.Format("{0}: {1}", user.name, message);

            var isSpectator = false;
            foreach(var spectator in spectators)
            {
                Console.WriteLine("Checking if spectator {0} equals {1}", spectator.id, user.id);
                if (spectator.id == user.id)
                {
                    isSpectator = true;
                }
            }

            // If message was sent by someone who is not a spectator, send to all game state observers.
            if (!isSpectator)
            {
                Console.WriteLine("Updating {0} game observers about message: {1} at gameId: {2}",
                    gameStatesObserver.Count(),
                    message,
                    gameId);

                gameStatesObserver.Update(message);
            }

            // Send to all spectators anyway.
            Console.WriteLine("Updating  {0} spectator observers about message: {1} at gameId: {2}",
                spectateObserver.Count(),
                message,
                gameId);

            spectateObserver.Update(message);
            
        }

        public ReturnMessage joinSpectate(SystemUser user)
        {
            ReturnMessage m = gamePreferences.canPerformUserActions(this, user, "spectate");
            if (!m.success)
                return m;

            //check that the user is not playing
            foreach (Player p in players)
                if (p != null && p.systemUserID == user.id)
                    return new ReturnMessage(false, "Couldn't spectate the game because the user is already playing the game.");

            //check that the user is not spectating
            foreach (SystemUser spectateUser in spectators)
                if (spectateUser.id == user.id)
                    return new ReturnMessage(false, "Couldn't spectate the game because the user is already spectating the game.");

            spectators.Add(user);
            GameLog.logLine(gameId, GameLog.Actions.Spectate_Join, user.id.ToString());
            
            return new ReturnMessage(true, "");
        }


        public void leaveGamePlayer(Player p)
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                if (players[i] != null && players[i].systemUserID == p.systemUserID)
                {
                    players[i] = null;
                    GameLog.logLine(gameId, GameLog.Actions.Player_Left, p.systemUserID.ToString());
                    break;
                }
            }
        }

        public void leaveGameSpectator(SystemUser user)
        {
            GameLog.logLine(gameId, GameLog.Actions.Spectate_Left, user.id.ToString());
            spectators.Remove(user);
        }

        private void InitializeGame()
        {
            isGameIsOver = false;
            gameState = GameState.bFlop;
            preparePlayersState();
            pot = 0;
            deck.Shuffle();
            dealCards();
            currentSmall = getNextPlayer(currentDealer);
            currentBig = getNextPlayer(currentSmall);
            minNumberOfPlayerRounds = numbersOfPlayersInRound();
            betBlinds();
            gameStatesObserver.Update(this);
            spectateObserver.Update(this);
            players[getNextPlayer(currentBig)].playerState = PlayerState.my_turn;
            //players[currentDealer].playerState = PlayerState.my_turn;
        }

        public void playGame()
        {
            GameLog.logLine(gameId, GameLog.Actions.Game_Start, DateTime.Now.ToString());
            InitializeGame();
            gameStatesObserver.Update(this);
            spectateObserver.Update(this);
        }

        private void preparePlayersState()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                {
                    players[i].playerState = Player.PlayerState.in_round;
                    players[i].TokensInBet = 0;
                    playersStats[i].numberOfGamesPlayed++;
                }
            }
        }

        private void checkWhoWins()
        {
            int highRank = -1;
            int winnerIndex = -1;

            for (int i = 0; i < players.Length - 1; i++)
            {
                if (players[i].playerState.Equals(Player.PlayerState.in_round) || players[i].playerState.Equals(Player.PlayerState.my_turn))
                    if ((int)players[i].handRank > highRank)
                    {
                        winnerIndex = i;
                        highRank = (int)players[i].handRank;
                    }
            }

            players[winnerIndex].playerState = PlayerState.winner;
            players[winnerIndex].Tokens += pot;
            playersStats[winnerIndex].totalGrossProfit += pot;
            if (playersStats[winnerIndex].highetsCashInAGame < players[winnerIndex].Tokens)
                playersStats[winnerIndex].highetsCashInAGame = players[winnerIndex].Tokens;

            for (int i = 0; i < playersStats.Length; i++)
            {
                if (players[i] != null && playersStats[i] != null)
                {
                    leaderBoardUpdateCallback(new int[] { players[i].systemUserID, playersStats[i].highetsCashInAGame, playersStats[i].totalGrossProfit });
                }
            }


            gameStatesObserver.Update(this);
            spectateObserver.Update(this);
        }

        public void playersSetsTheirBets(bool firstBets)
        {
            if (firstBets)
                for (int i = nextToSeat(currentBig); i < maxPlayers; i++)
                {
                    if (players[i] != null && (players[i].playerState == PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)))
                    {
                        players[i].playerState = PlayerState.my_turn;
                        //UPDATE everybody
                        gameStatesObserver.Update(this);
                        spectateObserver.Update(this);
                    }
                }
            else
                for (int i = nextToSeat(currentDealer); i < maxPlayers; i++)
                {
                    if (players[i] != null && (players[i].playerState == Player.PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)))
                    {
                        players[i].playerState = PlayerState.my_turn;
                        //UPDATE everybody
                        gameStatesObserver.Update(this);
                        spectateObserver.Update(this);
                    }
                }
        }

        public BetAction chooseWhatToDo(Player p)
        {
            return BetAction.check;
        }

        public void setInitialState()
        {
            for (int i = 0; i < maxPlayers; i++)
            {
                if (players[i] != null)
                    players[i].playerState = PlayerState.in_round;
            }
        }

        public void dealCards()
        {
            int index = nextToSeat(currentDealer);

            //deal first card
            for (int i = 0; i < maxPlayers; i++)
            {
                if (players[index] != null)
                {
                    if (players[index].playerCards.Count >= 2)
                    {
                        players[index].playerCards.Clear();
                    }

                    Card newCard = deck.Top();
                    players[index].playerCards.Add(newCard);
                    GameLog.logLine(gameId, GameLog.Actions.Deal_Card, newCard.ToString());
                }
                index = (index + 1) % maxPlayers;
            }

            index = nextToSeat(currentDealer);
            //deal second card
            for (int i = 0; i < maxPlayers; i++)
            {
                if (players[index] != null)
                {
                    Card newCard = deck.Top();
                    players[index].playerCards.Add(newCard);
                    GameLog.logLine(gameId, GameLog.Actions.Deal_Card, newCard.ToString());
                }
                index = (index + 1) % maxPlayers;
            }
            flop = null;
            river = null;
            turn = null;
        }


        public int getNextPlayer(int current)
        {
            int i = (current + 1) % maxPlayers;
            while (i != current)
            {
                if (players[i] != null && (players[i].playerState == Player.PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)))
                {
                    GameLog.logLine(gameId, GameLog.Actions.Small_Blind, players[i].systemUserID.ToString());
                    return i;
                }
                i = (i + 1) % maxPlayers;
            }
            throw new ArgumentException("cannot locate next player");
        }

        public void betBlinds()
        {
            players[currentSmall].Tokens -= currentBlindBet / 2;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Bet,
                players[currentSmall].systemUserID.ToString(),
                (currentBlindBet / 2).ToString());

            players[currentSmall].TokensInBet = currentBlindBet / 2;

            players[currentBig].Tokens -= currentBlindBet;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Bet,
                players[currentBig].systemUserID.ToString(),
                (currentBlindBet / 2).ToString());

            players[currentBig].TokensInBet = currentBlindBet;

            pot += ((currentBlindBet + (currentBlindBet / 2)));
        }

        private void continueGame()
        {
            switch (gameState)
            {
                case GameState.bFlop:
                    //reveal flop
                    flop = new List<Card>();
                    for (int i = 0; i < 3; i++)
                    {
                        Card flopCard = deck.Top();
                        flop.Add(flopCard);
                        GameLog.logLine(gameId, GameLog.Actions.Flop, i.ToString(), flopCard.ToString());
                    }
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i] != null && (players[i].playerState == PlayerState.in_round || players[i].playerState == PlayerState.my_turn))
                            players[i].TokensInBet = 0;
                    }
                    minNumberOfPlayerRounds = numbersOfPlayersInRound();
                    gameState++;
                    break;
                case GameState.bTurn:
                    turn = deck.Top();
                    GameLog.logLine(
                        gameId,
                        GameLog.Actions.Turn,
                        turn.ToString());
                    for (int i = 0; i < players.Length; i++)
                    {
                        if(players[i] != null && (players[i].playerState == PlayerState.in_round || players[i].playerState == PlayerState.my_turn))
                             players[i].TokensInBet = 0;
                    }
                    minNumberOfPlayerRounds = numbersOfPlayersInRound();
                    gameState++;
                    break;
                case GameState.bRiver:
                    river = deck.Top();
                    GameLog.logLine(
                        gameId,
                        GameLog.Actions.River,
                        turn.ToString());
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i] != null && (players[i].playerState == PlayerState.in_round || players[i].playerState == PlayerState.my_turn))
                            players[i].TokensInBet = 0;
                    }
                    minNumberOfPlayerRounds = numbersOfPlayersInRound();
                    gameState++;
                    break;
                case GameState.aRiver:
                    gameState++;
                    Player p = decideWhoWon();
                    isGameIsOver = true;
                    p.Tokens += pot;
                    Console.WriteLine("############" + pot + "############");
                    p.playerState = PlayerState.winner;
                    gameStatesObserver.Update(this);
                    spectateObserver.Update(this);
                    break;
            }
        }

        private Player decideWhoWon()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && (players[i].playerState == Player.PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)))
                {
                    players[i].handRank = checkHandRank(players[i]);
                    Console.WriteLine("player: " + players[i].systemUserID + " hand rank: " + players[i].handRank.ToString());
                }
            }

            int maxHandPlayer = 9;
            int maxHandIndex = -1;
            int maxHandPlayerCards = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && (players[i].playerState == Player.PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)) && (int)players[i].handRank <= maxHandPlayer)
                {
                    if ((int)players[i].handRank == maxHandPlayer)
                    {
                        if (players[i].handRankCards > maxHandPlayerCards)
                        {
                            maxHandPlayer = (int)players[i].handRank;
                            maxHandIndex = i;
                            maxHandPlayerCards = players[i].handRankCards;
                        }
                    }
                    else
                    {
                        maxHandPlayer = (int)players[i].handRank;
                        maxHandIndex = i;
                        maxHandPlayerCards = players[i].handRankCards;
                    }
                }
            }
            return players[maxHandIndex];
        }

        public ReturnMessage bet(Player p, int amount)
        {
            /////////////////////////////////////////////////////////////
            if (p.Tokens - amount < 0)
                return new ReturnMessage(false, "not enough coins");
            if (currentBet > amount && amount != p.Tokens)
                return new ReturnMessage(false, "need to bet more");

            p.Tokens -= amount;

            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Raise,
                p.systemUserID.ToString(),
                amount.ToString());

            pot += amount;
            int turn = checkWhosTurnIs();
            players[turn].TokensInBet += amount;
            int bet = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && (players[i].playerState == PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)) && players[i].Tokens != 0)
                {
                    bet = players[i].TokensInBet;
                    break;
                }
            }
            bool isBetOver = true;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && (players[i].playerState == PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)) && players[i].TokensInBet != bet && players[i].Tokens != 0)
                {
                    isBetOver = false;
                }
            }
           
            
            players[turn].playerState = PlayerState.in_round;
            players[nextToSeat(turn)].playerState = PlayerState.my_turn;
            
            if (isBetOver)
            {
                continueGame();
            }
            gameStatesObserver.Update(this);
            spectateObserver.Update(this);
            // TODO: Gili, you need to send the message to the other players
            return new ReturnMessage(true, "");
        }

        public ReturnMessage call(Player p, int minimumBet)
        {
            return bet(p, minimumBet);
        }

        public ReturnMessage fold(Player p)
        {
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Fold,
                p.systemUserID.ToString());

            int fold = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && (players[i].playerState == PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)) && players[i].Tokens != 0)
                {
                    fold = players[i].TokensInBet;
                    break;
                }
            }

            bool isFoldOver = true;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && (players[i].playerState == PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)) && players[i].TokensInBet != fold && players[i].Tokens != 0)
                {
                    isFoldOver = false;
                }
            }
            int turn = checkWhosTurnIs();
            players[turn].playerState = PlayerState.folded;
            int nextPlayer = nextToSeat(turn);
            players[nextPlayer].playerState = PlayerState.my_turn;
            if (numbersOfPlayersInRound() == 1 && !checkIfAnyPlayerIsWinner())
            {
                players[nextPlayer].playerState = PlayerState.winner;
                players[nextPlayer].Tokens += pot;
                gameStatesObserver.Update(this);
                spectateObserver.Update(this);
                return new ReturnMessage(true, "");
            }
            if (isFoldOver)
            {
                continueGame();
            }
            gameStatesObserver.Update(this);
            spectateObserver.Update(this);
            return new ReturnMessage(true, "");
        }

        private bool checkIfAnyPlayerIsWinner()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == PlayerState.winner)
                    return true;
            }
            return false;
        }

        public ReturnMessage check(Player p)
        {
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Check,
                p.systemUserID.ToString());
            int turn = checkWhosTurnIs();
            players[turn].playerState = PlayerState.in_round;
            players[nextToSeat(turn)].playerState = PlayerState.my_turn;
            minNumberOfPlayerRounds--;
            if (minNumberOfPlayerRounds <= 0 )
                continueGame();
            gameStatesObserver.Update(this);
            spectateObserver.Update(this);
            return new ReturnMessage(true, "");
        }
        
        private int checkWhosTurnIs()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == PlayerState.my_turn)
                    return i;
            }
            return -1;
        }

        public int nextToSeat(int seat)
        {
            int i = seat;
            int j = (i + 1) % maxPlayers;
            while (i != j)
            {
                if (players[j] != null && (players[j].playerState == Player.PlayerState.in_round || players[i].playerState.Equals(Player.PlayerState.my_turn)))
                    return j;
                j = (j + 1) % maxPlayers;
            }
            return j;
        }

        public HandsRanks checkHandRank(Player p)
        {
            List<Card> fullHand = new List<Card>();
            for (int i = 0; i < 3; i++)
            {
                fullHand.Add(flop[i]);
            }

            for (int i = 0; i < 2; i++)
            {
                fullHand.Add(p.playerCards[i]);
            }

            fullHand.Add(turn);
            fullHand.Add(river);

            p.fullHand = fullHand;

            for (int i = 0; i < fullHand.Count; i++)
            {
                Console.WriteLine("player " + p.systemUserID + " --- " + fullHand[i].ToString());
            }

            p.fullHand.Sort();

            if (checkRoyalFlush(p.fullHand))
                return HandsRanks.RoyalFlush;
            if (checkStraightFlush(p.fullHand) != -1)
                return HandsRanks.StraightFlush;
            if (checkFourOfAKind(p.fullHand) != -1)
                return HandsRanks.FourOfAKind;
            if ((p.handRankCards = checkFullHouse(p.fullHand, 3)) != -1)
                return HandsRanks.FullHouse;
            if ((p.handRankCards = checkFlush(p.fullHand)) != -1)
                return HandsRanks.Flush;
            if ((p.handRankCards = checkStraight(p.fullHand)) != -1)
                return HandsRanks.Straight;
            if ((p.handRankCards = checkThreeOfAKind(p.fullHand)) != -1)
                return HandsRanks.ThreeOfAKind;
            if ((p.handRankCards = checkTwoPairs(p.fullHand)) != -1)
                return HandsRanks.TwoPairs;
            if ((p.handRankCards = checkPair(p.fullHand)) != -1)
                return HandsRanks.Pair;

            p.handRankCards = p.fullHand[0].Value;
            return HandsRanks.HighCard;
        }
        
        private void addToPot(int sum)
        {
            pot += sum;
            GameLog.logLine(gameId, GameLog.Actions.Pot_Changed, sum.ToString(), pot.ToString());
        }

        private int checkHighRemainingCards(List<Card> fullHand)
        {
            fullHand.Sort();
            return fullHand[0].Value;
        }

        public bool checkRoyalFlush(List<Card> fullHand)
        {
            int[] counters = new int[4];
            for (int i = 0; i < 4; i++)
                counters[i] = 0;

            for (int i = 0; i < fullHand.Count; i++)
                if (fullHand[i].Value == 1 ||
                    fullHand[i].Value == 10 ||
                    fullHand[i].Value == 11 ||
                    fullHand[i].Value == 12 ||
                    fullHand[i].Value == 13)

                    counters[(int)fullHand[i].Type]++;

            for (int i = 0; i < 4; i++)
                if (counters[i] == 5)
                    return true;
            return false;
        }

        public int checkStraightFlush(List<Card> fullHand)
        {
            int highestCardValue = 0;
            int[] valueBits = new int[4];
            for (int i = 0; i < 4; i++)
                valueBits[i] = 0;

            for (int i = 0; i < fullHand.Count; i++)
                valueBits[(int)fullHand[i].Type] += (int)Math.Pow(2, fullHand[i].Value - 1);

            int bitCounter = 0;

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 13; j++)
                {
                    if (valueBits[i] % 2 == 1)
                    {
                        bitCounter++;
                        valueBits[i] = valueBits[i] / 2;
                        if (bitCounter == 5)
                        {
                            highestCardValue = j + 1;
                        }
                    }
                    else
                    {
                        bitCounter = 0;
                        valueBits[i] = valueBits[i] / 2;
                    }
                    if (bitCounter == 5)
                    {
                        return highestCardValue;
                    }
                }
            return -1;
        }

        public int checkFourOfAKind(List<Card> fullHand)
        {
            int cardValueOfFourOfAKind = 0;
            int[] fourOfAKindCounter = new int[13];
            for (int i = 0; i < 13; i++)
                fourOfAKindCounter[i] = 0;
            for (int i = 0; i < fullHand.Count; i++)
            {
                fourOfAKindCounter[fullHand[i].Value - 1]++;
                if (fourOfAKindCounter[fullHand[i].Value - 1] == 4)
                    cardValueOfFourOfAKind = fullHand[i].Value;
            }

            for (int i = 0; i < 13; i++)
            {
                if (fourOfAKindCounter[i] == 4)
                {
                    return cardValueOfFourOfAKind;
                }
            }
            return -1;
        }

        public int checkFullHouse(List<Card> fullHand, int whatToReturn)
        {
            int[] threeOfAKindCounter = new int[13];
            int[] twoOfAKindCounter = new int[13];

            bool threeOfAKind = false;
            bool twoOfAKind = false;

            int threeOfAKindCardValue = 0;
            int twoOfAKindCardValue = 0;

            for (int i = 0; i < 13; i++)
            {
                threeOfAKindCounter[i] = 0;
                twoOfAKindCounter[i] = 0;
            }

            for (int i = 0; i < fullHand.Count; i++)
            {
                threeOfAKindCounter[fullHand[i].Value - 1]++;
                twoOfAKindCounter[fullHand[i].Value - 1]++;

                if (threeOfAKindCounter[fullHand[i].Value - 1] == 3)
                    threeOfAKindCardValue = fullHand[i].Value;
                if (twoOfAKindCounter[fullHand[i].Value - 1] == 2)
                    twoOfAKindCardValue = fullHand[i].Value;
            }

            for (int i = 0; i < 13; i++)
            {
                if (threeOfAKindCounter[i] == 3)
                    threeOfAKind = true;
                if (twoOfAKindCounter[i] == 2)
                    twoOfAKind = true;
            }

            if (twoOfAKind && threeOfAKind)
                switch (whatToReturn)
                {
                    case 2:
                        return twoOfAKindCardValue;
                    case 3:
                        return threeOfAKindCardValue;
                }
            return -1;

        }

        public int checkFlush(List<Card> fullHand)
        {
            int[] shape = new int[4];

            for (int i = 0; i < 4; i++)
            {
                shape[i] = 0;
            }

            for (int i = 0; i < 4; i++)
                if (shape[i] == 5)
                    return 0;
            return -1;
        }

        public int checkStraight(List<Card> fullHand)
        {
            int valueCounter = 0;
            for (int i = 0; i < fullHand.Count; i++)
                if (fullHand[i].Value == 1 ||
                    fullHand[i].Value == 10 ||
                    fullHand[i].Value == 11 ||
                    fullHand[i].Value == 12 ||
                    fullHand[i].Value == 13)
                    valueCounter++;
            if (valueCounter == 5)
                return 1;

            int valueBits = 0;

            for (int i = 0; i < fullHand.Count; i++)
                valueBits += (int)Math.Pow(2, fullHand[i].Value - 1);

            int bitCounter = 0;
            int highestCardValue = 0;

            for (int j = 0; j < 13; j++)
            {
                if (valueBits % 2 == 1)
                {
                    bitCounter++;
                    valueBits = valueBits / 2;
                    if (bitCounter == 5)
                        highestCardValue = j + 1;
                }
                else
                {
                    bitCounter = 0;
                    valueBits = valueBits / 2;
                }
                if (bitCounter == 5)
                    return highestCardValue;
            }
            return -1;
        }

        public int checkThreeOfAKind(List<Card> fullHand)
        {
            int[] threeOfAKindCounter = new int[13];
            for (int i = 0; i < 13; i++)
                threeOfAKindCounter[i] = 0;

            for (int i = 0; i < fullHand.Count; i++)
            {
                threeOfAKindCounter[fullHand[i].Value - 1]++;
                if (threeOfAKindCounter[i] == 3)
                    return fullHand[i].Value;
            }

            return -1;
        }

        public int checkTwoPairs(List<Card> fullHand)
        {
            int[] twoOfAKindCounterA = new int[13];
            int[] twoOfAKindCounterB = new int[13];
            bool first = false, second = false;
            for (int i = 0; i < 13; i++)
            {
                twoOfAKindCounterA[i] = 0;
                twoOfAKindCounterB[i] = 0;
            }
            int firstPair = 0;
            for (int i = 0; i < fullHand.Count; i++)
            {
                twoOfAKindCounterA[fullHand[i].Value - 1]++;
                if (twoOfAKindCounterA[fullHand[i].Value - 1] == 2)
                {
                    firstPair = fullHand[i].Value;
                    first = true;
                }
            }

            for (int i = 0; i < fullHand.Count; i++)
            {
                if (fullHand[i].Value != firstPair)
                {
                    twoOfAKindCounterB[fullHand[i].Value - 1]++;
                    second = true;
                }

            }
            if (first && second)
                return firstPair;

            //for (int i = 0; i < 13; i++)
            //    if (twoOfAKindCounterA[i] == 2 && twoOfAKindCounterB[i] == 2)
            //        return 0;
            return -1;
        }

        public int checkPair(List<Card> fullHand)
        {
            int[] pairCounter = new int[13];
            for (int i = 0; i < 13; i++)
                pairCounter[i] = 0;

            for (int i = 0; i < fullHand.Count; i++)
            {
                pairCounter[fullHand[i].Value - 1]++;
                if (pairCounter[i] == 2)
                    return fullHand[i].Value;
            }
            return -1;
        }
        
        public Player GetPlayer(int playerIndex)
        {
            return players[playerIndex];
        }

        private int numbersOfPlayersInRound()
        {
            int ans = 0;
            for (int i = 0; i <players.Length; i++)
            {
                if (players[i] != null && (players[i].playerState == PlayerState.in_round || players[i].playerState == PlayerState.my_turn))
                {
                    ans++;
                }
            }
            return ans;
        }

        public Dictionary<int, List<Card>> GetPlayerCards(int userId)
        {
            Dictionary<int, List<Card>> playerCards = new Dictionary<int, List<Card>>();
            var isSpectator = false;

            // Check if user is a spectator, if so. return all cards.
            foreach (var spectator in spectators)
            {
                if (spectator.id == userId)
                {
                    isSpectator = true;
                }
            }
            // TODO: A, I don't know how you say the game is over, so just insert in this if instead false a boolean indicating the game is over.
            if (isGameIsOver || isSpectator)
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] != null && players[i].playerCards != null && players[i].playerCards.Count == 2)
                        playerCards[i] = players[i].playerCards;
                }
            else
                for (int i = 0; i < players.Length; i++)
                    if (players[i] != null && players[i].systemUserID == userId && players[i].playerCards != null && players[i].playerCards.Count == 2)
                        playerCards[i] = players[i].playerCards;
            return playerCards;
        }
    }
}