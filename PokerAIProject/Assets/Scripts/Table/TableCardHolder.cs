using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TableCardHolder : CardHolder {

    public Card card3;
    public Transform card3Position;

    public Card card4;
    public Transform card4Position;

    public Card card5;
    public Transform card5Position;

    public TextMeshProUGUI potText;


    public void ClearTable()
    {
        potText.text = "";
        card1 = null;
        card2 = null;
        card3 = null;
        card4 = null;
        card5 = null;
    }
}
