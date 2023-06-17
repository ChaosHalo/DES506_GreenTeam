using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour
{
    public GameObject firstButton;
    public GameObject secondButton;

    public void ToggleButtons()
    {
        if (firstButton.active == true)
        {
            firstButton.SetActive(false);
            secondButton.SetActive(true);
        }
        else
        {
            firstButton.SetActive(true);
            secondButton.SetActive(false);
        }
    }

    public void ResetButtons()
    {
        firstButton.SetActive(true);
        secondButton.SetActive(false);
    }
}
