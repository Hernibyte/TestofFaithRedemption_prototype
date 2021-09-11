﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardTaked : MonoBehaviour
{
    [SerializeField] DeckOfCards deck;
    public CanvasGroup panel;
    public Slot card;
    public CardType cardImageType;
    StatsMenu stats;

    private void Start()
    {
        stats = FindObjectOfType<StatsMenu>();
        panel.alpha = .0f;
    }

    private void Update()
    {
        if(card != null)
        {
            Card theCardOnself = card.card;

            if (theCardOnself != null)
            {
                Debug.Log("ENTRO A TIPO CARTA");
                cardImageType.SetCardUtility((CardType.CardUtility)theCardOnself.sCard.myType);
            }
        }
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
        panel.alpha = .0f;
    }
}
