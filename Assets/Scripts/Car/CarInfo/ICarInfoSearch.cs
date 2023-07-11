using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarInfoSearch
{
    /// <summary>
    /// Get the ranking of carName cars
    /// 获取carName车的排名
    /// </summary>
    /// <param name="carName"></param>
    /// <param name="lapIndex"></param>
    /// <returns></returns>
    int GetPlace(string carName);
    /// <summary>
    /// Get the elapsed time of the carName car
    /// 获取carName车的用时
    /// </summary>
    /// <param name="carName"></param>
    /// <param name="lapIndex"></param>
    /// <returns></returns>
    float GetLapTime(string carName);
    /// <summary>
    /// Get the time difference between the firstPlace name and the nextPlace name
    /// 获取第firstPlace名与nextPlace名之间的时间差
    /// </summary>
    /// <param name="firstPlace"></param>
    /// <param name="nextPlace"></param>
    /// <returns></returns>
    float GetGapTime(int firstPlace, int nextPlace);
    /// <summary>
    /// Get the time difference between the firstCarName car and the nextCarName car
    /// 获取firstCarName车与nextCarName车之间的时间差
    /// </summary>
    /// <param name="firstCar"></param>
    /// <param name="nextCar"></param>
    /// <returns></returns>
    float GetGapTime(string firstCarName, string nextCarName);
    /// <summary>
    /// Determine if the firstCarName car finishes the race after the nextCarName car
    /// 判断firstCarName车是否在nextCarName车后完成比赛
    /// </summary>
    /// <param name="firstCarName"></param>
    /// <param name="nextCarName"></param>
    /// <returns></returns>
    bool IsFinishAfter(string firstCarName, string nextCarName);
}
