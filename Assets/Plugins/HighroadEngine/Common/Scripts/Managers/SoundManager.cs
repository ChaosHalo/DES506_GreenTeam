using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using System.Linq;
namespace MoreMountains.HighroadEngine
{	
	/// <summary>
	/// This persistent singleton handles sound playing
	/// </summary>
	public class SoundManager : MonoBehaviour
	{
		public enum Type { Music, SFX};
		[SerializeField]
		// true if the music is enabled	
		private bool MusicOn = true;
        [SerializeField]
		// true if the sound fx are enabled
		private bool SfxOn = true;
        /// the music volume
        [Range(0,1)]
		public float MusicVolume=0.3f;
		/// the sound fx volume
		[Range(0,1)]
		public float SfxVolume=1f;

	    protected AudioSource _backgroundMusic;
		private List<AudioSource> musics = new();
        [SerializeField]
		private List<GameObject> sounds = new();
        private void Start()
        {
            
        }
        #region Music
        /// <summary>
        /// Plays a background music.
        /// Only one background music can be active at a time.
        /// </summary>
        /// <param name="Clip">Your audio clip.</param>
        public virtual void PlayBackgroundMusic(AudioSource Music)
		{
			// if(!musics.Contains(Music)) musics.Add(Music);
			// if the music's been turned off, we do nothing and exit
			/*if (!MusicOn)
				return;*/
			// if we already had a background music playing, we stop it

			if (_backgroundMusic != null)
			{
				StopBackGroundMusic();
			}
			// we set the background music clip
			_backgroundMusic = Music;
			// we set the music's volume
			_backgroundMusic.volume = MusicOn ? MusicVolume : 0;
			// we set the loop setting to true, the music will loop forever
			_backgroundMusic.loop = true;
			// we start playing the background music
			_backgroundMusic.Play();
		}

		/// <summary>
		/// Stops the background music.
		/// </summary>
		public virtual void StopBackGroundMusic()
		{
			if (_backgroundMusic != null)
			{
				/*_backgroundMusic.Stop();
				Destroy(_backgroundMusic);*/
				foreach (AudioSource Music in musics)
				{
					Music.Stop();
				}
			}
		}
		public void TurnOnMusic()
		{
			MusicOn = true;
			_backgroundMusic.Play();
		}
		public void TurnOffMusic()
		{
			MusicOn = false;
			_backgroundMusic.Stop();
		}
		public bool IsMusicPlaying() => MusicOn;
        #endregion
        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <returns>An audiosource</returns>
        /// <param name="Sfx">The sound clip you want to play.</param>
        /// <param name="Location">The location of the sound.</param>
        /// <param name="Volume">The volume of the sound.</param>
        public virtual AudioSource PlaySound(AudioClip sfx, Vector3 location, Transform parent, bool shouldDestroyAfterPlay=true, bool pitchShift= false, float minDistance =1, float maxDistance=500, float volume =1)
		{
            if (!SfxOn)
                return null;

            if (sfx == null)
                return null;
			
			// we create a temporary game object to host our audio source
			GameObject temporaryAudioHost = new GameObject("TempAudio");

			sounds.Add(temporaryAudioHost);
			// 设定父物体
			temporaryAudioHost.transform.parent = parent;
			// we set the temp audio's position
			temporaryAudioHost.transform.position = location;
			// we add an audio source to that host
			AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
			// we set that audio source clip to the one in paramaters
			// audioSource.clip = sfx; 
			// we set the audio source volume to the one in parameters
			audioSource.volume = SfxOn ? SfxVolume * volume : 0;
			// 设定近大远小效果
			audioSource.spatialBlend = 1;
            // set min/max distance
			audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
            // set pitch
            audioSource.pitch = 1;
            if (pitchShift == true)
            {
                float newPitch = Random.Range(0.5f, 1.5f);
                audioSource.pitch = newPitch;
            }
            // we start playing the sound
            audioSource.PlayOneShot(sfx); 
			// we destroy the host after the clip has played
			if (shouldDestroyAfterPlay)
			{
				//Destroy(temporaryAudioHost, sfx.length);
			}
			// we return the audiosource reference
			return audioSource;
		}
		
		/// <summary>
		/// Plays a sound
		/// </summary>
		/// <returns>An audiosource</returns>
		/// <param name="Sfx">The sound clip you want to play.</param>
		/// <param name="Location">The location of the sound.</param>
		/// <param name="Volume">The volume of the sound.</param>
		public virtual AudioSource PlayLoop(AudioClip Sfx, Vector3 Location, Transform parent, float minDistance = 1, float maxDistance = 500, float volume=1)
		{
			/*if (!SfxOn)
				return null;*/

			if (Sfx == null)
				return null;

			// we create a temporary game object to host our audio source
			GameObject temporaryAudioHost = new GameObject("TempAudio");

			sounds.Add(temporaryAudioHost);
			// 设定父物体
			temporaryAudioHost.transform.parent = parent;
			// we set the temp audio's position
			temporaryAudioHost.transform.position = Location;
			// we add an audio source to that host
			AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource; 
			// we set that audio source clip to the one in paramaters
			audioSource.clip = Sfx;
            // we set the audio source volume to the one in parameters
            audioSource.volume = SfxOn ? SfxVolume * volume : 0;
            // we set it to loop
            audioSource.loop=true;
			// 设定近大远小效果
			audioSource.spatialBlend = 1;
            // set min/max distance
            audioSource.rolloffMode = AudioRolloffMode.Linear;
            audioSource.minDistance=minDistance;
			audioSource.maxDistance=maxDistance;
			// we start playing the sound
			audioSource.Play(); 
			// we return the audiosource reference
			return audioSource;
        }

        public virtual AudioClip GetRandomClip(List<AudioClip> clipList)
        {
			if (clipList.Count == 0)
				return null;

            return clipList[Random.Range(0, clipList.Count)];
        }
		public void TurnOnSFX()
        {
			sounds = sounds.Where(p => p != null).ToList();
			foreach(var sound in sounds)
            {
				sound.GetComponent<AudioSource>().volume = SfxVolume;
				sound.GetComponent<AudioSource>().mute = false;
			}
			SfxOn = true;
        }
		public void TurnOffSFX()
        {
			sounds = sounds.Where(p => p != null).ToList();
			foreach (var sound in sounds)
			{
				sound.GetComponent<AudioSource>().volume = 0;
				sound.GetComponent<AudioSource>().mute = true;
			}
			SfxOn = false;
		}
		public bool IsSFXPlaying() => SfxOn;
    }
}