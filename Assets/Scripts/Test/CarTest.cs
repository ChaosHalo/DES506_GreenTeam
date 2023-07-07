using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            print(CarInfoSerach.instance.GetRank(GlobalConstants.BILLY));
            print(CarInfoSerach.instance.GetGapTime(1,2)); 
            print(CarInfoSerach.instance.GetGapTime(GlobalConstants.BILLY, GlobalConstants.SUZIE));
            print(CarInfoSerach.instance.GetLapTime(GlobalConstants.BILLY));
            print(CarInfoSerach.instance.IsFinishAfter(GlobalConstants.BILLY, GlobalConstants.SUZIE));
        }
    }
}
