using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    [SerializeField] private float splashDuration = 1.5f;

    internal IEnumerator RunSplashScreen()
    {
        yield return new WaitForSeconds(splashDuration);
        LoadNewScene(1);
    }

    public void LoadNewScene(int index)
    {
        SceneManager.LoadScene(index);
        MyGameManager.instance.SetNewState(index);
    }

    public void TEMPloadscene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
