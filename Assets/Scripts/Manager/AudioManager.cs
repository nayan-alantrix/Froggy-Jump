using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
     [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource soundEffect;
    [SerializeField] private List<AudioData> sounds;

    private void Start()
    {
        PlayMusic(SoundType.Music);
    }

    public void PlayMusic(SoundType soundType)
    {
        AudioClip clip = GetSoundClip(soundType);
        if (clip != null)
        {
            music.clip = clip;
            music.Play();
        }
        else
        {
            Debug.Log("Audio clip not found for " + soundType);
        }
    }

    public void PlaySFX(SoundType soundType)
    {
        AudioClip clip = GetSoundClip(soundType);
        if (clip != null)
        {
            soundEffect.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("Audio clip not found for " + soundType);
        }

    }

    private AudioClip GetSoundClip(SoundType soundType)
    {
        AudioData audio = sounds.Find(x => x.soundType == soundType);
        if (audio != null)
        {
            return audio.soundClip;
        }        
        return null;
    }
}

[Serializable]
public class AudioData
{
    public SoundType soundType;
    public AudioClip soundClip;
}

public enum SoundType   
{
    Music,
    ButtonClick,
    GameStart,
    CarPull,
    LevelWin,
    LevelLose
}