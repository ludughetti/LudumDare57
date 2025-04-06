using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioDataSource selfDataSource;
        [SerializeField] private LevelManager levelManager;

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer mainMixer;
        [SerializeField] private AudioMixerGroup musicGroup;
        [SerializeField] private AudioMixerGroup sfxGroup;

        [Header("Audio Configs")]
        [SerializeField] private AudioConfig mainMusic;
        [SerializeField] private AudioConfig winAudio;
        [SerializeField] private AudioConfig loseAudio;

        [Header("Audio Names")]
        [SerializeField] private string masterVolume = "MasterVolume";
        [SerializeField] private string musicVolume = "MusicVolume";
        [SerializeField] private string sfxVolume = "SFXVolume";

        private Dictionary<AudioClip, AudioSource> _audioSources = new();

        public string MasterVolume => masterVolume;
        public string MusicVolume => musicVolume;
        public string SFXVolume => sfxVolume;

        private void Awake()
        {
            selfDataSource.DataInstance = this;
        }

        private void OnEnable()
        {
            levelManager.triggerEndgame += HandleEndGameSFX;

            SetMasterVolume(1f);
            SetMusicVolume(1f);
            SetSFXVolume(1f);

            PlayAudio(mainMusic);
        }

        private void OnDisable()
        {
            levelManager.triggerEndgame -= HandleEndGameSFX;
        }

        public void PlayAudio(AudioConfig audioConfig)
        {
            if (audioConfig == null || audioConfig.Clip == null) return;

            AudioSource source = GetOrCreateAudioSource(audioConfig);
            source.Play();
        }

        private AudioSource GetOrCreateAudioSource(AudioConfig audioConfig)
        {
            if (_audioSources.TryGetValue(audioConfig.Clip, out AudioSource existingSource))
                return existingSource;

            AudioSource newSource = CreateNewAudioSource(audioConfig);
            _audioSources[audioConfig.Clip] = newSource;

            return newSource;
        }

        private AudioSource CreateNewAudioSource(AudioConfig audioConfig)
        {
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.clip = audioConfig.Clip;
            newSource.loop = audioConfig.Loop;
            newSource.volume = audioConfig.Volume;
            newSource.playOnAwake = false;

            newSource.outputAudioMixerGroup = audioConfig.IsMusic ? musicGroup : sfxGroup;

            return newSource;
        }

        private void HandleEndGameSFX(bool hasWon)
        {
            if(hasWon)
                PlayAudio(winAudio);

            else
                PlayAudio(loseAudio);
        }

        public void SetMasterVolume(float volume) => mainMixer.SetFloat(masterVolume, ConvertToDecibels(volume));
        public void SetMusicVolume(float volume) => mainMixer.SetFloat(musicVolume, ConvertToDecibels(volume));
        public void SetSFXVolume(float volume) => mainMixer.SetFloat(sfxVolume, ConvertToDecibels(volume));

        private float ConvertToDecibels(float volume)
        {
            return volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        }
    }
}