using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTutorialButton : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToHide = new();
    [SerializeField] private List<GameObject> objectsToShow = new();

    public void OnCompleteAction()
    {
        foreach(var  obj in objectsToHide)
            obj.SetActive(false);
        foreach(var  obj in objectsToShow)
            obj.SetActive(true);
    }
}
