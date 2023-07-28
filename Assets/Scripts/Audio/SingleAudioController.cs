using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.HighroadEngine;
public class SingleAudioController : MonoBehaviour
{
    private AudioSource m_AudioSource;
    private SoundManager.Type type;
    //开始
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetType(SoundManager.Type _type)
    {
        type = _type;
    }
    public void SetVolume(float volume)
    {
        m_AudioSource.volume = volume;
    }
    public void Mute()
    {
        m_AudioSource.volume = 0;
    }
    public void ResetVolume(float volume)
    {
        SetVolume(volume);
    }
}
