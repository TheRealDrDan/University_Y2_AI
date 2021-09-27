using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play<T> : State<T>
{

    private AIBehaviour AIBehaviour;

    private bool check = true;
    private bool call = true;
    private bool bet = true;
    private bool raise = true;
    private bool fold = true;

    private float checkUtility = 0f;
    private float callUtility = 0f;
    private float betUtility = 0f;
    private float raiseUtility = 0f;
    private float foldUtility = 0f;

    private float totalUtility = 0f;
    private int optionsAvailable = 0;

    public Play(T stateName, AIBehaviour controller, float minDuration) : base(stateName, controller, minDuration)
    {
        AIBehaviour = controller;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Logic();
    }

    private void Options()
    {
        check = true;
        call = true;
        bet = true;
        raise = true;
        fold = true;
        RemoveImpossibleOptions();
    }

    private void RemoveImpossibleOptions()
    {
        int previousPlayer = TableBehaviour.tb.ReturnPreviousPlayer();
        if (TableBehaviour.tb.players[previousPlayer].currentBet == AIBehaviour.currentBet)
        {
            call = false;
            raise = false;
        }
        else
        {
            check = false;
            bet = false;
        }   
    }

    private float FuzzyHandValue()
    {
        float result = 0;
        float x = (int)AIBehaviour.hand.hands;
        float x1 = (int)Hand.Hands.RoyalFlush;
        if (x <= 0)
        {
            result = 0;
        }
        else
        {
            result = (((x / (x1 - (int)Hand.Hands.HighCard)) - ((int)Hand.Hands.HighCard / (x1 - (int)Hand.Hands.HighCard))));
        }
        return result;
    }

    private float FuzzyNot(float value)
    {
        return 1 - value;
    }
    private float FuzzyOR(float value1,float value2)
    {
        if (value1 > value2)
            return value1;
        else
            return value2;
    }

    private void UtilityValues()
    {
        checkUtility = 0f;
        callUtility = 0f;
        betUtility = 0f;
        raiseUtility = 0f;
        foldUtility = 0f;
        totalUtility = 0f;
        optionsAvailable = 0;
        if (check)
        {
            checkUtility = AIBehaviour.checkValue * (int)AIBehaviour.hand.hands * TableBehaviour.tb.roundCount;
            optionsAvailable++;
            totalUtility += checkUtility;
        }
        if (call)
        {
            callUtility = AIBehaviour.callValue * (int)AIBehaviour.hand.hands * TableBehaviour.tb.roundCount;
            optionsAvailable++;
            totalUtility += callUtility;
        }
        if (bet)
        {
            betUtility = AIBehaviour.betValue * (int)AIBehaviour.hand.hands * TableBehaviour.tb.roundCount;
            optionsAvailable++;
            totalUtility += betUtility;
        }
        if (raise)
        {
            raiseUtility = AIBehaviour.raiseValue * (int)AIBehaviour.hand.hands * TableBehaviour.tb.roundCount;
            optionsAvailable++;
            totalUtility += raiseUtility;
        }
        if (fold)
        {
            foldUtility = ((int)AIBehaviour.hand.hands * -1) + (AIBehaviour.foldValue + FuzzyNot(FuzzyHandValue())/10) * TableBehaviour.tb.roundCount;
            optionsAvailable++;
            totalUtility += foldUtility;
        }



    }

    private void Logic()
    {
        if (!AIBehaviour.boughtin)
        {
            AIBehaviour.bet = true;
            stateFinished = true;
        }

        AIBehaviour.hand = CheckHand.checkHand.BestHand(AIBehaviour.cardHolder.card1, AIBehaviour.cardHolder.card2);
        Options();
        UtilityValues();
        while (true)
        {
            if (stateFinished)
                return;
            float desicion = Random.Range(0, totalUtility);
            float min = 0f;
            min += foldUtility;
            if (desicion <= min && foldUtility != 0)
            {
                AIBehaviour.fold = true;
                Debug.Log("Fold");
                stateFinished = true;
                return;
            }
            min += raiseUtility;
            if (desicion <= min && raiseUtility != 0)
            {
                AIBehaviour.raise = true;
                Debug.Log("Raise");
                stateFinished = true;
                return;
            }
            min += betUtility;
            if (desicion <= min && betUtility != 0)
            {
                AIBehaviour.bet = true;
                Debug.Log("Bet");
                stateFinished = true;
                return;
            }
            min += callUtility;
            if (desicion <= min && callUtility != 0)
            {
                AIBehaviour.call = true;
                Debug.Log("Call");
                stateFinished = true;
                return;
            }
            min += checkUtility;
            if (desicion <= min && checkUtility != 0)
            {
                AIBehaviour.check = true;
                Debug.Log("Check");
                stateFinished = true;
                return;
            }
        }       
    }
}
