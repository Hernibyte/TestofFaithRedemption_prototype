using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckOfCards : MonoBehaviour
{
    public Slot[] slots = new Slot[7];
    public Slot slotCardTaked;
    public Slot None;
    public bool updateDeck = false;

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
        slots[index] = slotCardTaked;
        updateDeck = true;
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
