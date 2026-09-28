using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class sound_and_music : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    public AudioClip musicClip;
    public AudioClip fight_music;
    public AudioClip attack_sound;
    public AudioClip walk_sound;
    public AudioClip dad_death_sound;
    public AudioClip mom_death_sound;
    public AudioClip dog_death_sound;

    private void Awake()
    {
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        if (soundSource == null)
        {
            soundSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        StartMusic(musicClip);
    }

    public void StartMusic(AudioClip musicClip)
    {
        if (musicClip == null)
        {
            return;
        }

        musicSource.clip = musicClip;
        musicSource.loop = true;
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (musicSource != null && musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void PlaySound(AudioClip soundClip)
    {
        if (soundClip == null || soundSource == null)
        {
            return;
        }

        soundSource.PlayOneShot(soundClip);
    }
    
    public IEnumerator Walking_Sound()
    {
        for(int i = 0; i < 2; i++)
        {
            PlaySound(walk_sound);
            yield return new WaitForSeconds(walk_sound.length);
        }
    }
}
