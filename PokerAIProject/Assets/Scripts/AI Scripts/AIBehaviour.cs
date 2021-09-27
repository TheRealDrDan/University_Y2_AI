using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AIBehaviour : CardPlayer {

    public enum AIStates { WaitForTurn, EndTurn, Play, Check, Call, Bet, Raise, Fold}
    [HideInInspector]
    public FSM<AIStates> AIFSM;


    [HideInInspector]
    public bool check = false;
    [HideInInspector]
    public bool call = false;
    [HideInInspector]
    public bool bet = false;
    [HideInInspector]
    public bool raise = false;
    [HideInInspector]
    public bool fold = false;

    [Space]
    [Header("UTILITY AI VALUES")]
    public float checkValue = 6f;
    [Space]
    public float callValue = 6f;
    [Space]
    public float betValue = 2f;
    [Space]
    public float raiseValue = 2f;
    [Space]
    public float foldValue = 5f;


    private void Awake()
    {
        cardHolder = GetComponent<CardHolder>();
        hand = new Hand(Hand.Hands.HighCard);
        canPlay = false;
    }

    void Start () {
        money = TableBehaviour.tb.startMoney;
        moneyText.text = money.ToString();
        InitialiseFSM();
	}

    private void Update()
    {
        AIFSM.CurrentState.Act();
        AIFSM.Check();
    }

    private void InitialiseFSM()
    {
        AIFSM = new FSM<AIStates>();
        AIFSM.AddState(new WaitForTurn<AIStates>(AIStates.WaitForTurn, this, 0f));
        AIFSM.AddState(new EndTurn<AIStates>(AIStates.EndTurn, this, 2f));
        AIFSM.AddState(new Play<AIStates>(AIStates.Play, this, 1f));
        AIFSM.AddState(new Check<AIStates>(AIStates.Check, this, 1f));
        AIFSM.AddState(new Call<AIStates>(AIStates.Call, this, 0f));
        AIFSM.AddState(new Bet<AIStates>(AIStates.Bet, this, 0f));
        AIFSM.AddState(new Raise<AIStates>(AIStates.Raise, this, 0f));
        AIFSM.AddState(new Fold<AIStates>(AIStates.Fold, this, 0f));

        AIFSM.SetInitialState(AIStates.WaitForTurn);

        AIFSM.AddTransition(AIStates.WaitForTurn, AIStates.Play);
        AIFSM.AddTransition(AIStates.EndTurn, AIStates.WaitForTurn);

        AIFSM.AddTransition(AIStates.Play, AIStates.Bet);
        AIFSM.AddTransition(AIStates.Bet, AIStates.EndTurn);

        AIFSM.AddTransition(AIStates.Play, AIStates.Check);
        AIFSM.AddTransition(AIStates.Check, AIStates.EndTurn);

        AIFSM.AddTransition(AIStates.Play, AIStates.Call);
        AIFSM.AddTransition(AIStates.Call, AIStates.EndTurn);

        AIFSM.AddTransition(AIStates.Play, AIStates.Raise);
        AIFSM.AddTransition(AIStates.Raise, AIStates.EndTurn);

        AIFSM.AddTransition(AIStates.Play, AIStates.Fold);
        AIFSM.AddTransition(AIStates.Fold, AIStates.EndTurn);
    }

    public bool GuardEndTurnToWaitForTurn(State<AIStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardWaitForTurnToPlay(State<AIStates> currentState)
    {
        return canPlay;
    }

    public bool GuardPlayToBet(State<AIStates> currentState)
    {
        return bet;
    }

    public bool GuardBetToEndTurn(State<AIStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardPlayToCheck(State<AIStates> currentState)
    {
        return check;
    }

    public bool GuardCheckToEndTurn(State<AIStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardPlayToCall(State<AIStates> currentState)
    {
        return call;
    }

    public bool GuardCallToEndTurn(State<AIStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardPlayToRaise(State<AIStates> currentState)
    {
        return raise;
    }

    public bool GuardRaiseToEndTurn(State<AIStates> currentState)
    {
        return currentState.StateFinished;
    }

    public bool GuardPlayToFold(State<AIStates> currentState)
    {
        if (fold)
            Fold();
        return fold;
    }

    public bool GuardFoldToEndTurn(State<AIStates> currentState)
    {
        return currentState.StateFinished;
    }

    public void StopPlay()
    {
        canPlay = false;
    }
    
}
