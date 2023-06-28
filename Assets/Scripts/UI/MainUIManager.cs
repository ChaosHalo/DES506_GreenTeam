using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainUIManager : MonoBehaviour
{
    public void TryAgain()
    {
        if (MyGameManager.instance.Map != null)
        {
            Destroy(MyGameManager.instance.Map);
        }
        SceneManager.LoadScene("TrackEditor");
    }
}
