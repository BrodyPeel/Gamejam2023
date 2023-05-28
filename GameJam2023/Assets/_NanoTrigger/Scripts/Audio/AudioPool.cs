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

    public AudioSource PlaySound(AudioClip clip, float volume, bool loop, Transform parentTransform)
    {
        if (availableSources.Count == 0)
        {
            Debug.LogWarning("No audio sources available in pool.");
            return null;
        }

        AudioSource source = availableSources.Dequeue();
        source.transform.parent = parentTransform;
        source.gameObject.SetActive(true);
        source.clip = clip;
        source.volume = volume;
        source.loop = loop;
        source.Play();

        if (!loop)
        {
            StartCoroutine(ReturnSourceToPool(source, source.clip.length));
        }

        return source;
    }

    private IEnumerator ReturnSourceToPool(AudioSource source, float delay)
    {
        Debug.Log($"Source will be returned to pool in {delay} seconds. Currently looping: {source.loop}.");
        yield return new WaitForSeconds(delay);
        Debug.Log("Returning source to pool...");
        source.loop = false;
        source.Stop();
        source.gameObject.SetActive(false);
        source.clip = null;
        source.transform.parent = this.transform;
        availableSources.Enqueue(source);
    }



}
