using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MissionEvent")]
public class MissionEvent : ScriptableObject
{
    public List<MissionEventListener> listeners = new List<MissionEventListener>();

    public void Raise(Component sender=null, object data=null)
    {
        for (int i = 0; i < listeners.Count; i++) 
        {
            listeners[i].OnEventRaised(sender, data);
        }
    }

    public void RegisterListener(MissionEventListener listener)
    {
        if(!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(MissionEventListener listener)
    {
        if(listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
