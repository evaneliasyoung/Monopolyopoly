using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound {
        PassButtonClick,
        BuyButton,
        Roll1,
        Roll2,
        Community1,
        Community2,
        Community3,
        Community4,
        Community5,
        Community6,
        MainGameplayTheme,
        ButtonClick,
        NextButton,
        MoneyBag1,
        MoneyBag2,
        MoneyBag3,
        Crowbar1,
        Crowbar2,
        Crowbar3,
        Tommygun1,
        Tommygun2,
        Tommygun3,
        Bow1,
        Bow2,
        Bow3,
        Chance1,
        Chance2,
        Chance3,
        Chance4,
        Chance5,
        Chance6,
        PrisonCell,
        Roll3,

    }

    public static void PlaySound(Sound sound) {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource= soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    public static void PlaySoundSpecificVolume(Sound sound, float volumeLevel) {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource= soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound), volumeLevel);
    }

    private static AudioClip GetAudioClip (Sound sound) {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray) {
            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
            
        }
        Debug.LogError("Sound " + sound + " not found!"); 
        return null; 
    }
}

