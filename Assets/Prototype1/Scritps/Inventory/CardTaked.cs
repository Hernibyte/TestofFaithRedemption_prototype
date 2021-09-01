using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardTaked : MonoBehaviour
{
    [SerializeField] DeckOfCards deck;
    public CanvasGroup panel;
    public Slot card;
    StatsMenu stats;

    private void Start()
    {
        stats = FindObjectOfType<StatsMenu>();
        panel.alpha = .0f;
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

        panel.alpha = .0f;
    }

    public void Discard()
    {
        card = null;
    }
}
