﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using MoreMountains.HighroadEngine;
public class FMODCarController : MonoBehaviour
{
    private FMOD.Studio.EventInstance eventInstance;
    private FMOD.Studio.PARAMETER_ID parameterId;
    private FMODUnity.StudioEventEmitter StudioEventEmitter;
    private SolidController solidController;
    private CarManager carManager;
    public float RPM;
    public float Load;

    private float minRPM => carManager.CarInfoScriptableObject.CarAudioInfo.MinRPM;
    private float maxnRPM => carManager.CarInfoScriptableObject.CarAudioInfo.MaxRPM;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        solidController = GetComponent<SolidController>();
        StudioEventEmitter = GetComponent<StudioEventEmitter>();
        carManager = GetComponent<CarManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RPM = (maxnRPM - minRPM) * solidController.NormalizedSpeed + minRPM;
        StudioEventEmitter.SetParameter("RPM", RPM);
    }
}