using UnityEngine;
using FMODUnity;

public class FMODAudioController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public string fmodEventPath;

    private FMOD.Studio.EventInstance fmodEventInstance;
    private float maxDistance = Mathf.Infinity;

    private void Start()
    {
        // 创建FMOD音频实例
        fmodEventInstance = RuntimeManager.CreateInstance(fmodEventPath);
        fmodEventInstance.start();

        // 设置最大距离为无穷大
        SetMaxDistance(maxDistance);
    }

    public void SetMaxDistance(float maxDistance)
    {
        // 设置最大距离
        this.maxDistance = maxDistance;
        fmodEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        fmodEventInstance.setProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, this.maxDistance);
    }
}
