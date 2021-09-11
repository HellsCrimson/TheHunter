using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    
    [Range(0.0001f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
    
    public bool loopSound;
    
    public AudioMixerGroup group;
}
