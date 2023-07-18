using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;

public interface IIncidentEffect
{
    void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject);
    static float CalEffect(IncidentEffectScriptableObject incidentEffectScriptableObject, float originValue)
    {
        if(incidentEffectScriptableObject.type == IncidentEffectScriptableObject.Type.Reduce)
        {
            return originValue * (1 - incidentEffectScriptableObject.Value);
        }
        else if(incidentEffectScriptableObject.type == IncidentEffectScriptableObject.Type.Increase)
        {
            return originValue * (1 + incidentEffectScriptableObject.Value);
        }
        return originValue;
    }
    static void DebugLog(string message)
    {
        Debug.Log(message);
    }
}

public class GrassEffect : IIncidentEffect
{
    public void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject)
    {
        throw new System.NotImplementedException();
    }

}

/// <summary>
/// reduce 30% acceleration
/// </summary>
public class DessertEffect : IIncidentEffect
{
    public void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject)
    {
        var value = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager.GetInitCar().Acceleration);
        carManager.SetEngineForce(value);
        IIncidentEffect.DebugLog(carManager.CarInfo.Name + " DessertEffect: reduce " + incidentEffectScriptableObject.Value + " acceleration");
    }
}
/// <summary>
/// Increase 20% speed max
/// </summary>
public class RockyEffect : IIncidentEffect
{
    public void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject)
    {
        var value = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager.GetInitCar().TopSpeed);
        carManager.SetFullThrottleVelocity(value);
        IIncidentEffect.DebugLog(carManager.CarInfo.Name + " RockyEffect: Increase " + incidentEffectScriptableObject.Value + " speed max");
    }
}

/// <summary>
/// reduce 30% current speed
/// </summary>
public class WaterEffect : IIncidentEffect
{
    public void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject)
    {
        var value = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager.GetInitCar().TopSpeed);
        carManager.SetFullThrottleVelocity(value);

        var acc = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager.GetInitCar().Acceleration);
        carManager.SetEngineForce(acc);

        IIncidentEffect.DebugLog(carManager.CarInfo.Name + " WaterEffect: reduce " + incidentEffectScriptableObject.Value + " current speed");
    }
}

/// <summary>
/// decrease 30% the handling
/// </summary>
public class SnowEffect : IIncidentEffect
{
    public void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject)
    {
        var value = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager.GetInitCar().Handling);
        carManager.SetCarGrip(value);
        IIncidentEffect.DebugLog(carManager.CarInfo.Name + " SnowEffect: decrease" + incidentEffectScriptableObject.Value + " the handling");
    }
}