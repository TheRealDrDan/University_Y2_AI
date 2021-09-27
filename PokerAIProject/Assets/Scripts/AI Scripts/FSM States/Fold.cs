using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fold<T> : State<T>
{

    private AIBehaviour AIBehaviour;

    public Fold(T stateName, AIBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        AIBehaviour = controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateFinished = true;
    }
}
