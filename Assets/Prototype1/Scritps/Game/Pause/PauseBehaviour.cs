using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBehaviour : MonoBehaviour
{
    [SerializeField] GameObject pause;

    private void Start() 
    {
        pause.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0f;
            pause.SetActive(true);
        }
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        pause.SetActive(false);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
