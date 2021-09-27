using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firstroundbetting<T> : State<T>
{

    private TableBehaviour tableBehaviour;

    public Firstroundbetting(T stateName, TableBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        tableBehaviour = controller;
    }
    public override void OnEnter()
    {
        stateFinished = false;
        tableBehaviour.roundCount++;
        base.OnEnter();
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
            if (allEqual)
            {               
                stateFinished = true;
            }
               
        }
    }

}