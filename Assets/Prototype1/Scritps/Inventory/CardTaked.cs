using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTaked : MonoBehaviour
{
    [SerializeField] DeckOfCards deck;
    public Slot card;
    StatsMenu stats;

    private void Start()
    {
        stats = FindObjectOfType<StatsMenu>();
    }

    public void TakeCard()
    {
        if (!stats.openStats)
        {
            stats.flagToOpen = 0;
            stats.openStats = !stats.openStats;
        }
        deck.SetCardTaked(card);
        card = null;
    }

    public void Discard()
    {
        card = null;
    }
}
