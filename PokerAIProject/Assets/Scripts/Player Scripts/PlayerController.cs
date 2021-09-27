using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : CardPlayer
{
    public GameObject UI;


    public GameObject check;
    public GameObject call;
    public GameObject raise;
    public GameObject bet;
    public GameObject fold;
    

    private void Awake()
    {
        cardHolder = GetComponent<CardHolder>();
        moneyText.text = money.ToString();
        money = TableBehaviour.tb.startMoney;       
    }

    private void Update()
    {
        if (!canPlay)
            return;
        if(canPlay)
        {
            HideSelectOptions();
            ShowHideALLOptions(true);
            canPlay = false;
        }
    }    
    private void EndPlay()
    {
        if (currentBet > 0)
            boughtin = true;
        ShowAllSelectOptions();
        ShowHideALLOptions(false);
        TableBehaviour.tb.turnFinished = true;
    }

    public void AddMoneyToTable(float value)
    {
        TableBehaviour.tb.AddToPot(value);
        currentBet += value;
        betText.text = currentBet.ToString();
    }

    private void HideSelectOptions()
    {
        if((role == TableBehaviour.Role.SB) && !boughtin)
        {
            ShowAllSelectOptions();
            check.SetActive(false);
            call.SetActive(false);
            raise.SetActive(false);
        }
        else if ((role == TableBehaviour.Role.BB) && !boughtin)
        {
            ShowAllSelectOptions();
            check.SetActive(false);
            call.SetActive(false);
            raise.SetActive(false);
        }
        else
        {
            if (TableBehaviour.tb.players[previousPlayer].currentBet == currentBet)
            {
                call.SetActive(false);
                raise.SetActive(false);
            }
            else
            {
                check.SetActive(false);
                bet.SetActive(false);
            }
        }
    }

    private void ShowAllSelectOptions()
    {
        check.SetActive(true);
        bet.SetActive(true);
        call.SetActive(true);
        raise.SetActive(true);
    }

    private void ShowHideALLOptions(bool enable)
    {
        UI.SetActive(enable);
    }

    public void Bet()
    {
        if((role == TableBehaviour.Role.SB) && !boughtin)
        {
            DecreaseMoney(TableBehaviour.tb.minBet/2);
            AddMoneyToTable(TableBehaviour.tb.minBet/2);
            EndPlay();
            return;
        }     
        else
        {          
            DecreaseMoney(TableBehaviour.tb.minBet);
            AddMoneyToTable(TableBehaviour.tb.minBet);
        }
        EndPlay();
    }

    public void Raise()
    {
        DecreaseMoney(TableBehaviour.tb.minBet*2);
        AddMoneyToTable(TableBehaviour.tb.minBet*2);
        EndPlay();
    }

    public void Call()
    {
        float difference;
        difference = TableBehaviour.tb.players[TableBehaviour.tb.ReturnPreviousPlayer()].currentBet - currentBet;
        DecreaseMoney(difference);
        AddMoneyToTable(difference);
        EndPlay();
    }

    public void Check()
    {
        EndPlay();
    }

    public void PlayerFold()
    {
        Fold();
        EndPlay();
    }



}
