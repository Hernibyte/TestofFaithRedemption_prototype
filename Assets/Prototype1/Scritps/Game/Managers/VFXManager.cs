using UnityEngine;

public class VFXManager : MonoBehaviour
{
    CameraShake cameraShakeHandler;

    static VFXManager instance;
    public static VFXManager Get()
    {
        return instance;
    }
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        cameraShakeHandler = gameObject.GetComponent<CameraShake>();
    }
    public void ShakeScreen(float duration, float amplitud)
    {
        StartCoroutine(cameraShakeHandler.Shake(duration, amplitud));
    }
}
