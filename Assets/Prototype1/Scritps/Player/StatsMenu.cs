using UnityEngine;

public class StatsMenu : MonoBehaviour
{
    [SerializeField] Animation animations;
    [SerializeField] CanvasGroup canvasIcon;
    [SerializeField] CanvasGroup canvasGStats;

    Animator animator;

    public bool openStats;
    public int flagToOpen;

    bool fadeIn = false;
    bool fadeOut = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        flagToOpen = 0;
        openStats = false;
        canvasIcon.alpha = 0;
        canvasGStats.alpha = 0;
        animations.Play("StatsClose");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            flagToOpen = 0;
            openStats = !openStats;
        }

        if (flagToOpen == 0)
        {
            if(openStats)
            {
                animations.Play("StatsOpen");
                fadeIn = true;
                fadeOut = false;
            }
            else
            {
                animations.Play("StatsClose");
                fadeOut = true;
                fadeIn = false;
            }
            flagToOpen = 1;
        }

        if(fadeIn)
        {
            canvasIcon.alpha += Time.deltaTime * 0.9f;
            canvasGStats.alpha += Time.deltaTime / 1.2f;

            if (canvasIcon.alpha >= 1)
                fadeIn = false;
        }
        else if(fadeOut)
        {
            canvasIcon.alpha -= Time.deltaTime * 2;
            canvasGStats.alpha -= Time.deltaTime * 2;

            if (canvasIcon.alpha <= 0)
                fadeOut = false;
        }
    }
}
