using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;

public interface IIncidentEffect
{
    void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject);
    static float CalEffect(IncidentEffectScriptableObject incidentEffectScriptableObject, CarManager carManager, float originValue)
    {
        // originValue * ImpactFactor
        return originValue * CalImpactDirection(incidentEffectScriptableObject, carManager);
    }
    static float CalImpactDirection(IncidentEffectScriptableObject incidentEffectScriptableObject, CarManager carManager)
    {
        if (incidentEffectScriptableObject.type == IncidentEffectScriptableObject.Type.Reduce)
        {
            return 1 - CalImpactOffRoad(incidentEffectScriptableObject, carManager);
        }
        else if (incidentEffectScriptableObject.type == IncidentEffectScriptableObject.Type.Increase)
        {
            return 1 + CalImpactOffRoad(incidentEffectScriptableObject, carManager);
        }
        return 1;
    }
    static float CalImpactOffRoad(IncidentEffectScriptableObject incidentEffectScriptableObject, CarManager carManager)
    {
        return incidentEffectScriptableObject.Value * (1 - carManager.GetInitCar().OffRoad);
    }
    static void DebugLog(string carName, string effectName,string effectDescription, string effectValue, string effectObject)
    {
        Debug.Log(carName + " " + effectName + " " + effectDescription + ": " + effectValue + " " + effectObject);
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
        var value = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager, carManager.GetInitCar().Acceleration);
        carManager.SetEngineForce(value);

        IIncidentEffect.DebugLog(
            carManager.CarInfo.Name,
            "DessertEffect",
            incidentEffectScriptableObject.type.ToString(),
            IIncidentEffect.CalImpactOffRoad(incidentEffectScriptableObject, carManager).ToString(),
            "acceleration");
    }
}
/// <summary>
/// Increase 20% speed max
/// </summary>
public class RockyEffect : IIncidentEffect
{
    public void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject)
    {
        var value = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager, carManager.GetInitCar().TopSpeed);
        carManager.SetFullThrottleVelocity(value);
        IIncidentEffect.DebugLog(
            carManager.CarInfo.Name,
            "RockyEffect",
            incidentEffectScriptableObject.type.ToString(),
            IIncidentEffect.CalImpactOffRoad(incidentEffectScriptableObject, carManager).ToString(),
            "speed max");
    }
}

/// <summary>
/// reduce 30% current speed
/// </summary>
public class WaterEffect : IIncidentEffect
{
    public void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject)
    {
        var value = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager, carManager.GetInitCar().TopSpeed);
        carManager.SetFullThrottleVelocity(value);

        var acc = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager, carManager.GetInitCar().Acceleration);
        carManager.SetEngineForce(acc);

        IIncidentEffect.DebugLog(
            carManager.CarInfo.Name,
            "WaterEffect",
            incidentEffectScriptableObject.type.ToString(),
            IIncidentEffect.CalImpactOffRoad(incidentEffectScriptableObject, carManager).ToString(),
            "current speed");
        
    }
}

/// <summary>
/// decrease 30% the handling
/// </summary>
public class SnowEffect : IIncidentEffect
{
    public void EnterEffect(CarManager carManager, IncidentEffectScriptableObject incidentEffectScriptableObject)
    {
        var value = IIncidentEffect.CalEffect(incidentEffectScriptableObject, carManager, carManager.GetInitCar().Handling);
        carManager.SetCarGrip(value);

        IIncidentEffect.DebugLog(
            carManager.CarInfo.Name,
            "SnowEffect",
            incidentEffectScriptableObject.type.ToString(),
            IIncidentEffect.CalImpactOffRoad(incidentEffectScriptableObject, carManager).ToString(),
            "the handling");
    }
}