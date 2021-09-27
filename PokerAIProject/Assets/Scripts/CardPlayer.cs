using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardPlayer : MonoBehaviour {

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI roleText;
    public TextMeshProUGUI betText;

    public TableBehaviour.Role role = TableBehaviour.Role.NA;
    public bool playerFolded = false;

    [HideInInspector]
    public CardHolder cardHolder;
    [HideInInspector]
    public Hand hand = new Hand(Hand.Hands.HighCard);

    public float money;
    public float currentBet;

    [HideInInspector]
    public bool boughtin = false;

    [HideInInspector]
    public bool canPlay = false;

    protected int previousPlayer = 0;


    public void resetToPlay()
    {
        boughtin = false;
        currentBet = 0f;
        playerFolded = false;
        betText.text = "";
        role = TableBehaviour.Role.NA;
        roleText.text = "";
        cardHolder.ClearHand();
    }

    public void Play()
    {
        if (TableBehaviour.tb.IsOnePlayerLeft())
            return;
        canPlay = true;
        Debug.Log(gameObject.name + "'s turn.");
        previousPlayer = TableBehaviour.tb.ReturnPreviousPlayer();
    }

    public void DecreaseMoney(float value)
    {
        money -= value;
        boughtin = true;
        moneyText.text = money.ToString();
    }

    public void IncreaseMoney(float value)
    {
        money += value;
        moneyText.text = money.ToString();
    }

    public void Fold()
    {
        playerFolded = true;
        TableBehaviour.tb.RemovePlayerFromMatch(this);
    }

    public Hand myHand()
    {
        hand = CheckHand.checkHand.BestHand(cardHolder.card1, cardHolder.card2);
        return hand;
    }
}
