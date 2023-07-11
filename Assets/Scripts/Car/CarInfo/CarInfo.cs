using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 车辆属性值
/// 一共10个等级
/// </summary>
[System.Serializable]
public class CarInfo : ICarInfo
{
    /// <summary>
    /// 名称
    /// </summary>
    /// <value></value>
    [field: SerializeField]
    public string Name { get; set; }
    /// <summary>
    /// 最大速度
    /// </summary>
    /// <value></value>
    [field: SerializeField, Range(GlobalConstants.MIN_LEVEL, GlobalConstants.MAX_LEVEL)]
    public int TopSpeed { get; set; }
    /// <summary>
    /// 加速度
    /// </summary>
    /// <value></value>
    [field: SerializeField, Range(GlobalConstants.MIN_LEVEL, GlobalConstants.MAX_LEVEL)]
    public int Acceleration { get; set; }
    /// <summary>
    /// 操控性
    /// </summary>
    /// <value></value>
    [field: SerializeField, Range(GlobalConstants.MIN_LEVEL, GlobalConstants.MAX_LEVEL)]
    public int Handling { get; set; }
    /// <summary>
    /// 克服环境能力
    /// </summary>
    /// <value></value>
    [field: SerializeField, Range(GlobalConstants.MIN_LEVEL, GlobalConstants.MAX_LEVEL)]
    public int OffRoad { get; set; }
    public List<int> Rank { get; }
    public List<float> TimeForOneLapList { get; }

}

public class Car : ICarReality
{
    public float TopSpeed { get; private set; }
    public float Acceleration { get; private set; }
    public float Handling { get; private set; }
    public float OffRoad { get; private set; }

    public Car()
    {
        TopSpeed = 0;
        Acceleration = 0;
        Handling = 0;
        OffRoad = 0;
    }
    // 构造函数
    public Car(float topSpeed, float acceleration, float handling, float offRoad)
    {
        TopSpeed = topSpeed;
        Acceleration = acceleration;
        Handling = handling;
        OffRoad = offRoad;
    }
    public Car(FactorsBaseObject factorsBaseObject, CarInfo carInfo)
    {
        TopSpeed = factorsBaseObject.Get(factorsBaseObject.TopSpeed, carInfo.TopSpeed);
        Acceleration = factorsBaseObject.Get(factorsBaseObject.Acceleration, carInfo.Acceleration);
        Handling = factorsBaseObject.Get(factorsBaseObject.Handling, carInfo.Handling);
        OffRoad = factorsBaseObject.Get(factorsBaseObject.OffRoad, carInfo.OffRoad);
    }
}

[System.Serializable]
public class CarUIInfo
{
    public Sprite PlaceholderCarImage;
    public Sprite BackGroundImage;
}
