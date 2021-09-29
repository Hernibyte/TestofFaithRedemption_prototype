using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBehaviour : MonoBehaviour
{
    [SerializeField] LayerMask playerMask;
    [SerializeField] List<Slot> slotsElection;
    CardTaked taked;

    private void Start()
    {
        taked = FindObjectOfType<CardTaked>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Contains(playerMask, collision.gameObject.layer))
        {
            if (Input.GetKey(KeyCode.E))
            {
                taked.panel.alpha = 1f;
                taked.panel.interactable = true;

                taked.isOpen = true;
                taked.card = slotsElection[Random.Range(0, (slotsElection.Count - 1))];

                Destroy(this.gameObject);
            }
        }
    }

    bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
