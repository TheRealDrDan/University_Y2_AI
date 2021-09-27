using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Preflop<T> : State<T>
{

    private TableBehaviour tableBehaviour;
    private int currentEntryRoundCount = 0;

    public Preflop(T stateName, TableBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        tableBehaviour = controller;
    }

    public override void OnEnter()
    {
        stateFinished = false;
        Debug.Log("Preflop");
        if (tableBehaviour.IsOnePlayerLeft())
        {
            stateFinished = true;
            return;
        }
        tableBehaviour.table.card1 = tableBehaviour.PickACard();
        tableBehaviour.table.card2 = tableBehaviour.PickACard();
        tableBehaviour.table.card3 = tableBehaviour.PickACard();
        tableBehaviour.DisplayCard(tableBehaviour.table.card1, tableBehaviour.table.card1Position);
        tableBehaviour.DisplayCard(tableBehaviour.table.card2, tableBehaviour.table.card2Position);
        tableBehaviour.DisplayCard(tableBehaviour.table.card3, tableBehaviour.table.card3Position);
        tableBehaviour.roundCount++;
        currentEntryRoundCount = tableBehaviour.roundCount;
    }

    public override void Act()
    {
        if (stateFinished)
            return;      
        if (tableBehaviour.IsOnePlayerLeft())
        {
            stateFinished = true;
            return;
        }
        tableBehaviour.Check();
        if (tableBehaviour.playerTurn == tableBehaviour.dealerID)
        {
            float playerBet = tableBehaviour.players[0].currentBet;
            bool allEqual = true;
            for (int i = 1; i < tableBehaviour.players.Count; i++)
            {
                if (tableBehaviour.players[i].currentBet != playerBet)
                    allEqual = false;
            }
            if (allEqual && currentEntryRoundCount != tableBehaviour.roundCount)
            {
                stateFinished = true;
            }

        }
    }
}