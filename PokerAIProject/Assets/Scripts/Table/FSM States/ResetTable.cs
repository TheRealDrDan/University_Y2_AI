using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetTable<T> : State<T> {

    private TableBehaviour tableBehaviour;

    public ResetTable(T stateName, TableBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        tableBehaviour = controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        tableBehaviour.ResetTable();
        tableBehaviour.play = false;
        for (int i = 0; i < tableBehaviour.players.Count; i++)
        {
            tableBehaviour.players[i].resetToPlay();
        }
        tableBehaviour.roundCount = 1;   
        tableBehaviour.DestroyAllCards();
        tableBehaviour.table.ClearTable();
        AssignRole();
        stateFinished = true;
    }

    private void AssignRole()
    {
        for (int j = 0; j < tableBehaviour.players.Count; j++)
        {
            if (tableBehaviour.players[TableBehaviour.tb.playerTurn].role == TableBehaviour.Role.NA)
            {
                switch (j)
                {
                    case 0:
                        tableBehaviour.players[TableBehaviour.tb.playerTurn].role = TableBehaviour.Role.D;
                        break;
                    case 1:
                        tableBehaviour.players[TableBehaviour.tb.playerTurn].role = TableBehaviour.Role.SB;
                        break;
                    case 2:
                        tableBehaviour.players[TableBehaviour.tb.playerTurn].role = TableBehaviour.Role.BB;
                        break;
                    default:
                        tableBehaviour.players[TableBehaviour.tb.playerTurn].role = TableBehaviour.Role.S;
                        break;
                }
                tableBehaviour.players[TableBehaviour.tb.playerTurn].roleText.text = tableBehaviour.players[TableBehaviour.tb.playerTurn].role.ToString();
                if (tableBehaviour.players[TableBehaviour.tb.playerTurn].role == TableBehaviour.Role.D)
                    tableBehaviour.dealerID = tableBehaviour.playerTurn;
            }
            tableBehaviour.IncrementWhosTurn();
        }
    }
}
