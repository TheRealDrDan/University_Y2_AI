using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public enum Suits { H = 1, S = 4, C = 2, D = 3, NA = 0 };

    public Suits suit;

    public int value;

    public Card(Suits Suit, int Value)
    {
        suit = Suit;
        value = Value;
    }

}
