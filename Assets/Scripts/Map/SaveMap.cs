using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Linq;
public class SaveMap : MonoBehaviour
{
    public ObjectPlacer objectPlacer;
    public void OnSaveMap()
    {
        // 创建一个空的父物体
        GameObject map = new GameObject("Map");
        // 去空
        List<GameObject> pieces = objectPlacer.placedObjects.Where(o => o != null).ToList();
        // 只筛选轨道
        //List<GameObject> Tracks = pieces.Where(o => !o.name.Contains("Terrain")).ToList();

        // 将列表中的每个GameObject设置为父物体的子物体
        foreach (GameObject obj in pieces)
        {
            obj.transform.parent = map.transform;
        }

        MyGameManager.instance.Map = map;
        //SaveGameObject(map);
    }
    //private void SaveGameObject(GameObject objectToSave)
    //{
    //    // 创建一个新的Prefab Asset
    //    GameObject prefab = PrefabUtility.SaveAsPrefabAsset(objectToSave, "Assets/Resources/SaveMap/Map.prefab");

    //    if (prefab != null)
    //    {
    //        Debug.Log("GameObject saved as prefab: " + prefab.name);
    //    }
    //    else
    //    {
    //        Debug.LogError("Failed to save GameObject as prefab.");
    //    }
    //}
}
