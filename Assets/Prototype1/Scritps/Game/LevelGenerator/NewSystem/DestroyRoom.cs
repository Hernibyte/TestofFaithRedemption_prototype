using UnityEngine;

public class DestroyRoom : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if (!other.CompareTag("Player") && !other.CompareTag("RoomSpace"))
        Destroy(other.gameObject);
    }
}
