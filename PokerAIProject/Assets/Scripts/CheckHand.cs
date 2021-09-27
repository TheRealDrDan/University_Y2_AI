using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHand : MonoBehaviour
{

    public static CheckHand checkHand;

    Hand currentBestHand = new Hand(Hand.Hands.HighCard);

    List<Card> cards = new List<Card>();

    private int length;


    public void Awake()
    {
        checkHand = this;
    }


   public int CardsInHandValue(List<Card> cards)
    {
        int total = 0;
        for (int i = 0; i < cards.Count; i++)
        {
            total += cards[i].value + (int)cards[i].suit; 
        }
        return total;
    }

    public Hand BestHand(Card firstCard, Card secondCard)
    {
        cards = new List<Card>();
        cards.Add(firstCard);
        cards.Add(secondCard);
        if (TableBehaviour.tb.table.card1 != null)
            cards.Add(TableBehaviour.tb.table.card1);
        if (TableBehaviour.tb.table.card2 != null)
            cards.Add(TableBehaviour.tb.table.card2);
        if (TableBehaviour.tb.table.card3 != null)
            cards.Add(TableBehaviour.tb.table.card3);
        if (TableBehaviour.tb.table.card4 != null)
            cards.Add(TableBehaviour.tb.table.card4);
        if (TableBehaviour.tb.table.card5 != null)
            cards.Add(TableBehaviour.tb.table.card5);
        currentBestHand = new Hand(Hand.Hands.HighCard);

        length = cards.Count - 1;

        HighCard();
        SinglePair();
        TwoPair();
        ThreeOfAKind();
        Straight();
        Flush();
        FullHouse();
        FourOfAKind();
        StraightFlush();
        RoyalStraightFlush();


        return currentBestHand;
    }

    private void HighCard()
    {
        Card highCard = new Card(Card.Suits.NA, 0);
        for (int i = 0; i < 2; i++)
        {
            if (cards[i].value > highCard.value)
                highCard = cards[i];
        }
        currentBestHand.hands = Hand.Hands.HighCard;
        currentBestHand.cards = new List<Card>();
        currentBestHand.cards.Add(highCard);
    }


    private void SinglePair()
    {
        List<Card> pairs = new List<Card>();
        int comparrisons = length;
        for (int i = 0; i < length; i++)
        {
            for (int j = i + 1; j <= comparrisons; j++)
            {
                //Debug.Log("Comparing: " + cards[i].suit + cards[i].value + " to " + cards[j].suit + cards[j].value);
                if (cards[i].value == cards[j].value)
                {
                    pairs.Add(cards[i]);
                    pairs.Add(cards[j]);
                }

            }
        }
        if (pairs.Count == 2)
        {
            currentBestHand.hands = Hand.Hands.Pair;
            currentBestHand.cards = new List<Card>();
            currentBestHand.cards.Add(pairs[0]);
            currentBestHand.cards.Add(pairs[1]);
        }
    }


    private void TwoPair()
    {
        List<Card> pairs = new List<Card>();
        int comparrisons = length;
        for (int i = 0; i < length; i++)
        {
            for (int j = i + 1; j <= comparrisons; j++)
            {
                //Debug.Log("Comparing: " + cards[i].suit + cards[i].value + " to " + cards[j].suit + cards[j].value);
                if (cards[i].value == cards[j].value)
                {
                    pairs.Add(cards[i]);
                    pairs.Add(cards[j]);
                }

            }
        }
        if (pairs.Count >= 4)
        {
            Card temp1, temp2;
            bool continueSort = true;
            if (pairs.Count == 2)
            {
                continueSort = false;
            }


            while (continueSort)
            {
                for (int i = 0; i < pairs.Count - 2; i += 2)
                {
                    for (int j = i + 2; j < pairs.Count - 2; j += 2)
                    {
                        if (pairs[i].value > pairs[j].value)
                        {
                            continueSort = true;
                            temp1 = pairs[i];
                            temp2 = pairs[i + 1];
                            pairs[i] = pairs[j];
                            pairs[i + 1] = pairs[j + 1];
                            pairs[j] = temp1;
                            pairs[j + 1] = temp2;
                        }
                        else
                        {
                            continueSort = false;
                        }
                    }
                }
                continueSort = false;
            }
            currentBestHand.hands = Hand.Hands.TwoPair;
            currentBestHand.cards = new List<Card>();
            currentBestHand.cards.Add(pairs[pairs.Count - 2]);
            currentBestHand.cards.Add(pairs[pairs.Count - 1]);
            currentBestHand.cards.Add(pairs[pairs.Count - 4]);
            currentBestHand.cards.Add(pairs[pairs.Count - 3]);
        }
    }

    private void ThreeOfAKind()
    {
        if (length < 3)
            return;
        List<Card> threeOfAKinds = new List<Card>();
        for (int i = 0; i < length; i++)
        {
            for (int j = i + 1; j < (length); j++)
            {
                for (int k = j + 1; k <= length; k++)
                {
                    //Debug.Log("Comparing: " + cards[i].suit + cards[i].value + " to " + cards[j].suit + cards[j].value + " to " + cards[k].suit + cards[k].value);
                    if (cards[i].value == cards[j].value && cards[i].value == cards[k].value)
                    {
                        threeOfAKinds.Add(cards[i]);
                        threeOfAKinds.Add(cards[j]);
                        threeOfAKinds.Add(cards[k]);
                    }
                }
            }
        }
        if (threeOfAKinds.Count > 0)
        {
            currentBestHand.hands = Hand.Hands.ThreeOfAKind;
            currentBestHand.cards = new List<Card>();
            currentBestHand.cards.Add(threeOfAKinds[0]);
            currentBestHand.cards.Add(threeOfAKinds[1]);
            currentBestHand.cards.Add(threeOfAKinds[2]);
        }
    }

    private void Straight()
    {
        if (length < 5)
            return;
        List<Card> orderedByValueCards = new List<Card>();
        List<Card> foundStraights = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
        {
            orderedByValueCards.Add(cards[i]);
        }
        Card temp1;
        bool continueSort = true;
        while (continueSort)
        {
            for (int i = 0; i < orderedByValueCards.Count - 1; i++)
            {
                for (int j = i + 1; j < orderedByValueCards.Count; j++)
                {
                    if (orderedByValueCards[i].value > orderedByValueCards[j].value)
                    {
                        continueSort = true;
                        temp1 = orderedByValueCards[i];
                        orderedByValueCards[i] = orderedByValueCards[j];
                        orderedByValueCards[j] = temp1;
                    }
                    else
                    {
                        continueSort = false;
                    }
                }
            }
        }



        for (int i = 0; i < (length - 3); i++)
        {
            bool isOneMore = true;
            for (int k = i; k < i + 4; k++)
            {
                if (isOneMore)
                {
                    if (orderedByValueCards[k + 1].value - orderedByValueCards[k].value != 1)
                    {
                        isOneMore = false;
                    }
                }
            }
            if (isOneMore)
            {
                foundStraights.Add(orderedByValueCards[i]);
                foundStraights.Add(orderedByValueCards[i + 1]);
                foundStraights.Add(orderedByValueCards[i + 2]);
                foundStraights.Add(orderedByValueCards[i + 3]);
                foundStraights.Add(orderedByValueCards[i + 4]);
            }
        }
        if (foundStraights.Count != 0)
        {
            if (foundStraights.Count > 1 && foundStraights.Count < 6)
            {
                currentBestHand.hands = Hand.Hands.Straight;
                currentBestHand.cards = new List<Card>();
                currentBestHand.cards.Add(foundStraights[0]);
                currentBestHand.cards.Add(foundStraights[1]);
                currentBestHand.cards.Add(foundStraights[2]);
                currentBestHand.cards.Add(foundStraights[3]);
                currentBestHand.cards.Add(foundStraights[4]);
            }
            else
            {
                int indexForBestStraight = 0;
                for (int i = 0; i < foundStraights.Count; i += 5)
                {
                    if (foundStraights[i].value > foundStraights[indexForBestStraight].value)
                        indexForBestStraight = i;
                }
                currentBestHand.hands = Hand.Hands.Straight;
                currentBestHand.cards = new List<Card>();
                currentBestHand.cards.Add(foundStraights[indexForBestStraight]);
                currentBestHand.cards.Add(foundStraights[indexForBestStraight + 1]);
                currentBestHand.cards.Add(foundStraights[indexForBestStraight + 2]);
                currentBestHand.cards.Add(foundStraights[indexForBestStraight + 3]);
                currentBestHand.cards.Add(foundStraights[indexForBestStraight + 4]);
            }
        }
    }

    private void Flush()
    {
        if (length < 5)
            return;
        List<Card> orderedByValueCards = new List<Card>();

        List<Card> heartCards = new List<Card>();
        List<Card> clubCards = new List<Card>();
        List<Card> diamondCards = new List<Card>();
        List<Card> spadeCards = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
        {
            orderedByValueCards.Add(cards[i]);
        }
        Card temp1;
        bool continueSort = true;
        while (continueSort)
        {
            for (int i = 0; i < orderedByValueCards.Count - 1; i++)
            {
                for (int j = i + 1; j < orderedByValueCards.Count; j++)
                {
                    if (orderedByValueCards[i].value > orderedByValueCards[j].value)
                    {
                        continueSort = true;
                        temp1 = orderedByValueCards[i];
                        orderedByValueCards[i] = orderedByValueCards[j];
                        orderedByValueCards[j] = temp1;
                    }
                    else
                    {
                        continueSort = false;
                    }
                }
            }
        }
        for (int i = 0; i < orderedByValueCards.Count; i++)
        {
            switch (orderedByValueCards[i].suit)
            {
                case Card.Suits.H:
                    heartCards.Add(orderedByValueCards[i]);
                    break;
                case Card.Suits.C:
                    clubCards.Add(orderedByValueCards[i]);
                    break;
                case Card.Suits.D:
                    diamondCards.Add(orderedByValueCards[i]);
                    break;
                case Card.Suits.S:
                    spadeCards.Add(orderedByValueCards[i]);
                    break;
            }
        }
        if (heartCards.Count >= 5)
        {
            currentBestHand.hands = Hand.Hands.Flush;
            currentBestHand.cards = new List<Card>();
            currentBestHand.cards.Add(heartCards[heartCards.Count - 1]);
            currentBestHand.cards.Add(heartCards[heartCards.Count - 2]);
            currentBestHand.cards.Add(heartCards[heartCards.Count - 3]);
            currentBestHand.cards.Add(heartCards[heartCards.Count - 4]);
            currentBestHand.cards.Add(heartCards[heartCards.Count - 5]);
        }
        else if (clubCards.Count >= 5)
        {
            currentBestHand.hands = Hand.Hands.Flush;
            currentBestHand.cards = new List<Card>();
            currentBestHand.cards.Add(clubCards[clubCards.Count - 1]);
            currentBestHand.cards.Add(clubCards[clubCards.Count - 2]);
            currentBestHand.cards.Add(clubCards[clubCards.Count - 3]);
            currentBestHand.cards.Add(clubCards[clubCards.Count - 4]);
            currentBestHand.cards.Add(clubCards[clubCards.Count - 5]);
        }
        else if (diamondCards.Count >= 5)
        {
            currentBestHand.hands = Hand.Hands.Flush;
            currentBestHand.cards = new List<Card>();
            currentBestHand.cards.Add(diamondCards[diamondCards.Count - 1]);
            currentBestHand.cards.Add(diamondCards[diamondCards.Count - 2]);
            currentBestHand.cards.Add(diamondCards[diamondCards.Count - 3]);
            currentBestHand.cards.Add(diamondCards[diamondCards.Count - 4]);
            currentBestHand.cards.Add(diamondCards[diamondCards.Count - 5]);
        }
        else if (spadeCards.Count >= 5)
        {
            currentBestHand.hands = Hand.Hands.Flush;
            currentBestHand.cards = new List<Card>();
            currentBestHand.cards.Add(spadeCards[spadeCards.Count - 1]);
            currentBestHand.cards.Add(spadeCards[spadeCards.Count - 2]);
            currentBestHand.cards.Add(spadeCards[spadeCards.Count - 3]);
            currentBestHand.cards.Add(spadeCards[spadeCards.Count - 4]);
            currentBestHand.cards.Add(spadeCards[spadeCards.Count - 5]);
        }
    }

    private void FullHouse()
    {
        if (length < 5)
            return;
        List<Card> threeOfAKinds = new List<Card>();
        List<Card> cardsToCheck = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
        {
            cardsToCheck.Add(cards[i]);
        }

        for (int i = 0; i < length; i++)
        {
            for (int j = i + 1; j < (length); j++)
            {
                for (int k = j + 1; k <= length; k++)
                {
                    if (cardsToCheck[i].value == cardsToCheck[j].value && cardsToCheck[i].value == cardsToCheck[k].value)
                    {
                        threeOfAKinds.Add(cardsToCheck[i]);
                        threeOfAKinds.Add(cardsToCheck[j]);
                        threeOfAKinds.Add(cardsToCheck[k]);
                    }
                }
            }
        }
        if (threeOfAKinds.Count > 0)
        {
            cardsToCheck.Remove(threeOfAKinds[0]);
            cardsToCheck.Remove(threeOfAKinds[1]);
            cardsToCheck.Remove(threeOfAKinds[2]);
            List<Card> pairs = new List<Card>();
            int comparrisons = cardsToCheck.Count;
            for (int i = 0; i < cardsToCheck.Count; i++)
            {
                for (int j = i + 1; j < comparrisons; j++)
                {
                    if (cardsToCheck[i].value == cardsToCheck[j].value)
                    {
                        pairs.Add(cardsToCheck[i]);
                        pairs.Add(cardsToCheck[j]);
                    }

                }
            }
            if (pairs.Count == 2)
            {
                currentBestHand.hands = Hand.Hands.FullHouse;
                currentBestHand.cards = new List<Card>();
                currentBestHand.cards.Add(threeOfAKinds[0]);
                currentBestHand.cards.Add(threeOfAKinds[1]);
                currentBestHand.cards.Add(threeOfAKinds[2]);
                currentBestHand.cards.Add(pairs[0]);
                currentBestHand.cards.Add(pairs[1]);
            }
            else if (pairs.Count > 4)
            {
                Card temp1, temp2;
                bool continueSort = true;
                while (continueSort)
                {
                    for (int i = 0; i < pairs.Count - 2; i += 2)
                    {
                        for (int j = i + 2; j < pairs.Count - 2; j += 2)
                        {
                            if (pairs[i].value > pairs[j].value)
                            {
                                continueSort = true;
                                temp1 = pairs[i];
                                temp2 = pairs[i + 1];
                                pairs[i] = pairs[j];
                                pairs[i + 1] = pairs[j + 1];
                                pairs[j] = temp1;
                                pairs[j + 1] = temp2;
                            }
                            else
                            {
                                continueSort = false;
                            }
                        }
                    }
                }

                currentBestHand.hands = Hand.Hands.FullHouse;
                currentBestHand.cards = new List<Card>();
                currentBestHand.cards.Add(threeOfAKinds[0]);
                currentBestHand.cards.Add(threeOfAKinds[1]);
                currentBestHand.cards.Add(threeOfAKinds[2]);
                currentBestHand.cards.Add(pairs[0]);
                currentBestHand.cards.Add(pairs[1]);
            }


        }
    }


    private void FourOfAKind()
    {
        if (length < 4)
            return;
        List<Card> fourOfAKinds = new List<Card>();
        for (int i = 0; i < length; i++)
        {
            for (int j = i + 1; j < length; j++)
            {
                for (int k = j + 1; k < length; k++)
                {
                    for (int o = k + 1; o <= length; o++)
                    {
                        if (cards[i].value == cards[j].value && cards[i].value == cards[k].value && cards[k].value == cards[o].value)
                        {
                            fourOfAKinds.Add(cards[i]);
                            fourOfAKinds.Add(cards[j]);
                            fourOfAKinds.Add(cards[k]);
                            fourOfAKinds.Add(cards[o]);
                        }
                    }
                }
            }
        }
        if (fourOfAKinds.Count > 0)
        {
            currentBestHand.hands = Hand.Hands.FourOfAKind;
            currentBestHand.cards = new List<Card>();
            currentBestHand.cards.Add(fourOfAKinds[0]);
            currentBestHand.cards.Add(fourOfAKinds[1]);
            currentBestHand.cards.Add(fourOfAKinds[2]);
            currentBestHand.cards.Add(fourOfAKinds[3]);
        }
    }

    private void StraightFlush()
    {
        if (length < 5)
            return;
        List<Card> orderedByValueCards = new List<Card>();
        List<Card> foundStraights = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
        {
            orderedByValueCards.Add(cards[i]);
        }
        Card temp1;
        bool continueSort = true;
        while (continueSort)
        {
            for (int i = 0; i < orderedByValueCards.Count - 1; i++)
            {
                for (int j = i + 1; j < orderedByValueCards.Count; j++)
                {
                    if (orderedByValueCards[i].value > orderedByValueCards[j].value)
                    {
                        continueSort = true;
                        temp1 = orderedByValueCards[i];
                        orderedByValueCards[i] = orderedByValueCards[j];
                        orderedByValueCards[j] = temp1;
                    }
                    else
                    {
                        continueSort = false;
                    }
                }
            }
        }
        for (int i = 0; i < (length - 3); i++)
        {
            bool isOneMore = true;
            for (int k = i; k < i + 4; k++)
            {
                if (isOneMore)
                {
                    if (orderedByValueCards[k + 1].value - orderedByValueCards[k].value != 1)
                    {
                        isOneMore = false;
                    }
                }
            }
            if (isOneMore)
            {
                foundStraights.Add(orderedByValueCards[i]);
                foundStraights.Add(orderedByValueCards[i + 1]);
                foundStraights.Add(orderedByValueCards[i + 2]);
                foundStraights.Add(orderedByValueCards[i + 3]);
                foundStraights.Add(orderedByValueCards[i + 4]);
            }
        }
        if (foundStraights.Count != 0)
        {
            List<Card> foundStraightFlush = new List<Card>();
            for (int i = 0; i < foundStraights.Count; i += 5)
            {
                bool isSameSuit = true;
                for (int k = i; k < i + 5; k++)
                {
                    if (foundStraights[i].suit != foundStraights[k].suit)
                        isSameSuit = false;
                }
                if (isSameSuit)
                {
                    for (int k = i; k < i + 5; k++)
                    {
                        foundStraightFlush.Add(foundStraights[k]);
                    }
                }

            }

            if (foundStraightFlush.Count >= 1)
            {
                currentBestHand.hands = Hand.Hands.StraightFlush;
                currentBestHand.cards = new List<Card>();
                currentBestHand.cards.Add(foundStraightFlush[0]);
                currentBestHand.cards.Add(foundStraightFlush[1]);
                currentBestHand.cards.Add(foundStraightFlush[2]);
                currentBestHand.cards.Add(foundStraightFlush[3]);
                currentBestHand.cards.Add(foundStraightFlush[4]);
            }
        }
    }

    private void RoyalStraightFlush()
    {
        if (length < 5)
            return;
        List<Card> orderedByValueCards = new List<Card>();
        List<Card> foundStraights = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
        {
            orderedByValueCards.Add(cards[i]);
        }
        Card temp1;
        bool continueSort = true;
        while (continueSort)
        {
            for (int i = 0; i < orderedByValueCards.Count - 1; i++)
            {
                for (int j = i + 1; j < orderedByValueCards.Count; j++)
                {
                    if (orderedByValueCards[i].value > orderedByValueCards[j].value)
                    {
                        continueSort = true;
                        temp1 = orderedByValueCards[i];
                        orderedByValueCards[i] = orderedByValueCards[j];
                        orderedByValueCards[j] = temp1;
                    }
                    else
                    {
                        continueSort = false;
                    }
                }
            }
        }
        for (int i = 0; i < (length - 3); i++)
        {
            bool isOneMore = true;
            for (int k = i; k < i + 4; k++)
            {
                if (isOneMore)
                {
                    if (orderedByValueCards[k + 1].value - orderedByValueCards[k].value != 1)
                    {
                        isOneMore = false;
                    }
                }
            }
            if (isOneMore)
            {
                foundStraights.Add(orderedByValueCards[i]);
                foundStraights.Add(orderedByValueCards[i + 1]);
                foundStraights.Add(orderedByValueCards[i + 2]);
                foundStraights.Add(orderedByValueCards[i + 3]);
                foundStraights.Add(orderedByValueCards[i + 4]);
            }
        }
        if (foundStraights.Count != 0)
        {
            List<Card> foundStraightFlush = new List<Card>();
            for (int i = 0; i < foundStraights.Count; i += 5)
            {
                bool isSameSuit = true;
                for (int k = i; k < i + 5; k++)
                {
                    if (foundStraights[i].suit != foundStraights[k].suit)
                        isSameSuit = false;
                }
                if (isSameSuit)
                {
                    for (int k = i; k < i + 5; k++)
                    {
                        foundStraightFlush.Add(foundStraights[k]);
                    }
                }

            }

            if (foundStraightFlush.Count >= 1 && foundStraightFlush[0].value == 10)
            {
                currentBestHand.hands = Hand.Hands.StraightFlush;
                currentBestHand.cards = new List<Card>();
                currentBestHand.cards.Add(foundStraightFlush[0]);
                currentBestHand.cards.Add(foundStraightFlush[1]);
                currentBestHand.cards.Add(foundStraightFlush[2]);
                currentBestHand.cards.Add(foundStraightFlush[3]);
                currentBestHand.cards.Add(foundStraightFlush[4]);
            }
        }
    }
}
