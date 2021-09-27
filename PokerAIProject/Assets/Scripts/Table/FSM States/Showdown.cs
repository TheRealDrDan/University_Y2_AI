using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Showdown<T> : State<T>
{

    private TableBehaviour tableBehaviour;

    private struct Winner
    {
        public int handValue;
        public int playerID;
        public int valueOfAllCards;
        public Winner(int value1, int value2)
        {
            handValue = value1;
            playerID = value2;
            valueOfAllCards = 0;
        }
        public void SetValueOfAllCards(int value)
        {
            valueOfAllCards = value;
        }
    }

    public Showdown(T stateName, TableBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        tableBehaviour = controller;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("SHOWDOWN");
        for (int i = 0; i < tableBehaviour.players.Count; i++)
        {
            tableBehaviour.players[i].canPlay = false;
        }
        if (tableBehaviour.IsOnePlayerLeft())
        {
            stateFinished = true;
            return;
        }
        ShowDown();
        stateFinished = true;
    }

    private void ShowDown()
    {
        tableBehaviour.play = false;

        for (int i = 0; i < tableBehaviour.players.Count; i++)
        {
            Hand hand = tableBehaviour.players[i].myHand();
            Debug.Log(hand.hands.ToString());
        }

        int bestValue = 0;
        int handValue = 0;
        int playerID = 0;
        List<Winner> potentialWinners = new List<Winner>();
        for (int i = 0; i < tableBehaviour.players.Count; i++)
        {
            tableBehaviour.IncrementWhosTurn();
            handValue = (int)tableBehaviour.players[tableBehaviour.playerTurn].hand.hands;
            tableBehaviour.DisplayCard(tableBehaviour.players[tableBehaviour.playerTurn].cardHolder.card1, tableBehaviour.players[tableBehaviour.playerTurn].cardHolder.card1Position);
            tableBehaviour.DisplayCard(tableBehaviour.players[tableBehaviour.playerTurn].cardHolder.card2, tableBehaviour.players[tableBehaviour.playerTurn].cardHolder.card2Position);
            playerID = tableBehaviour.playerTurn;
            if (handValue > bestValue)
            {
                bestValue = handValue;
                potentialWinners = new List<Winner>();
                Winner temp = new Winner(bestValue,playerID);                
                potentialWinners.Add(temp);
            }
            else if (handValue == bestValue)
            {
                bestValue = handValue;
                Winner temp = new Winner(bestValue, playerID);
                potentialWinners.Add(temp);
            }
        }

        if(potentialWinners.Count == 1)
        {        
                Debug.Log(tableBehaviour.players[potentialWinners[0].playerID].ToString() + " wins!");
                tableBehaviour.players[potentialWinners[0].playerID].IncreaseMoney(tableBehaviour.pot);
        }
        else
        {
            for (int i = 0; i < potentialWinners.Count; i++)
            {
                    potentialWinners[i].SetValueOfAllCards(CheckHand.checkHand.CardsInHandValue(tableBehaviour.players[potentialWinners[i].playerID].hand.cards));
            }
            Winner winner = new Winner(0,0);
            for (int i = 0; i < potentialWinners.Count; i++)
            {
                if (potentialWinners[i].valueOfAllCards > winner.valueOfAllCards)
                    winner = potentialWinners[i];
            }
            Debug.Log(tableBehaviour.players[winner.playerID].ToString() + " wins!");
            tableBehaviour.players[winner.playerID].IncreaseMoney(tableBehaviour.pot);
        }        
    }     

}