using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{

    public Card card1;
    public Transform card1Position;

    public Card card2;
    public Transform card2Position;

    public void ClearHand()
    {
        card1 = null;
        card2 = null;
    }

}
