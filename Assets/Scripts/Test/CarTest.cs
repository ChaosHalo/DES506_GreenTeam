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
            print(CarInfoSearch.instance.GetPlace(GlobalConstants.BILLY));
            print(CarInfoSearch.instance.GetGapTime(1,2)); 
            print(CarInfoSearch.instance.GetGapTime(GlobalConstants.BILLY, GlobalConstants.SUZIE));
            print(CarInfoSearch.instance.GetLapTime(GlobalConstants.BILLY));
            print(CarInfoSearch.instance.IsFinishAfter(GlobalConstants.BILLY, GlobalConstants.SUZIE));
        }
    }
}
