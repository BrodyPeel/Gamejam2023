using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Music { SSLoop, Music2 }
public enum SFX { Select, Cancel }

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    public AudioSource musicSource;
    public AudioSource SFXSource;

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
            DontDestroyOnLoad(gameObject);
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

    public void PlayMusic(Music music, float fadeInDuration = 1.0f)
    {
        StartCoroutine(FadeMusic(musicDictionary[music], fadeInDuration));
    }

    public void PlayMusic(Music music)
    {
        musicSource.volume = 1f;
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

    public void PlaySFX(SFX sfx, float volume = 1f)
    {
        if (sfxDictionary.TryGetValue(sfx, out AudioClip clip))
        {
            SFXSource.volume = volume;
            SFXSource.clip = clip;
            SFXSource.Play();
        }
        else
        {
            Debug.LogWarning("Sound effect not found in the dictionary: " + sfx.ToString());
        }
    }

    public AudioSource PlaySFX(SFX sfx, Vector3 sfxPosition, float volume = 1f)
    {
        AudioSource sfxSource = new GameObject("SFX_" + sfx.ToString()).AddComponent<AudioSource>();
        sfxSource.transform.position = sfxPosition;
        sfxSource.clip = sfxDictionary[sfx];
        sfxSource.volume = volume;
        sfxSource.spatialBlend = 1.0f; // Set the spatialBlend to 1 for 3D sound.
        sfxSource.Play();

        // Destroy the AudioSource after it finishes playing the clip.
        Destroy(sfxSource.gameObject, sfxSource.clip.length);

        return sfxSource;
    }
}


