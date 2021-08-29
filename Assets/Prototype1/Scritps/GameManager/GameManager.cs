using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public DeckOfCards deck;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);

        instance = this;
    }

    void Start()
    {
        deck = FindObjectOfType<DeckOfCards>();
    }

    public Slot GetCard(int index)
    {
        if(index >= 0 || index < 7)
            return deck.slots[index];
        return null;
    }
}
