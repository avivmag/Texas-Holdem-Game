using System.Collections.Generic;
using Backend.User;
using System;
using static Backend.Game.Player;
using Backend.Game.DecoratorPreferences;
using System.Diagnostics;

namespace Backend.Game
{
    public class TexasHoldemGame : Messages.Notification
    {
        public enum HandsRanks { HighCard = 8, Pair = 7, TwoPairs = 6, ThreeOfAKind = 5, Straight, Flush = 4, FullHouse = 3, FourOfAKind = 2, StraightFlush = 1, RoyalFlush = 0 }
        public enum BetAction { fold, bet, call, check, raise }
        public enum GameState { bFlop = 0, bTurn = 1, bRiver = 2, aRiver = 3}
        private GameState gameState;

        public int gameId { get; set; }
        public int currentDealer { get; set; }
        public int currentBig { get; set; }
        public int currentSmall { get; set; }
        public int currentBlindBet { get; set; }
        public int pot { get; set; }
        public int tempPot { get; set; }
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
        private bool isGameActive;

        public List<Card> flop { get; set; }
        public Card turn { get; set; }
        public Card river { get; set; }

        public GameObserver gameStatesObserver { get; set; }
        //public GameObserver playersChatObserver;
        //public GameObserver spectateChatObserver;
        //public GameObserver gameStatesObserver;

        // TODO: Gili - notice Gil decorator pattern and Aviv player.TokensInBet - you should use them in your logic
        static int currentId;
        static int getNextId()
        {
            return ++currentId;
        }


        public TexasHoldemGame(SystemUser user, MustPreferences gamePreferences)
        {
            gameCreatorUserId = user.id;
            this.gamePreferences = gamePreferences;
            pot = 0;
            active = true;
            deck = new Deck();
            spectators = new List<SystemUser>();
            gameStatesObserver = new GameObserver();
            //setting the players array according to the max players pref if entered else 9 players is the max.
            maxPlayers = 9;
            MaxPlayersDecPref maxPlayersDec = (MaxPlayersDecPref)gamePreferences.getOptionalPref(new MaxPlayersDecPref(0, null));
            if (maxPlayersDec != null)
                maxPlayers = maxPlayersDec.maxPlayers;
            players = new Player[maxPlayers];
            availableSeats = maxPlayers - 1;


            // TODO: remove when the db is created.

            // NO MORE FUCKING RANDOM!!!
            // https://youtu.be/2JM4LZX-oxg?t=6s
            //Random rnd = new Random();
            //this.gameId = rnd.Next(0, 999999);
            this.gameId = TexasHoldemGame.getNextId();
            GameLog.setLog(gameId, DateTime.Now);
            GameLog.logLine(gameId, GameLog.Actions.Game_Start, DateTime.Now.ToString());
            //var m = joinGame(user);
            //if(!m.success)
            //    Console.WriteLine(m.description);
            for (int i = 1; i < maxPlayers; i++)
            {
                players[i] = null;
            }

            //flop = new List<Card>();
            //flop.Add(new Card(Card.cardType.club, 5));
            //flop.Add(new Card(Card.cardType.diamond, 6));
            //flop.Add(new Card(Card.cardType.heart, 7));

            isGameActive = false;

            //playersChatObserver = new GameObserver(GameObserver.ObserverType.PlayersChat);
            //spectateChatObserver = new GameObserver(GameObserver.ObserverType.SpectateChat);
            //gameStatesObserver = new GameObserver(GameObserver.ObserverType.GameStates);

            currentDealer = 0;
        }

        //public object Subscribe(object client)
        //{
        //    go.Subscribe(client);
        //}

        //public TexasHoldemGame(SystemUser user, GamePreferences gamePreferences)
        //{
        //    this.gameCreatorUserId = user.id;
        //    this.GamePreferences = gamePreferences;
        //    pot = 0;
        //    active = true;
        //    deck = new Deck();
        //    spectators = new List<SystemUser>();

        //    players = new Player[GamePreferences.MaxPlayers];
        //    availableSeats = GamePreferences.MaxPlayers - 1;

        //    // TODO: remove when the db is created.
        //    Random rnd = new Random();
        //    this.gameId = rnd.Next(0, 999999);
        //    GameLog.setLog(gameId, DateTime.Now);
        //    GameLog.logLine(gameId, GameLog.Actions.Game_Start, DateTime.Now.ToString());
        //    var m = joinGame(user);
        //    for (int i = 1; i < GamePreferences.MaxPlayers; i++)
        //    {
        //        players[i] = null;
        //    }

        //    flop = new List<Card>();

        //    currentDealer = 0;
        //}

        //public void removeUser(SystemUser user)
        //{
        //    for (int i=0; i<players.Length; i++)
        //    {
        //        if (players[i].systemUserID == user.id)
        //        {
        //            // updates the money and rank of the user.
        //            user.money += players[i].Tokens;
        //            user.updateRank(players[i].Tokens - gamePreferences.BuyInPolicy);
        //            players[i] = null;
        //            return;
        //        }
        //    }
        //    foreach(SystemUser u in spectators)
        //    {
        //        if (u.id == user.id)
        //        {
        //            spectators.Remove(u);
        //            u.spectatingGame.Remove(this);
        //        }
        //    }
        //}

        //public ReturnMessage joinGame(SystemUser user)
        //{
        //    ReturnMessage m = gamePreferences.canPerformUserActions(this, user, "join");
        //    if (!m.success)
        //        return m;

        //    //check the user money
        //    //if (user.money < gamePreferences.BuyInPolicy)
        //    //    return new ReturnMessage(false, "Could not join the game because the user dont have enough money to join.");

        //    //check if there are available seats.
        //    //if (AvailableSeats == 0)
        //    //    return new ReturnMessage(false, "There are no available seats.");


        //    Player p = new Player(user.id, gamePreferences.BuyInPolicy, user.rank);

        //    //check that the player stands in the league ranks.
        //    //if (GamePreferences.isLeague && (p.userRank < GamePreferences.MinRank || p.userRank > GamePreferences.MaxRank))
        //    //    return new ReturnMessage(false, "The rank of the user is not standing in the league game policy.");

        //    //check that the player is not already in the game
        //    for (int i = 0; i < players.Length; i++)
        //    {
        //        if (players[i] != null && players[i].systemUserID == user.id)
        //            return new ReturnMessage(false, "The player is already taking part in the wanted game.");
        //    }

        //    //check that the player is not spectating
        //    foreach (SystemUser u in spectators)
        //        if (u.id == user.id)
        //            return new ReturnMessage(false, "Couldn't join the game because the user is already spectating the game.");

        //    //seats the player in the first available seat
        //    for (int i = 0; i < players.Length; i++)
        //    {
        //        if (players[i] == null)
        //        {
        //            players[i] = p;
        //            GameLog.logLine(gameId, GameLog.Actions.Player_Join, user.id.ToString());
        //            break;
        //        }
        //    }

        //    return new ReturnMessage(true, "");
        //}

        public ReturnMessage removeUser(SystemUser user)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].systemUserID == user.id)
                {
                    //get the game policy if exist to the rank update
                    int buyIn = 0;
                    BuyInPolicyDecPref buyInPref = (BuyInPolicyDecPref)gamePreferences.getOptionalPref(new BuyInPolicyDecPref(0, null));
                    if (buyInPref != null)
                        buyIn = buyInPref.buyInPolicy;
                    // updates the money and rank of the user.
                    user.money += players[i].Tokens;
                    user.updateRank(players[i].Tokens - buyIn);
                    players[i] = null;
                    return new ReturnMessage(true, "");
                }
            }
            foreach (SystemUser u in spectators)
            {
                if (u.id == user.id)
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

        ///////////////  UNUSED ///////////////
        //private void removePlayer(SystemUser user,Player p)
        //{
        //    if (user.isNewPlayer())
        //    {
        //        if (user.tempRank - rankToChange > 0)
        //        {
        //        }
        //        user.setTempGames();
        //    }
        //    if (user.rank + rankToChange > 0)
        //    {
        //    }
        //}
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

        // TODO: Gili, I've added the seat index as we've decided. 
        public ReturnMessage joinGame(SystemUser user, int seatIndex)
        {
            ReturnMessage m = gamePreferences.canPerformUserActions(this, user, "join");
            if (!m.success)
                return m;

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
            GameLog.logLine(gameId, GameLog.Actions.Player_Join, user.id.ToString());

            //playersChatObserver.Subscribe(p);
            gameStatesObserver.Update(this);
            return new ReturnMessage(true, "");
        }

        //public int AvailableSeats
        //{
        //    get
        //    {
        //        int ans = 0;
        //        for (int i = 0; i < players.Length; i++)
        //        {
        //            if (players[i] == null)
        //                ans++;
        //        }
        //        return ans;
        //    }
        //}

        //public ReturnMessage joinSpectate(SystemUser user)
        //{
        //    //check that the game aloow to spectate
        //    if (!gamePreferences.IsSpectatingAllowed.Value)
        //        return new ReturnMessage(false, "Couldn't spectate the game because the game preferences is not alowing.");

        //    //check that the user is not playing
        //    foreach (Player p in players)
        //        if (p != null)
        //        {
        //            if (p.systemUserID == user.id)
        //                return new ReturnMessage(false, "Couldn't spectate the game because the user is already playing the game.");
        //        }

        //    //check that the user is not spectating
        //    foreach (SystemUser spectateUser in spectators)
        //        if (spectateUser.id == user.id)
        //            return new ReturnMessage(false, "Couldn't spectate the game because the user is already spectating the game.");

        //    spectators.Add(user);
        //    GameLog.logLine(gameId, GameLog.Actions.Spectate_Join, user.id.ToString());
        //    return new ReturnMessage(true,"");
        //}

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
            //gameStatesObserver.Subscribe(p);
            //spectateChatObserver(players);
            //playersChatObserver(players);
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
            gameState = GameState.bFlop;
            preparePlayersState();
            deck.Shuffle();
            dealCards();
            currentSmall = getNextPlayer(currentDealer);
            Console.WriteLine("small " + currentSmall);
            currentBig = getNextPlayer(currentSmall);
            Console.WriteLine("big " + currentBig);
            betBlinds();
        }

        public void playGame()
        {
            GameLog.logLine(gameId, GameLog.Actions.Game_Start, DateTime.Now.ToString());
            InitializeGame();
            // playersSetsTheirBets(true);
            gameStatesObserver.Update(this);


            



        }

        private void preparePlayersState()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null)
                    players[i].playerState = Player.PlayerState.in_round;
            }
        }

        private void checkWhoWins()
        {
            int highRank = -1;
            int winnerIndex = -1;

            for (int i = 0; i < players.Length - 1; i++)
            {
                if (players[i].playerState.Equals(Player.PlayerState.in_round))
                    if ((int)players[i].handRank > highRank)
                    {
                        winnerIndex = i;
                        highRank = (int)players[i].handRank;
                    }
            }

            players[winnerIndex].playerState = PlayerState.winner;
            gameStatesObserver.Update(this);
        }

        public void playersSetsTheirBets(bool firstBets)
        {
            if (firstBets)
                for (int i = nextToSeat(currentBig); i < maxPlayers; i++)
                {
                    if (players[i] != null && players[i].playerState == PlayerState.in_round)
                    {
                        players[i].playerState = PlayerState.my_turn;
                        //UPDATE everybody
                        gameStatesObserver.Update(this);
                    }
                }
            else
                for (int i = nextToSeat(currentDealer); i < maxPlayers; i++)
                {
                    if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                    {
                        players[i].playerState = PlayerState.my_turn;
                        //UPDATE everybody
                        gameStatesObserver.Update(this);
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
        }

        //public int setSmallBlind()
        //{
        //    int i = currentDealer;
        //    int j = (currentDealer + 1) % maxPlayers;
        //    while (i != j)
        //    {
        //        if (players[j] != null && players[j].playerState == Player.PlayerState.in_round)
        //        {
        //            GameLog.logLine(gameId, GameLog.Actions.Small_Blind, players[j].systemUserID.ToString());
        //            return j;
        //        }
        //        j = (j + 1) % maxPlayers;
        //    }
        //    return -1;
        //}
        public int getNextPlayer(int current)
        {
            int i = (current + 1) % maxPlayers;
            while (i != current)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                {
                    GameLog.logLine(gameId, GameLog.Actions.Small_Blind, players[i].systemUserID.ToString());
                    return i;
                }
                i = (i + 1) % maxPlayers;
            }
            throw new ArgumentException("cannot locate next player");
        }

        //public int setBigBlind()
        //{
        //    int i = currentSmall;
        //    int j = (currentSmall + 1) % maxPlayers;
        //    while (i != j)
        //    {
        //        if (players[j] != null && players[j].playerState == Player.PlayerState.in_round)
        //            break;
        //        j = (j + 1) % maxPlayers;
        //    }

        //    j = (j + 1) % maxPlayers;
        //    while (i != j)
        //    {
        //        if (players[j] != null && players[j].playerState == Player.PlayerState.in_round)
        //        {
        //            GameLog.logLine(gameId, GameLog.Actions.Big_Blind, players[j].systemUserID.ToString());
        //            return j;
        //        }
        //        j = (j + 1) % maxPlayers;
        //    }
        //    return -1;
        //}

        public void betBlinds()
        {
            players[currentSmall].Tokens -= currentBlindBet / 2;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Bet,
                players[currentSmall].systemUserID.ToString(),
                (currentBlindBet / 2).ToString());

            players[currentBig].Tokens -= currentBlindBet;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Bet,
                players[currentBig].systemUserID.ToString(),
                (currentBlindBet / 2).ToString());

            tempPot += ((currentBlindBet + (currentBlindBet / 2)));
        }

        private void continueGame()
        {
            switch (gameState)
            {
                case GameState.bFlop:
                    //reveal flop
                    for (int i = 0; i < 3; i++)
                    {
                        Card flopCard = deck.Top();
                        flop.Add(flopCard);
                        GameLog.logLine(gameId, GameLog.Actions.Flop, i.ToString(), flopCard.ToString());
                        gameState++;
                    }
                    break;
                case GameState.bTurn:
                    turn = deck.Top();
                    GameLog.logLine(
                        gameId,
                        GameLog.Actions.Turn,
                        turn.ToString());
                    gameState++;
                    break;
                case GameState.bRiver:
                    river = deck.Top();
                    GameLog.logLine(
                        gameId,
                        GameLog.Actions.River,
                        turn.ToString());
                    gameState++;
                    break;
                case GameState.aRiver:
                    Player p = decideWhoWon();
                    p.playerState = PlayerState.winner;
                    break;
            }
        }

        private Player decideWhoWon()
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round)
                    players[i].handRank = checkHandRank(players[i]);
            }

            int maxHandPlayer = 9;
            int maxHandIndex = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == Player.PlayerState.in_round && (int)players[i].handRank <= maxHandPlayer)
                {
                    maxHandPlayer = (int)players[i].handRank;
                    maxHandIndex = i;
                }
            }
            return players[maxHandIndex];
        }

        public ReturnMessage bet(Player p, int amount)
        {
            if (p.Tokens - amount < 0)
                return new ReturnMessage(false, "not enough coins");
            if (currentBet > amount && amount != p.Tokens)
                return new ReturnMessage(false, "need to bet more");

            currentBet = Math.Max(amount, currentBet);

            p.Tokens -= amount;

            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Raise,
                p.systemUserID.ToString(),
                amount.ToString());

            tempPot += amount;
            int bet = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == PlayerState.in_round && players[i].Tokens != 0)
                {
                    bet = players[i].TokensInBet;
                    break;
                }
            }
            bool isBetOver = true;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == PlayerState.in_round && players[i].TokensInBet != bet && players[i].Tokens != 0)
                {
                    isBetOver = false;
                }
            }

            if (isBetOver)
            {
                addToPot(tempPot);
                gameStatesObserver.Update(this);
                continueGame();
            }
            // TODO: Gili, you need to send the message to the other players
            return new ReturnMessage(true, "");
        }

        public void call(Player p)
        {
            p.Tokens -= currentBet;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Bet,
                p.systemUserID.ToString(),
                currentBet.ToString());
            tempPot += currentBet;

            int call = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == PlayerState.in_round && players[i].Tokens != 0)
                {
                    call = players[i].TokensInBet;
                    break;
                }
            }

            bool isCallOver = true;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == PlayerState.in_round && players[i].TokensInBet != call && players[i].Tokens != 0)
                {
                    isCallOver = false;
                }
            }

            if (isCallOver)
            {
                addToPot(tempPot);
                gameStatesObserver.Update(this);
                continueGame();
            }
        }

        public ReturnMessage fold(Player p)
        {
            p.playerState = Player.PlayerState.folded;
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Fold,
                p.systemUserID.ToString());

            int fold = -1;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == PlayerState.in_round && players[i].Tokens != 0)
                {
                    fold = players[i].TokensInBet;
                    break;
                }
            }

            bool isFoldOver = true;
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i] != null && players[i].playerState == PlayerState.in_round && players[i].TokensInBet != fold && players[i].Tokens != 0)
                {
                    isFoldOver = false;
                }
            }

            if (isFoldOver)
            {
                addToPot(tempPot);
                gameStatesObserver.Update(this);
                continueGame();
            }
            return null; // TODO: Gili!
        }

        public ReturnMessage check(Player p)
        {
            GameLog.logLine(
                gameId,
                GameLog.Actions.Action_Check,
                p.systemUserID.ToString());
            gameStatesObserver.Update(this);
            continueGame();
            return null; // TODO: Gili!
        }

        public int nextToSeat(int seat)
        {
            int i = seat;
            int j = (i + 1) % maxPlayers;
            while (i != j)
            {
                if (players[j] != null && players[j].playerState == Player.PlayerState.in_round)
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
                fullHand.Add(flop[0]);
                flop.RemoveAt(0);
            }

            for (int i = 0; i < 2; i++)
            {
                fullHand.Add(p.playerCards[0]);
            }

            fullHand.Add(turn);
            fullHand.Add(river);

            fullHand.Sort();

            if (checkRoyalFlush(fullHand))
                return HandsRanks.RoyalFlush;
            if (checkStraightFlush(fullHand) != -1)
                return HandsRanks.StraightFlush;
            if (checkFourOfAKind(fullHand) != -1)
                return HandsRanks.FourOfAKind;
            if (checkFullHouse(fullHand, 3) != -1)
                return HandsRanks.FullHouse;
            if (checkFlush(fullHand) != -1)
                return HandsRanks.Flush;
            if (checkStraight(fullHand) != -1)
                return HandsRanks.Straight;
            if (checkThreeOfAKind(fullHand) != -1)
                return HandsRanks.ThreeOfAKind;
            if (checkTwoPairs(fullHand) != -1)
                return HandsRanks.TwoPairs;

            //for (int i = 0; i < fullHand.Count; i++)
            //{
            //    Console.Out.Write(fullHand[i].Value + "  ");
            //    Console.Out.Write(fullHand[i].Type.ToString() + "  ");
            //    Console.Out.WriteLine();
            //}


            return HandsRanks.HighCard;
        }

        private void addToPot(int sum)
        {
            pot += sum;
            GameLog.logLine(gameId, GameLog.Actions.Pot_Changed, sum.ToString(), pot.ToString());
        }


        public bool checkRoyalFlush(List<Card> fullHand)
        {
            int[] counters = new int[4];
            for (int i = 0; i < 4; i++)
                counters[i] = 0;

            for (int i = 0; i < 7; i++)
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

            for (int i = 0; i < 7; i++)
                valueBits[(int)fullHand[i].Type] += (int)Math.Pow(2, fullHand[i].Value - 1);

            int bitCounter = 0;

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 13; j++)
                {
                    if (valueBits[i] % 2 == 1)
                    {
                        bitCounter++;
                        valueBits[i] = valueBits[i] / 2;
                        Console.Out.WriteLine(bitCounter + "    first  ");

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
            for (int i = 0; i < 7; i++)
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

            for (int i = 0; i < 7; i++)
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
            for (int i = 0; i < 7; i++)
                if (fullHand[i].Value == 1 ||
                    fullHand[i].Value == 10 ||
                    fullHand[i].Value == 11 ||
                    fullHand[i].Value == 12 ||
                    fullHand[i].Value == 13)
                    valueCounter++;
            if (valueCounter == 5)
                return 1;

            int valueBits = 0;

            for (int i = 0; i < 7; i++)
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

            for (int i = 0; i < 7; i++)
                threeOfAKindCounter[fullHand[i].Value - 1]++;

            for (int i = 0; i < 13; i++)
                if (threeOfAKindCounter[i] == 3)
                    return 0;

            return -1;
        }

        public int checkTwoPairs(List<Card> fullHand)
        {
            int[] twoOfAKindCounterA = new int[13];
            int[] twoOfAKindCounterB = new int[13];

            for (int i = 0; i < 13; i++)
            {
                twoOfAKindCounterA[i] = 0;
                twoOfAKindCounterB[i] = 0;
            }

            for (int i = 0; i < 7; i++)
            {
                twoOfAKindCounterA[fullHand[i].Value - 1]++;
                twoOfAKindCounterB[fullHand[i].Value - 1]++;
            }

            for (int i = 0; i < 13; i++)
                if (twoOfAKindCounterA[i] == 2 && twoOfAKindCounterB[i] == 2)
                    return 0;
            return -1;
        }

        public int checkPair(List<Card> fullHand)
        {
            int[] pairCounter = new int[13];
            for (int i = 0; i < 13; i++)
                pairCounter[i] = 0;

            for (int i = 0; i < 7; i++)
                pairCounter[fullHand[i].Value - 1]++;

            for (int i = 0; i < 13; i++)
                if (pairCounter[i] == 2)
                    return 0;

            return -1;
        }

        // TODO: Gili - tries to position a player where he wants (on a seat)
        public ReturnMessage ChoosePlayerSeat(int playerIndex)
        {
            return null;
        }
        public Player GetPlayer(int playerIndex)
        {
            return players[playerIndex];
        }
        public Card[] GetPlayerCards(int playerIndex)
        {
            return players[playerIndex].playerCards.ToArray();
        }
        // TODO: Gili - when showoff happends this method would be called to get all the cards - so dont forget to update when a showoff happens
        public IDictionary<int, Card[]> GetShowOff()
        {
            IDictionary<int, Card[]> ans = new Dictionary<int, Card[]>();
            for (int i = 0; i < players.Length; i++)
            {
                ans[i] = players[i].playerCards.ToArray();
            }
            return ans;
        }
    }
}