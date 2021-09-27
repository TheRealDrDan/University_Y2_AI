using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class River<T> : State<T>
{

    private TableBehaviour tableBehaviour;
    private int currentEntryRoundCount = 0;

    public River(T stateName, TableBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        tableBehaviour = controller;
    }

    public override void OnEnter()
    {
        stateFinished = false;
        Debug.Log("River");
        if (tableBehaviour.IsOnePlayerLeft())
        {
            stateFinished = true;
            return;
        }
        tableBehaviour.table.card5 = tableBehaviour.PickACard();
        tableBehaviour.DisplayCard(tableBehaviour.table.card5, tableBehaviour.table.card5Position);
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