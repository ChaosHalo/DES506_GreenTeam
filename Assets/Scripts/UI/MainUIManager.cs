using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lovatto.MiniMap;
public class MainUIManager : MonoBehaviour
{
    public void TryAgain()
    {
        ClearAllCars();
        if (MyGameManager.instance.Map != null)
        {
            //Destroy(MyGameManager.instance.Map);
        }
        MyGameManager.instance.GetSceneManager().ChangeToState(CustomSceneManager.Index.BUILD);
        FindObjectOfType<CurrencyManager>().AddWinCurrency();
        
    }
    void ClearAllCars()
    {
        CarManager[] CarManager = FindObjectsOfType<CarManager>();
        // 清除小地图UI
        bl_MiniMapIcon[] bl_MiniMapIcons = FindObjectsOfType<bl_MiniMapIcon>();
        if(bl_MiniMapIcons.Length > 0)
        {
            Debug.Log("已清除小地图UI");
        }
        foreach (var i in bl_MiniMapIcons)
        {
            Destroy(i.gameObject);
        }
        foreach (var Car in CarManager)
        {
            Destroy(Car.gameObject);
        }
    }
}
