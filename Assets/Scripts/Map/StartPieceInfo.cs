using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPieceInfo : MonoBehaviour
{
    public Vector3 InitialOrientation => FirstWayPoint.position - CheckPoint.transform.position;
    public Transform FirstWayPoint;
    public List<Transform> StartPoints;
    public GameObject CheckPoint;
}
