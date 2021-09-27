using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deal<T> : State<T> {

    private TableBehaviour tableBehaviour;
    private bool dealCards = false;

    private float currentDealTime = 0f;

    private int cardToDealTo = 0;

	public Deal(T stateName, TableBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        tableBehaviour = controller;
    }

    public override void OnEnter()
    {    
        dealCards = true;
        Debug.Log(tableBehaviour.dealerID);
        Debug.Log(tableBehaviour.playerTurn);
        for (int i = 0; i < tableBehaviour.players.Count; i++)
        {
            tableBehaviour.players[i].cardHolder.card1 = new Card(Card.Suits.C, 14);
            tableBehaviour.players[i].cardHolder.card1 = tableBehaviour.PickACard();
            tableBehaviour.CreateCardObject(tableBehaviour.players[i].cardHolder.card1Position);
            tableBehaviour.players[i].cardHolder.card2 = new Card(Card.Suits.C, 14);
            tableBehaviour.players[i].cardHolder.card2 = tableBehaviour.PickACard();
            tableBehaviour.CreateCardObject(tableBehaviour.players[i].cardHolder.card2Position);           
        }
        ShowPlayerCards();
        ShowCardsInConsole();
        dealCards = false;
        tableBehaviour.turnFinished = true;
        stateFinished = true;
    }
    /// <summary>
    /// This code used to animate cards to each player
    /// Was disabled due to time constraints, please ignore.
    /// </summary>
    //public override void Act()
    //{
        //if (!dealCards)
        //    return;
        //if(currentDealTime <= 0f)
        //{
        //    if(tableBehaviour.playerTurn == tableBehaviour.dealerID && cardToDealTo >= 2)
        //    {
        //        //EXIT STATE CONDITIONS HERE!
                
        //        ShowCardsInConsole();
        //        ShowPlayerCards();
        //        tableBehaviour.turnFinished = true;

        //        dealCards = false;
        //        stateFinished = true;
              
        //    }
        //    else
        //    {           
        //        if (cardToDealTo == 0)
        //        {
        //            tableBehaviour.playerCards[tableBehaviour.playerTurn].card1 = new Card(Card.Suits.C, 14);
        //            tableBehaviour.playerCards[tableBehaviour.playerTurn].card1 = tableBehaviour.PickACard();
        //            tableBehaviour.CreateCardObject(tableBehaviour.playerCards[tableBehaviour.playerTurn].card1Position);
        //        }
        //        else if (cardToDealTo == 1)
        //        {
        //            tableBehaviour.playerCards[tableBehaviour.playerTurn].card2 = new Card(Card.Suits.C, 14);
        //            tableBehaviour.playerCards[tableBehaviour.playerTurn].card2 = tableBehaviour.PickACard();
        //            tableBehaviour.CreateCardObject(tableBehaviour.playerCards[tableBehaviour.playerTurn].card2Position);
        //        }
        //        if (tableBehaviour.playerTurn == tableBehaviour.dealerID)
        //        {
        //            cardToDealTo++;
        //        }
        //        tableBehaviour.IncrementWhosTurn();
        //    }
        //    currentDealTime = 1f / tableBehaviour.cardDealTime;
        //}
        //currentDealTime -= Time.deltaTime;
    //}

    private void ShowCardsInConsole()
    {
        foreach (CardPlayer player in tableBehaviour.players)
        {
            Debug.Log(player.ToString() + "'s first card is: " + player.cardHolder.card1.suit + player.cardHolder.card1.value);
            Debug.Log(player.ToString() + "'s second card is: " + player.cardHolder.card2.suit + player.cardHolder.card2.value);
        }
    }
    private void ShowPlayerCards()
    {
        tableBehaviour.DisplayCard(tableBehaviour.players[0].cardHolder.card1, tableBehaviour.players[0].cardHolder.card1Position);
        tableBehaviour.DisplayCard(tableBehaviour.players[0].cardHolder.card2, tableBehaviour.players[0].cardHolder.card2Position);
    }

}
