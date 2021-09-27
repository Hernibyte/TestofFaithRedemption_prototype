using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckOfCards : MonoBehaviour
{
    public Slot[] slots = new Slot[7];
    public Text[] texts = new Text[7];
    public Slot slotCardTaked;
    public Slot None;
    [HideInInspector] public bool updateDeck = false;

    public delegate void UpdateDeckOfCards(int indexToUpdate);
    public UpdateDeckOfCards updateSlotOfCard;

    private void Awake()
    {
        slotCardTaked = None;
        for (int i = 0; i < 7; i++)
        {
            slots[i] = None;
        }
    }

    public void SetCardTaked(Slot card)
    {
        slotCardTaked = card;
    }

    public void SetSlot(Card card)
    {
        foreach (Slot slot in slots)
        {
            if(slot != null)
            {
                slot.card = card;
                break;
            }
        }
    }

    public void SetSlot(int index)
    {
        if (slotCardTaked != None)
        {
            slots[index] = slotCardTaked;
            texts[index].text = slotCardTaked.card.sCard.name;
            slotCardTaked = None;
            updateDeck = true;
        }
    }

    public void SetSlotVarient(int index)
    {
        if(slotCardTaked != None)
        {
            slots[index] = slotCardTaked;
            texts[index].text = slotCardTaked.card.newSCard.name;
            slotCardTaked = None;
            updateDeck = true;

            updateSlotOfCard?.Invoke(index);
        }
    }

    public Slot GetSlot(int index)
    {
        return slots[index];
    }

    public Slot[] GetSlots()
    {
        return slots;
    }
}
