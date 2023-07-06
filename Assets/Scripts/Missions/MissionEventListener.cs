using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }
public class MissionEventListener : MonoBehaviour
{
    public MissionEvent missionEvent;
    public CustomGameEvent response;

    private void OnEnable()
    {
        missionEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        missionEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender = null, object data = null)
    {
        response.Invoke(sender, data);
    }
}
