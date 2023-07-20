using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BackToMenuUIManager : MonoBehaviour
{
    public UnityEvent UnityEvent;
    private void OnEnable()
    {
        Invoke("EventInovke", 0.2f);
    }
    void EventInovke()
    {
        UnityEvent?.Invoke();
    }
}
