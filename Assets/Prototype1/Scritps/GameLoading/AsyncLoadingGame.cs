using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsyncLoadingGame : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Prototype1");
    }
}
