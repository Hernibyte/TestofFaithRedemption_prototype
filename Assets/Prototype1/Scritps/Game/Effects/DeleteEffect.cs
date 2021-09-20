using UnityEngine;

public class DeleteEffect : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1.0f);
    }
}
