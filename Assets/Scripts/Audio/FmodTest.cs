using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FmodTest : MonoBehaviour
{
    private FMOD.Studio.EventInstance eventInstance;
    private FMOD.Studio.PARAMETER_ID parameterId;
    private FMODUnity.StudioEventEmitter StudioEventEmitter;
    public float RPM;
    public float Load;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        StudioEventEmitter = GetComponent<StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        /*eventInstance.setParameterByName("RPM", RPM);
        eventInstance.setParameterByName("Load", Load);*/
        StudioEventEmitter.SetParameter("RPM", RPM);
    }
}
