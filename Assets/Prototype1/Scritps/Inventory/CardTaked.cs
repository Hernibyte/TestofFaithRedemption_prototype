using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTaked : MonoBehaviour
{
    [SerializeField] DeckOfCards deck;
    [SerializeField] Slot card;

    public void TakeCard()
    {
        deck.SetCardTaked(card);
    }
}
