using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    [SerializeField] GameObject credits;
    [SerializeField] GameObject options;

    private void Start() 
    {
        credits.SetActive(false);
        options.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("GameLoading");
    }

    public void Options()
    {
        options.SetActive(true);
    }

    public void Credits()
    {
        credits.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }
    
    public void CloseOptions()
    {
        options.SetActive(false);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
    }
}
