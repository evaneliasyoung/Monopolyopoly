using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SpeedSlider : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private Slider _slider;

    void Start()
    {
        _slider.value = InitializeGame.GameSpeed;
    }

    public void ChangeVolume()
    {
        _audio.volume = _slider.value;
        InitializeGame.GameVolume = _slider.value;
    }

    public void ChangeSpeed()
    {
        InitializeGame.GameSpeed = _slider.value;
    }
}
