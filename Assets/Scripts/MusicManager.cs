using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicIntro;
    public AudioSource musicLoop;

    private void Start()
    {
        StartCoroutine(MusiqueAndLoop());
    }

    IEnumerator MusiqueAndLoop()
    {
        if(musicIntro.clip != null)
        {
            musicIntro.Play();
            yield return new WaitForSeconds(musicIntro.clip.length);
        }
        else
        {
            yield return 0;
        }
        musicLoop.Play();
    }
}
