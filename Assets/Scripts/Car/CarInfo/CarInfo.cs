using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [field: SerializeField]
    public float TopSpeed { get; set; }
    /// <summary>
    /// 加速度
    /// </summary>
    /// <value></value>
    [field: SerializeField]
    public float Acceleration { get; set; }
    /// <summary>
    /// 操控性
    /// </summary>
    /// <value></value>
    [field: SerializeField]
    public float Handling { get; set; }
    /// <summary>
    /// 克服环境能力
    /// </summary>
    /// <value></value>
    [field: SerializeField]
    public float OffRoad { get; set; }
    public List<int> Rank { get; }
    public List<float> TimeForOneLapList { get; }
}

