using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Music { NTLoop, Music2 }
public enum SFX { Select, Fire1, EnemySpawn1, EnemySpawn2, EnemySpawn3, EnemyDie1, EnemyDie2, EnemyDie3 }

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    public AudioSource musicSource;
    public AudioPool SFXPool;

    public float musicVolume;
    public float SFXVolume;

    [System.Serializable]
    public struct MusicClip
    {
        public Music music;
        public AudioClip clip;
    }

    [System.Serializable]
    public struct SFXClip
    {
        public SFX sfx;
        public AudioClip clip;
    }

    public List<MusicClip> musicClips;
    public List<SFXClip> sfxClips;

    private Dictionary<Music, AudioClip> musicDictionary;
    private Dictionary<SFX, AudioClip> sfxDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        musicDictionary = new Dictionary<Music, AudioClip>();
        foreach (MusicClip musicClip in musicClips)
        {
            musicDictionary.Add(musicClip.music, musicClip.clip);
        }

        sfxDictionary = new Dictionary<SFX, AudioClip>();
        foreach (SFXClip sfxClip in sfxClips)
        {
            sfxDictionary.Add(sfxClip.sfx, sfxClip.clip);
        }
    }

    public void PlayMusic(Music music, bool loop = false, float fadeInDuration = 1.0f)
    {
        musicSource.loop = loop;
        StartCoroutine(FadeMusic(musicDictionary[music], fadeInDuration));
    }

    public void PlayMusic(Music music, bool loop = false)
    {
        musicSource.volume = musicVolume;
        musicSource.loop = loop;
        musicSource.clip = musicDictionary[music];
        musicSource.Play();
    }

    private IEnumerator FadeMusic(AudioClip newMusic, float fadeInDuration)
    {
        float fadeOutDuration = fadeInDuration;
        float initialVolume = musicSource.volume;

        while (fadeOutDuration > 0)
        {
            fadeOutDuration -= Time.deltaTime;
            musicSource.volume = initialVolume * fadeOutDuration / fadeInDuration;
            yield return null;
        }

        musicSource.clip = newMusic;
        musicSource.Play();

        while (fadeInDuration > 0)
        {
            fadeInDuration -= Time.deltaTime;
            musicSource.volume = initialVolume * (1 - fadeInDuration / fadeInDuration);
            yield return null;
        }
    }

    public void PlaySFX(SFX sfx, bool loop = false)
    {
        if (sfxDictionary.TryGetValue(sfx, out AudioClip clip))
        {
            SFXPool.PlaySound(clip, SFXVolume, loop);
        }
        else
        {
            Debug.LogWarning("Sound effect not found in the dictionary: " + sfx.ToString());
        }
    }

    public AudioSource PlaySFX(SFX sfx, Vector3 sfxPosition, bool loop = false, float volume = 1f)
    {
        if (sfxDictionary.TryGetValue(sfx, out AudioClip clip))
        {
            AudioSource sfxSource = SFXPool.PlaySound(clip, volume, loop);
            if (sfxSource != null)
            {
                sfxSource.transform.position = sfxPosition;
                sfxSource.spatialBlend = 1.0f; // Set the spatialBlend to 1 for 3D sound.
            }
            return sfxSource;
        }
        else
        {
            Debug.LogWarning("Sound effect not found in the dictionary: " + sfx.ToString());
            return null;
        }
    }

}
