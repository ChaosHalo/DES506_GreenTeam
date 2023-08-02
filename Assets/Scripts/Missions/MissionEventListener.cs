using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Missions;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }
public class MissionEventListener : MonoBehaviour
{
    public MissionEvent missionEvent;
    public List<MissionEvent> missionEvents;
    public CustomGameEvent response;

    private void OnEnable()
    {
        if (missionEvent != null)
            missionEvent.RegisterListener(this);

        foreach(var e in missionEvents)
            e.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (missionEvent != null)
            missionEvent.UnregisterListener(this);

        foreach (var e in missionEvents)
            e.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender = null, object data = null)
    {
        response.Invoke(sender, data);
    }
}
