using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    [SerializeField] private AudioSource _audio;
    [SerializeField] private Slider _slider;

    void Start()
    {
        _slider.value = InitializeGame.GameVolume;
        _audio.volume = _slider.value;
    }

    public void ChangeVolume()
    {
        _audio.volume = _slider.value;
        InitializeGame.GameVolume = _slider.value;
    }
}
