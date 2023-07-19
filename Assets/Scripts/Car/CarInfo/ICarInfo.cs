using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 车辆属性值
/// </summary>
public interface ICarInfo
{
    string Name { get; }
    int TopSpeed { get; }
    int Acceleration { get; }
    int Handling { get; }
    int OffRoad { get; }
    // 运行数据列表

    /// <summary>
    /// 名次
    /// </summary>
    /// <value></value>
    List<int> Rank { get; }
    /// <summary>
    /// 车辆运行一圈所需秒数
    /// </summary>
    /// <value></value>
    List<float> TimeForOneLapList { get; }
}
/// <summary>
/// 车辆实际值
/// </summary>
public interface ICarReality
{
    float TopSpeed { get; }
    float Acceleration { get; }
    float Handling { get; }
    float OffRoad { get; }
}