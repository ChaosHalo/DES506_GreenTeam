using System.Diagnostics;
using System.Collections;
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
    private EngineSoundScriptableObject engineSound;

    public float RPM;
    public float Load;
    /*private float minRPM => carManager.CarInfoScriptableObject.CarAudioInfo.MinRPM;
    private float maxnRPM => carManager.CarInfoScriptableObject.CarAudioInfo.MaxRPM;*/
    private float minRPM => engineSound.rPM.Min;
    private float maxRPM => engineSound.rPM.Max;
    private float minLoad => engineSound.load.Min;
    private float maxLoad => engineSound.load.Max;

    private bool enableSoundUpdate = true;
    public bool ReversLoadLogic = true;
    public void EnableSoundUpdate() => enableSoundUpdate = true;
    public void DisableSoundUpdate() => enableSoundUpdate = false;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        engineSound = FMODManager.instance.sound;
        solidController = GetComponent<SolidController>();
        StudioEventEmitter = GetComponent<StudioEventEmitter>();
        carManager = GetComponent<CarManager>();

        InitData();
    }

    // Update is called once per frame
    void Update()
    {
        if (enableSoundUpdate)
        {
            SetRPM((maxRPM - minRPM) * solidController.NormalizedSpeed + minRPM);
        }
    }
    private void InitData()
    {
        if (FMODManager.instance.IsFMODOn())
        {
            TurnOnSound();
        }
        else
        {
            TurnOffSound();
        }
    }
    public void SetRPM(float _RPM)
    {
        RPM = _RPM;
        StudioEventEmitter.SetParameter("RPM", _RPM);
    }
    /// <summary>
    /// 最大值时静音
    /// 最小值时最大声
    /// </summary>
    /// <param name="_Load"></param>
    public void SetLoad(float _Load)
    {
        Load = _Load;
        StudioEventEmitter.SetParameter("Load", _Load);
        UnityEngine.Debug.Log("Load" + Load);
    }
    public void TurnOffSound()
    {
        if (ReversLoadLogic)
        {
            SetLoad(minLoad);
        }
        else
        {
            SetLoad(maxLoad);
        }
    }
    public void TurnOnSound()
    {
        if (ReversLoadLogic)
        {
            SetLoad(maxLoad);
        }
        else
        {
            SetLoad(minLoad);
        }
    }
}
