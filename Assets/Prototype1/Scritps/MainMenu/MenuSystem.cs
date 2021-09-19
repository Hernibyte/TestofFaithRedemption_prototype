using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Prototype1");
    }

    public void Options()
    {
        Debug.Log("Null");
    }

    public void Credits()
    {
        Debug.Log("Null");
    }
}
