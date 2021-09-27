using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flop<T> : State<T>
{

    private TableBehaviour tableBehaviour;
    private int currentEntryRoundCount = 0;

    public Flop(T stateName, TableBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        tableBehaviour = controller;
    }

    public override void OnEnter()
    {
        stateFinished = false;
        Debug.Log("Flop");
        if (tableBehaviour.IsOnePlayerLeft())
        {
            stateFinished = true;
            return;
        }
        tableBehaviour.table.card4 = tableBehaviour.PickACard();
        tableBehaviour.DisplayCard(tableBehaviour.table.card4, tableBehaviour.table.card4Position);
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