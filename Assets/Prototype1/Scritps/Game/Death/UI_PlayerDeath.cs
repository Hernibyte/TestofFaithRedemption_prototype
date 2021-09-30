using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_PlayerDeath : MonoBehaviour
{
    [SerializeField] GameObject deathPanel;

    private void Start() 
    {
        deathPanel.SetActive(false);
    }

    public void Activate()
    {
        deathPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
