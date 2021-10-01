using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardTaked : MonoBehaviour
{
    [SerializeField] Text cardName;
    [SerializeField] DeckOfCards deck;
    public CanvasGroup panel;
    public Slot card;
    public CardType cardImageType;
    StatsMenu stats;
    public bool isOpen;

    private void Start()
    {
        stats = FindObjectOfType<StatsMenu>();
        panel.alpha = .0f;
        panel.interactable = false;
    }

    private void Update()
    {
        if(card != null)
        {
            Card theCardOnself = card.card;

            if (theCardOnself != null)
            {
                if(cardImageType != null)
                    cardImageType.SetCardUtility((CardType.CardUtility)theCardOnself.sCard.myUtility);
                    cardName.text = card.card.sCard.name;
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
        isOpen = false;
        card = null;
        panel.alpha = .0f;
    }

    public void Discard()
    {
        isOpen = false;
        card = null;
        panel.alpha = .0f;
    }
}
