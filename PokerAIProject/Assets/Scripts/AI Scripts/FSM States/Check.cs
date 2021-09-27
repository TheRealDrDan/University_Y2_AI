using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check<T> : State<T>
{

    private AIBehaviour AIBehaviour;

    public Check(T stateName, AIBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        AIBehaviour = controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        AIBehaviour.check = false;
        stateFinished = true;
    }
}
