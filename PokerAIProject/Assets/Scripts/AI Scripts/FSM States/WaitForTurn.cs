using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForTurn<T> : State<T>
{

    private AIBehaviour AIBehaviour;

    public WaitForTurn(T stateName, AIBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        AIBehaviour = controller;
    }
}
