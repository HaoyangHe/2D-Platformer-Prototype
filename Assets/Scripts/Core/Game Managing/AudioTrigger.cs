using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private bool playOnAwake;              // Begins playing sound immediately without the player triggering the collider
    [SerializeField] private bool loop;
    [SerializeField] private bool triggerOnce;              // If set to true, the sound will only play once if triggered
    [SerializeField] private bool controlsTitle;            // This allows the level title to fade in while also fading in the music
    [SerializeField] private float fadeSpeed;               // How fast do we increase the volume? If set to 0, it will just play at a volume of 1
    [SerializeField][Range(0, 1)] private float maxVolume;  // The volume we are going to fade to
    [SerializeField] private AudioClip sound;
    
    private AudioSource audioSource;
    
    private bool hasPlayed;                                 // Is set to true once the sound plays
    private bool triggered;                                 // Is set to true once the player touches the collider trigger zone
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.loop = loop;
        audioSource.volume = fadeSpeed != 0 ? 0 : maxVolume;

        StartCoroutine(EnableCollider());
    }

    void Update()
    {
        if (triggered || playOnAwake)
        {
            if (!hasPlayed && !audioSource.isPlaying)
            {
                audioSource.Play();
                hasPlayed = true;
            }

            if (audioSource.volume < maxVolume && fadeSpeed != 0)
            {
                audioSource.volume += fadeSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (audioSource.volume > 0 && fadeSpeed != 0)
            {
                audioSource.volume -= fadeSpeed * Time.deltaTime;
            }
            else if (fadeSpeed == 0)
            {
                audioSource.Stop();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (!triggered && collision.gameObject == GameManager.Instance.player.gameObject)
        {
            if (controlsTitle)
            {
                // Control titles here
            }

            triggered = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Instance.player.gameObject)
        {
            triggered = false;

            if (!triggerOnce)
            {
                hasPlayed = false;
            }
        }
    }

    private IEnumerator EnableCollider()
    {
        // If the player spawns inside a large trigger area, it won't trigger.
        // Therefore, we wait 4 seconds to actually enable it so the trigger can actually occur.
        
        yield return new WaitForSeconds(4f);
        GetComponent<BoxCollider2D>().enabled = true;
    }
}