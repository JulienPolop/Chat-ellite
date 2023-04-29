using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioMixer masterMixer;
    public AudioSource musicIntro;
    public AudioSource musicLoop;

    public float crossFadeDuration = 0.5f;
    public float crossFadeOffset = 0.5f;
    [Range(0,1)]
    public float volume = 1;
    public AnimationCurve volumeCurve;

    private void Start()
    {
        StartCoroutine(MusiqueAndLoop());
    }

    public void Update()
    {
        masterMixer.SetFloat("Volume", volumeCurve.Evaluate(volume));
    }

    IEnumerator MusiqueAndLoop()
    {
        musicIntro.volume = 1;
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
            musicIntro.volume = 1 * (1-lerp);
            musicLoop.volume = 1 * lerp;
            yield return 0;
        }
        musicLoop.volume = 1;
    }
}
