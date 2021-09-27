using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Call<T> : State<T>
{

    private AIBehaviour AIBehaviour;

    public Call(T stateName, AIBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        AIBehaviour = controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        AIBehaviour.call = false;
        float difference = 0f;
        difference = TableBehaviour.tb.players[TableBehaviour.tb.ReturnPreviousPlayer()].currentBet - AIBehaviour.currentBet;
        AIBehaviour.currentBet += difference;
        AIBehaviour.betText.text = AIBehaviour.currentBet.ToString();
        AIBehaviour.DecreaseMoney(difference);
        TableBehaviour.tb.AddToPot(difference);
        stateFinished = true;
    }
}
