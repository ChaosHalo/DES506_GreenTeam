using UnityEngine;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.HighroadEngine
{
    /// <summary>
    /// Add this class to a GameObject to play a background music when instanciated.
    /// Only one background music will be played at a time.
    /// </summary>
    public class BackgroundMusic : MonoBehaviour
    {
        /// the sound clip to play
        public AudioClip BuildClip;
        public AudioClip RaceClip;

        protected AudioSource buildSource;
        protected AudioSource raceSource;

        protected SoundManager _soundManager;

        /// <summary>
        /// Gets the AudioSource associated to that GameObject, and asks the GameManager to play it.
        /// </summary>
        public virtual void Start()
        {
            _soundManager = FindObjectOfType<SoundManager>();

            if (_soundManager == null)
            {
                Debug.LogWarning("BackgroundMusic need a SoundManager gameObject in the scene to play music. Please add one.");
                return;
            }
            PlayBuildSource();
        }
        public void PlayBuildSource()
        {
            buildSource = TryAddSource(buildSource, BuildClip);
            _soundManager.PlayBackgroundMusic(buildSource);
        }
        public void PlayRaceSource()
        {
            raceSource = TryAddSource(raceSource, RaceClip);
            _soundManager.PlayBackgroundMusic(raceSource);
        }

        protected AudioSource TryAddSource(AudioSource _source, AudioClip clip)
        {
            if (_source != null) return _source;
            _source = gameObject.AddComponent<AudioSource>() as AudioSource;
            _source.playOnAwake = false;
            _source.spatialBlend = 0;
            _source.rolloffMode = AudioRolloffMode.Logarithmic;
            _source.loop = true;
            _source.clip = clip;

            return _source;
        }
    }
}