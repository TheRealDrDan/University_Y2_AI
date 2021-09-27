using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bet<T> : State<T> {

    private AIBehaviour AIBehaviour;
    
    public Bet(T stateName, AIBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        AIBehaviour = controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        AIBehaviour.bet = false;
        if (!AIBehaviour.boughtin)
        {
            FirstRoundBet();
        }
        else
        {
            BetMoney(TableBehaviour.tb.minBet);
            stateFinished = true;
        }
    }

    private void BetMoney(float value)
    {
        AIBehaviour.currentBet += value;
        AIBehaviour.betText.text = AIBehaviour.currentBet.ToString();
        AIBehaviour.DecreaseMoney(value);
        TableBehaviour.tb.AddToPot(value);
    }

    private void FirstRoundBet()
    {
        if (AIBehaviour.role == TableBehaviour.Role.SB)
        {
            BetMoney(TableBehaviour.tb.minBet / 2);        
        }
        else 
        {
            BetMoney(TableBehaviour.tb.minBet);
        }
        stateFinished = true;
    }

}
