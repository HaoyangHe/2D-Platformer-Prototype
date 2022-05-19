using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    
    public AudioClip clip;

    public bool loop;
    
    [Range(0.0f, 1.0f)] public float volume = 1;
    [Range(0.1f, 3.0f)] public float pitch = 1;

    [HideInInspector] public AudioSource source;
}
