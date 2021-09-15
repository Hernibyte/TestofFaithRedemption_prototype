using UnityEngine;

public class SightPlayer : MonoBehaviour
{
    Vector2 mousePosition;
    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }
}
