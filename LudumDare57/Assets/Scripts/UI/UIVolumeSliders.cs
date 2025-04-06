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

        private void Start()
        {
            masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
            musicSlider.onValueChanged.AddListener(UpdateMusicVolume);
            sfxSlider.onValueChanged.AddListener(UpdateSFXVolume);
        }

        private void UpdateMasterVolume(float value)
        {
            audioManager.SetMasterVolume(value);
        }

        private void UpdateMusicVolume(float value)
        {
            audioManager.SetMusicVolume(value);
        }

        private void UpdateSFXVolume(float value)
        {
            audioManager.SetSFXVolume(value);
        }

        [Serializable]
        public struct SoundButtons
        {
            [field: SerializeField] public Sprite ButtonOnIcon { get; set; }
            [field: SerializeField] public Sprite ButtonOffIcon { get; set; }
        }
    }
}
