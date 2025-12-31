using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound {
    public AudioClip clip;
    public string name;
    [Range(0f, 1f)]
    public float volume;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
