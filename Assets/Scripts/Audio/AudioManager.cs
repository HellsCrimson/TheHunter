using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
        
        foreach (Sound curSound in sounds)
        {
            curSound.source = gameObject.AddComponent<AudioSource>();
            curSound.source.clip = curSound.clip;
            curSound.source.volume = curSound.volume;
            curSound.source.loop = curSound.loopSound;
            curSound.source.outputAudioMixerGroup = curSound.group;
        }
    }

    public void PlaySound(string soundName)
    {
        Sound curSound = Array.Find(sounds, sound => sound.name == soundName);
        if (curSound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        curSound.source.Play();
    }
    
    public void StopSound (string soundName)
    {
        Sound curSound = Array.Find(sounds, sound => sound.name == soundName);
        if (curSound == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        curSound.source.Stop ();
    }
}
