using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("GameLoading");
    }

    public void Options()
    {
        Debug.Log("Null");
    }

    public void Credits()
    {
        Debug.Log("Null");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
