using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicIntro;
    public AudioSource musicLoop;

    public float crossFadeDuration = 0.5f;
    public float crossFadeOffset = 0.5f;
    public float volume = 1;

    private void Start()
    {
        StartCoroutine(MusiqueAndLoop());
    }

    IEnumerator MusiqueAndLoop()
    {
        musicIntro.volume = volume;
        yield return 0;
        if (musicIntro.clip != null)
        {
            musicIntro.Play();
            Debug.Log("musicIntro.clip.length "+ -crossFadeDuration + " crossFadeDuration "+ musicIntro.clip.length );
            yield return new WaitForSeconds(musicIntro.clip.length - crossFadeDuration - crossFadeOffset);
        }
        else
        {
            yield return 0;
        }

        musicLoop.volume = 0;
        musicLoop.Play();
        float lerp = 0;
        while (lerp < 1)
        {
            lerp += Time.deltaTime / crossFadeDuration;
            musicIntro.volume = volume * (1-lerp);
            musicLoop.volume = volume * lerp;
            yield return 0;
        }
        musicLoop.volume = volume;
    }
}
