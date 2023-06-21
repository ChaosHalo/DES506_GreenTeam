using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CarInfo", menuName = "Data/Car", order = 0)]
public class CarInfoScriptableObject : ScriptableObject
{
    public CarInfo CarInfo;
    public CarInfo Init()
    {
        return CarInfo;
    }
}