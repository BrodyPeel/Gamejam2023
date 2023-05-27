using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AudioPool : MonoBehaviour
{
    public AudioSource prefab;
    public int poolSize = 5;

    private Queue<AudioSource> availableSources;

    private void Awake()
    {
        availableSources = new Queue<AudioSource>(poolSize);

        for (int i = 0; i < poolSize; i++)
        {
            AudioSource newSource = Instantiate(prefab, transform);
            newSource.gameObject.SetActive(false);
            availableSources.Enqueue(newSource);
        }
    }

    public void PlaySound(AudioClip clip, float volume, bool loop)
    {
        if (availableSources.Count == 0)
        {
            Debug.LogWarning("No audio sources available in pool.");
            return;
        }

        AudioSource source = availableSources.Dequeue();
        source.gameObject.SetActive(true);
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.Play();

        StartCoroutine(ReturnSourceToPool(source, clip.length));
    }


    private IEnumerator ReturnSourceToPool(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!source.loop)
        {
            source.gameObject.SetActive(false);
            source.clip = null;

            availableSources.Enqueue(source);
        }
    }

}
