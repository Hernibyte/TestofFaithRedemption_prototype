using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckOfCards : MonoBehaviour
{
    public Slot[] slots = new Slot[7];
    public Slot slotCardTaked;
    public Slot None;

    private void Awake()
    {
        slotCardTaked = new Slot();
        for (int i = 0; i < 7; i++)
        {
            slots[i] = new Slot();
        }
    }

    void FixedUpdate()
    {
        //if (slotCardTaked.card == null)
        //    Debug.Log("nulo");
        //else
        //    Debug.Log("no nulo");

        //Debug.Log(slotCardTaked.card);
    }

    public void SetCardTaked(Slot card)
    {
        slotCardTaked = card;
        //Debug.Log(slotCardTaked);
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
