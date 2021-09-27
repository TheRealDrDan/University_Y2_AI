using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raise<T> : State<T>
{

    private AIBehaviour AIBehaviour;

    public Raise(T stateName, AIBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        AIBehaviour = controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        AIBehaviour.raise = false;
        AIBehaviour.currentBet += TableBehaviour.tb.minBet * 2;
        AIBehaviour.betText.text = AIBehaviour.currentBet.ToString();
        AIBehaviour.DecreaseMoney(TableBehaviour.tb.minBet * 2);
        TableBehaviour.tb.AddToPot(TableBehaviour.tb.minBet * 2);
        stateFinished = true;
    }
}
