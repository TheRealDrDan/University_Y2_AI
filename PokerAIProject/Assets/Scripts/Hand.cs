using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Hand
{
    public enum Hands { HighCard = 1, Pair = 2, TwoPair = 3, ThreeOfAKind = 4, Straight = 5, Flush = 6, FullHouse = 7, FourOfAKind = 8, StraightFlush = 9, RoyalFlush = 10 };

    public Hands hands;
    public List<Card> cards;

    public Hand(Hands handType)
    {
        hands = handType;
    }
}
