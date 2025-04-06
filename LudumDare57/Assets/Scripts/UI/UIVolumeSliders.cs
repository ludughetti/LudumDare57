using Audio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIVolumeSliders : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;

        [Header("Audio")]
        [SerializeField] private AudioConfig buttonAudio;

        [Header("Sliders")]
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private string _masterVolume;
        private string _musicVolume;
        private string _sfxVolume;

        private Dictionary<Button, SoundButtons> _buttonIcons;

        private void OnEnable()
        {
            _masterVolume = audioManager.MasterVolume;
            _musicVolume = audioManager.MusicVolume;
            _sfxVolume = audioManager.SFXVolume;
        }

        private void Start()
        {
            masterSlider.value = PlayerPrefs.GetFloat(_masterVolume, 1);
            musicSlider.value = PlayerPrefs.GetFloat(_musicVolume, 1);
            sfxSlider.value = PlayerPrefs.GetFloat(_sfxVolume, 1);

            masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        }

        private void UpdateMasterVolume(float value)
        {
            audioManager.SetMasterVolume(value);
            PlayerPrefs.SetFloat(_masterVolume, value);
        }

        private void UpdateMusicVolume(float value)
        {
            audioManager.SetMusicVolume(value);
            PlayerPrefs.SetFloat(_musicVolume, value);
        }

        private void UpdateSFXVolume(float value)
        {
            audioManager.SetSFXVolume(value);
            PlayerPrefs.SetFloat(_sfxVolume, value);
        }

        [Serializable]
        public struct SoundButtons
        {
            [field: SerializeField] public Sprite ButtonOnIcon { get; set; }
            [field: SerializeField] public Sprite ButtonOffIcon { get; set; }
        }
    }
}
