using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomClipPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    internal void PlayRandomClip(List<AudioClip> clipList, bool pitchShift = false)
    {
        audioSource.pitch = 1;

        if (pitchShift == true)
        {
            float newPitch = Random.Range(minPitch, maxPitch);
            audioSource.pitch = newPitch;
        }

        AudioClip clipToPlay = clipList[Random.Range(0, clipList.Count)];
        audioSource.PlayOneShot(clipToPlay);
    }

    internal void PlayClip(AudioClip clip, bool pitchShift = false)
    {
        audioSource.pitch = 1;

        if (pitchShift == true)
        {
            float newPitch = Random.Range(minPitch, maxPitch);
            audioSource.pitch = newPitch;
        }

        audioSource.PlayOneShot(clip);
    }
}
