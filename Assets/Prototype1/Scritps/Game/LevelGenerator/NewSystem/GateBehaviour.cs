using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateBehaviour : MonoBehaviour
{
    enum Side
    {
        Left,
        Right,
        Top,
        Down
    }
    [SerializeField] Side side;
    [SerializeField] LayerMask playerMask;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Contains(playerMask, collision.gameObject.layer))
        {
            switch (side)
            {
                case Side.Right:
                    collision.gameObject.transform.position = RePosition(6.5f, 0f, 0f);
                    break;
                case Side.Left:
                    collision.gameObject.transform.position = RePosition(-6.5f, 0f, 0f);
                    break;
                case Side.Down:
                    collision.gameObject.transform.position = RePosition(0f, -6.5f, 0f);
                    break;
                case Side.Top:
                    collision.gameObject.transform.position = RePosition(0f, 6.5f, 0f);
                    break;
            }
        }
    }

    Vector3 RePosition(float x, float y, float z)
    {
        Vector3 newPos = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z + z);
        return newPos;
    }

    bool Contains(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
