using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn<T> : State<T>
{

    private AIBehaviour AIBehaviour;

    public EndTurn(T stateName, AIBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        AIBehaviour = controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        AIBehaviour.StopPlay();
        TableBehaviour.tb.turnFinished = true;
        stateFinished = true;
    }
}
