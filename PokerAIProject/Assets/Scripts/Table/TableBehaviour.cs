using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBehaviour : MonoBehaviour {

	public enum tableStates { Deal, Firstroundbetting, Preflop, Flop, River, Showdown, ResetTable, OnePlayerLeft};
    [HideInInspector]
    public FSM<tableStates> tableFSM;

    public enum Role { NA, S, D, SB, BB};                                                           //Enum to assign roles
    [HideInInspector]
    public static TableBehaviour tb;

    [Header("Card Graphics")]
    public GameObject cardBack;                                                                     //Back of the card image
    public GameObject[] Hearts;                                                                     //All heart cards
    public GameObject[] Clubs;                                                                      //All club cards
    public GameObject[] Diamonds;                                                                   //All diamond cards
    public GameObject[] Spades;                                                                     //All spades cards
    [Space]

    [Header("Table Card Holder")]
    public TableCardHolder table;                                                                   //Tables card holders
    [Space]

    [Header("Players Controllers")]
    public List<CardPlayer> players;                                                                //All players in the match
    private List<CardPlayer> allPlayers;
    [Space]

    [Header("Game Limits & Rules")]
    public float startMoney = 1000f;                                                                //The money every player starts with
    public float minBetPercentageOfStartMoney = 0.01f;                                              //The percentage from the startmoney that is a minimum bet
    [HideInInspector]
    public float minBet;


    [HideInInspector]
    public int playerTurn = 0;                                                                      //Stores the current players turn
    [HideInInspector]
    public int previousPlayerTurn = 0;                                                              //Previous players turn
    [HideInInspector]
    public bool turnFinished = false;                                                               //Stores if the current player has finished their turn

    [HideInInspector]
    public int dealerID;                                                                            //The player who is a dealer

    [HideInInspector]
    public bool play = false;                                                                       //Determins if the match is playing

    [HideInInspector]
    public int roundCount = 1;                                                                      //The round number

    [HideInInspector]
    public List<GameObject> cardObjects;

    public float cardDealTime = 1.0f;   //Redundant variable (was used for animations before they were removed due to time constraints)

    [HideInInspector]
    public float pot;                                                                               //Value of the pot
    [HideInInspector]
    public float lastBet;                                                                           //The value of the last bet to the pot

    private void Awake()
    {
        tb = this;
        minBet = startMoney * minBetPercentageOfStartMoney;
    }
    void Start () {
        allPlayers = new List<CardPlayer>();
        for (int i = 0; i < players.Count; i++)
        {
            allPlayers.Add(players[i]);
        }
        InitialiseFSM();
        ResetTable();
	}
		
	void Update () {
        tableFSM.CurrentState.Act();
        tableFSM.Check();
	}

    public void ResetTable()
    {
        //playerTurn = Random.Range(0, players.Count);              //If the dealer is too be randomised, use this code.
        playerTurn = 0;
        pot = 0;
        lastBet = 0;
        players.Clear();
        players = new List<CardPlayer>();
        for (int i = 0; i < allPlayers.Count; i++)
        {
            players.Add(allPlayers[i]);
        }
        play = false;
    }

    private void InitialiseFSM()
    {
        tableFSM = new FSM<tableStates>();
        tableFSM.AddState(new Deal<tableStates>(tableStates.Deal, this, 1f));
        tableFSM.AddState(new Firstroundbetting<tableStates>(tableStates.Firstroundbetting, this, 0f));
        tableFSM.AddState(new Preflop<tableStates>(tableStates.Preflop, this, 0.2f));
        tableFSM.AddState(new Flop<tableStates>(tableStates.Flop, this, 0.2f));
        tableFSM.AddState(new River<tableStates>(tableStates.River, this, 0.2f));
        tableFSM.AddState(new Showdown<tableStates>(tableStates.Showdown, this, 10f));
        tableFSM.AddState(new ResetTable<tableStates>(tableStates.ResetTable, this, 0.5f));
        tableFSM.AddState(new OnePlayerLeft<tableStates>(tableStates.OnePlayerLeft, this, 0.5f));


        tableFSM.AddTransition(tableStates.Deal, tableStates.Firstroundbetting);
        tableFSM.AddTransition(tableStates.Firstroundbetting, tableStates.Preflop);
        tableFSM.AddTransition(tableStates.Preflop, tableStates.Flop);
        tableFSM.AddTransition(tableStates.Flop, tableStates.River);
        tableFSM.AddTransition(tableStates.River, tableStates.Showdown);

        tableFSM.AddTransition(tableStates.Showdown, tableStates.ResetTable);
        tableFSM.AddTransition(tableStates.OnePlayerLeft, tableStates.ResetTable);

        tableFSM.AddTransition(tableStates.ResetTable, tableStates.Deal);

        tableFSM.AddTransition(tableStates.Showdown, tableStates.OnePlayerLeft);


        tableFSM.SetInitialState(tableStates.ResetTable);
    }
    public bool GuardOnePlayerLeftToResetTable(State<tableStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardShowdownToOnePlayerLeft(State<tableStates> currentstate)
    {
        return IsOnePlayerLeft() && currentstate.StateFinished;
    }

    public bool GuardDealToFirstroundbetting(State<tableStates> currentState)
    {
        if (currentState.StateFinished)
            play = true;
        return currentState.StateFinished;
    }

    public bool GuardFirstroundbettingToPreflop(State<tableStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardPreflopToFlop(State<tableStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardFlopToRiver(State<tableStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardRiverToShowdown(State<tableStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardShowdownToResetTable(State<tableStates> currentState)
    {
        return currentState.StateFinished && !IsOnePlayerLeft();
    }

    public bool GuardResetTableToDeal(State<tableStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool IsOnePlayerLeft()
    {
        
        if (players.Count == 1)
        {
            play = false;
            return true;
        }            
        return false;
    }

    public void AddToPot(float value)
    {
        pot += value;
        table.potText.text = pot.ToString();
        lastBet = value;
    } 

    public void CreateCardObject(Transform position)
    {
        GameObject card = Instantiate(cardBack, position);
        cardObjects.Add(card);
    }

    public void DestroyAllCards()
    {
        for (int i = 0; i < cardObjects.Count; i++)
        {
            Destroy(cardObjects[i]);
        }
        cardObjects.Clear();
        cardObjects = new List<GameObject>();
    }

    public GameObject DisplayCard(Card card, Transform position)
    {
        switch (card.suit)
        {
            case Card.Suits.H:
                GameObject cardHearts = Instantiate(Hearts[card.value - 2], position);
                cardObjects.Add(cardHearts);
                return cardHearts;
            case Card.Suits.C:
                GameObject cardClubs = Instantiate(Clubs[card.value - 2], position);
                cardObjects.Add(cardClubs);
                return cardClubs;
            case Card.Suits.D:
                GameObject cardDiamonds = Instantiate(Diamonds[card.value - 2], position);
                cardObjects.Add(cardDiamonds);
                return cardDiamonds;
            case Card.Suits.S:
                GameObject cardSpades = Instantiate(Spades[card.value - 2], position);
                cardObjects.Add(cardSpades);
                return cardSpades;
        }
        return null;
    }

    public Card PickACard()
    {
        bool unPicked = false;
        Card card = new Card(Card.Suits.C, 1);
        while (unPicked != true)
        {
            int suit = Random.Range(0, 40);

            if (suit <= 10)
            {
                card.suit = Card.Suits.H;
            }
            else if (suit > 10 && suit <= 20)
            {
                card.suit = Card.Suits.C;
            }
            else if (suit > 20 && suit <= 30)
            {
                card.suit = Card.Suits.D;
            }
            else
            {
                card.suit = Card.Suits.S;
            }

            int value = Random.Range(0, 130);
            if (value <= 10)
            {
                card.value = 2;
            }
            else if (value > 10 && value <= 20)
            {
                card.value = 3;
            }
            else if (value > 20 && value <= 30)
            {
                card.value = 4;
            }
            else if (value > 30 && value <= 40)
            {
                card.value = 5;
            }
            else if (value > 40 && value <= 50)
            {
                card.value = 6;
            }
            else if (value > 50 && value <= 60)
            {
                card.value = 7;
            }
            else if (value > 60 && value <= 70)
            {
                card.value = 8;
            }
            else if (value > 70 && value <= 80)
            {
                card.value = 9;
            }
            else if (value > 80 && value <= 90)
            {
                card.value = 10;
            }
            else if (value > 90 && value <= 100)
            {
                card.value = 11;
            }
            else if (value > 100 && value <= 110)
            {
                card.value = 12;
            }
            else if (value > 110 && value <= 120)
            {
                card.value = 13;
            }
            else
            {
                card.value = 14;
            }
            if (IsCardInPlay(card))
            {
                unPicked = false;
            }
            else
            {
                unPicked = true;
            }
        }
        return card;
    }

    private bool IsCardInPlay(Card cardToCheck)
    {
        foreach (CardPlayer player in players)
        {
            if (player.cardHolder.card1 != null)
            {
                if (player.cardHolder.card1.suit == cardToCheck.suit && player.cardHolder.card1.value == cardToCheck.value)
                {
                    Debug.Log("Same card found");
                    return true;
                }
            }


            if (player.cardHolder.card2 != null)
            {
                if (player.cardHolder.card2.suit == cardToCheck.suit && player.cardHolder.card2.value == cardToCheck.value)
                {
                    Debug.Log("Same card found");
                    return true;
                }
            }


        }
        if (table.card1 != null)
        {
            if (table.card1.suit == cardToCheck.suit && table.card1.value == cardToCheck.value)
            {
                Debug.Log("Same card found");
                return true;
            }
        }
        if (table.card2 != null)
        {
            if (table.card2.suit == cardToCheck.suit && table.card2.value == cardToCheck.value)
            {
                Debug.Log("Same card found");
                return true;
            }
        }
        if (table.card3 != null)
        {
            if (table.card3.suit == cardToCheck.suit && table.card3.value == cardToCheck.value)
            {
                Debug.Log("Same card found");
                return true;
            }
        }
        if (table.card4 != null)
        {
            if (table.card4.suit == cardToCheck.suit && table.card4.value == cardToCheck.value)
            {
                Debug.Log("Same card found");
                return true;
            }
        }
        if (table.card5 != null)
        {
            if (table.card5.suit == cardToCheck.suit && table.card5.value == cardToCheck.value)
            {
                Debug.Log("Same card found");
                return true;
            }
        }

        return false;
    }

     public void IncrementWhosTurn()
    {
        previousPlayerTurn = playerTurn;
        playerTurn++;
        if (playerTurn >= players.Count)
            playerTurn = 0;
        Debug.Log("Player turn is: " + playerTurn + ". Round count is: " + roundCount);
    }

    public void Check()
    {
        if (!play)
            return;
        if (!turnFinished)
            return;
        turnFinished = false;
        roundCount++;
        IncrementWhosTurn();      
        players[playerTurn].Play();
    }

    public void RemovePlayerFromMatch(CardPlayer playerToRemove)
    {
        int playerIndex = -1;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == playerToRemove)
                playerIndex = i;
        }
        if (playerIndex == 0)
            previousPlayerTurn = players.Count;
        else
            previousPlayerTurn = playerTurn--;
        if(playerIndex != -1)
        {
            players[playerIndex].roleText.text = "FOLDED";
            players.RemoveAt(playerIndex);
        }
    }

    public int ReturnPreviousPlayer()
    {
        return previousPlayerTurn;
    }

}
