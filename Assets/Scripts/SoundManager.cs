using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    public AudioSource effectSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

    public void PlaySound(Sound sound) {
        /*
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource= soundGameObject.AddComponent<AudioSource>();
        */
        effectSource.PlayOneShot(GetAudioClip(sound));
    }

    public void PlaySoundSpecificVolume(Sound sound, float volumeLevel) {
        /*
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource= soundGameObject.AddComponent<AudioSource>();
        */
        effectSource.PlayOneShot(GetAudioClip(sound), volumeLevel);
    }

    public void MiscSound(string type)
    {
        switch (type)
        {
            case "jail":
                PlaySoundSpecificVolume(SoundManager.Sound.PrisonCell, 0.6f);
                break;
            case "buy":
                PlaySoundSpecificVolume(SoundManager.Sound.ButtonClick, 0.1f);
                PlaySoundSpecificVolume(SoundManager.Sound.BuyButton, 0.8f);
                break;
            case "roll":
                int n = (int)(Random.value * 3f);
                if (n == 0)
                {
                    PlaySoundSpecificVolume(SoundManager.Sound.Roll1, 0.8f);
                }
                if (n == 1)
                {
                    PlaySoundSpecificVolume(SoundManager.Sound.Roll2, 0.8f);
                }
                if (n == 2)
                {
                    PlaySoundSpecificVolume(SoundManager.Sound.Roll3, 0.8f);
                }
                break;
            case "next":
                PlaySound(SoundManager.Sound.NextButton);
                break;
            case "pass":
                PlaySoundSpecificVolume(SoundManager.Sound.PassButtonClick, 0.8f);
                break;
        }
    }

    public void CommunitySound(string pulledCard)
    {
        switch (pulledCard)
        {
            case "community1":
                PlaySoundSpecificVolume(SoundManager.Sound.Community1, 0.5f);
                break;
            case "community2":
                PlaySoundSpecificVolume(SoundManager.Sound.Community2, 0.5f);
                break;
            case "community3":
                PlaySoundSpecificVolume(SoundManager.Sound.Community3, 0.5f);
                break;
            case "community4":
                PlaySoundSpecificVolume(SoundManager.Sound.Community4, 0.5f);
                break;
            case "community5":
                PlaySoundSpecificVolume(SoundManager.Sound.Community5, 0.5f);
                break;
            case "community6":
                PlaySoundSpecificVolume(SoundManager.Sound.Community6, 0.5f);
                break;
            case "chance1": // Loan sharks
                PlaySoundSpecificVolume(SoundManager.Sound.Chance1, 0.5f);
                break;
            case "chance2": //Advance to Go 
                PlaySoundSpecificVolume(SoundManager.Sound.Chance2, 0.5f);
                break;
            case "chance3": //Green Door Brewery
                PlaySoundSpecificVolume(SoundManager.Sound.Chance3, 0.35f);
                break;
            case "chance4": //Carnello's Pizzaria
                PlaySoundSpecificVolume(SoundManager.Sound.Chance4, 0.4f);
                break;
            case "chance5": //Laundromat
                PlaySoundSpecificVolume(SoundManager.Sound.Chance5, 0.25f);
                break;
            case "chance6": //Go back 3 spaces
                PlaySoundSpecificVolume(SoundManager.Sound.Chance6, 0.4f);
                break;
        }
    }

    public void PlayPlayerSound(int playerNumber)
    {
        int n = (int)(Random.value * 3f);
        //Piece Specific Movement Sounds
        switch (playerNumber)
        {
            case 0: //MoneyToken
                if (n == 0)
                    PlaySoundSpecificVolume(SoundManager.Sound.MoneyBag1, 0.3f);
                if (n == 1)
                    PlaySoundSpecificVolume(SoundManager.Sound.MoneyBag2, 0.3f);
                if (n == 2)
                    PlaySoundSpecificVolume(SoundManager.Sound.MoneyBag3, 0.3f);
                break;
            case 1: //CrowbarToken
                if (n == 0)
                    PlaySoundSpecificVolume(SoundManager.Sound.Crowbar1, 0.3f);
                if (n == 1)
                    PlaySoundSpecificVolume(SoundManager.Sound.Crowbar2, 0.3f);
                if (n == 2)
                    PlaySoundSpecificVolume(SoundManager.Sound.Crowbar3, 0.3f);
                break;
            case 2: //TommygunToken
                if (n == 0)
                    PlaySoundSpecificVolume(SoundManager.Sound.Tommygun1, 0.2f);
                if (n == 1)
                    PlaySoundSpecificVolume(SoundManager.Sound.Tommygun2, 0.2f);
                if (n == 2)
                    PlaySoundSpecificVolume(SoundManager.Sound.Tommygun3, 0.2f);
                break;
            case 3: //BowToken
                if (n == 0)
                    PlaySoundSpecificVolume(SoundManager.Sound.Bow1, 0.4f);
                if (n == 1)
                    PlaySoundSpecificVolume(SoundManager.Sound.Bow2, 0.4f);
                if (n == 2)
                    PlaySoundSpecificVolume(SoundManager.Sound.Bow3, 0.4f);
                break;
        }
    }

    private AudioClip GetAudioClip (Sound sound) {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.i.soundAudioClipArray) {
            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
            
        }
        Debug.LogError("Sound " + sound + " not found!"); 
        return null; 
    }
}