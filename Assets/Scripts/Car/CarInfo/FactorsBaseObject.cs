using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 汽车各参数基数表格
/// 计算公式为y = ax + b
/// 固定为10个等级，故只修改每个汽车参数的最大和最小值
/// </summary>
[CreateAssetMenu(fileName = "Factor", menuName = "Data/Factor", order = 0)]
public class FactorsBaseObject : ScriptableObject
{
    [System.Serializable]
    public struct Extreme
    {
        public float Min;
        public float Max;
    }
    public Extreme TopSpeed;
    public Extreme Acceleration;
    public Extreme Handling;
    public Extreme OffRoad;
    /// <summary>
    /// 获取当前level下的对应汽车参数返回值
    /// </summary>
    /// <param name="e"></param>
    /// <param name="Level"></param>
    /// <returns></returns>
    public float Get(Extreme e, int level)
    {
        return CalculateLinearFunction(e.Min, e.Max, level);
    }
    /// <summary>
    /// y = ax + b
    /// </summary>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private static float CalculateLinearFunction(float minValue, float maxValue, int level)
    {
        float diff = GlobalConstants.MAX_LEVEL - GlobalConstants.MIN_LEVEL;
        float a = (maxValue - minValue) / diff; // 计算斜率 a
        float b = minValue - a; // 计算截距 b

        return a * level + b;
    }
}
