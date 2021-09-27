using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePlayerLeft<T> : State<T> {

    private TableBehaviour tableBehaviour;

    public OnePlayerLeft(T stateName, TableBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        tableBehaviour = controller;
    }

    public override void OnEnter()
    {
        Debug.Log(tableBehaviour.players[0].ToString() + " wins!");
        tableBehaviour.players[0].IncreaseMoney(tableBehaviour.pot);

        tableBehaviour.DisplayCard(tableBehaviour.players[0].cardHolder.card1, tableBehaviour.players[0].cardHolder.card1Position);
        tableBehaviour.DisplayCard(tableBehaviour.players[0].cardHolder.card2, tableBehaviour.players[0].cardHolder.card2Position);
        stateFinished = true;
    }

}
