using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance = null;
    public bool firstLoad = true;

    public AudioMixer masterMixer;
    public AudioSource musicMenu;
    public AudioSource musicIntro;
    public AudioSource musicLoop;
    public AudioSource musicLoose;

    Coroutine loopCoroutine = null;

    public float menuFadeDuration = 0.2f;
    public float crossFadeDuration = 0.5f;
    public float crossFadeOffset = 0.5f;
    public float crossLostDuration = 0.2f;
    public float backMenuDuration = 0.2f;
    [Range(0,1)]
    public float volume = 1;
    public AnimationCurve volumeCurve;


    [SerializeField] private AudioSource alien_crush;
    public void CrushAlien()
    {
        alien_crush.PlayOneShot(alien_crush.clip);
    }
    [SerializeField] private AudioSource cook;
    public void Cook()
    {
        cook.PlayOneShot(cook.clip);
    }
    [SerializeField] private AudioSource canon;
    public void Shoot()
    {
        canon.PlayOneShot(canon.clip);
    }
    [SerializeField] private AudioSource canon_empty;
    public void Shoot_Empty()
    {
        canon_empty.PlayOneShot(canon_empty.clip);
    }
    [SerializeField] private AudioSource feedShip;
    public void FeedShip()
    {
        if(feedShip != null)
            feedShip.PlayOneShot(feedShip.clip);
    }
    [SerializeField] private AudioSource boingPlanet;
    public void HitPlanet()
    {
        if (boingPlanet != null)
            boingPlanet.PlayOneShot(boingPlanet.clip);
    }


    //public AudioSource motor;
    //public void CrushAlien()
    //{
    //    alien_crush.PlayOneShot(alien_crush.clip);
    //}


    private void Awake()
    {
        if(instance != null)
        {
            if (LevelManager.instance != null)
                LevelManager.instance.music = instance; //in case the ref break
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        firstLoad = true;
        transform.SetParent(null);
        DontDestroyOnLoad(this.gameObject);

        Screen.fullScreen = true;

        if (Menu.instance != null)
        {
            //Lunch menu music
            Debug.Log("Launch !", musicMenu.gameObject);
            musicMenu.Play();
        }
        else
        {
            Debug.Log("Coroutine !");
            loopCoroutine = StartCoroutine(MusiqueAndLoop());
        }
    }

    public void Update()
    {
        masterMixer.SetFloat("Volume", volumeCurve.Evaluate(volume));


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.fullScreen = false;
        }
    }

    IEnumerator MusiqueAndLoop()
    {
        musicIntro.volume = 1;
        yield return 0;
        if (musicIntro.clip != null)
        {
            musicIntro.Play();
            musicLoop.Stop();
            while (musicIntro.isPlaying)
            {
                yield return 0;
            }          
        }
        else
        {
            yield return 0;
        }

        musicIntro.Stop();
        musicLoop.volume = 1;
        musicLoop.Play();
        loopCoroutine = null;
    }


    [ContextMenu("_MenuToGame")]
    public void MenuToGame()
    {
        StartCoroutine(MenuToGame_Corout());
    }
    [ContextMenu("_LostGame")]
    public void LostGame()
    {
        StartCoroutine(LostGame_Corout());
    }
    [ContextMenu("_ReplayGame")]
    public void ReplayGame()
    {
        StartCoroutine(ReplayGame_Corout());
    }
    [ContextMenu("_GameToMenu")]
    public void GameToMenu()
    {
        StopAllCoroutines();
        StartCoroutine(GameToMenu_Corout());
    }

    IEnumerator MenuToGame_Corout()
    {
        firstLoad = false;
        if (loopCoroutine != null)
            StopCoroutine(loopCoroutine);

        float lerp = musicMenu.volume;
        while(lerp > 0)
        {
            lerp -= Time.deltaTime / menuFadeDuration;
            musicMenu.volume = lerp;
            yield return 0;
        }
        musicMenu.volume = 0;
        musicMenu.Stop();
        loopCoroutine = StartCoroutine(MusiqueAndLoop());
    } 

    IEnumerator LostGame_Corout()
    {
        if (loopCoroutine != null)
            StopCoroutine(loopCoroutine);

        float intVol = musicIntro.volume;
        float lopVol = musicLoop.volume;

        musicMenu.volume = 0;//in case you loose REALLY quickly
        musicLoose.volume = 0;
        musicLoose.Play();
        float lerp = 0;
        while (lerp < 1)
        {
            lerp += Time.deltaTime / crossLostDuration;
            //Fade
            musicIntro.volume = intVol * (1 - lerp);
            musicLoop.volume = lopVol * (1 - lerp);
            //Rise
            musicLoose.volume = lerp;
            yield return 0;
        }
        musicIntro.volume = 0;
        musicLoop.volume = 0;
        musicLoose.volume = 1;
    }

    IEnumerator ReplayGame_Corout()
    {
        if (loopCoroutine != null)
            StopCoroutine(loopCoroutine);

        musicIntro.volume = 0;
        musicLoop.volume  = 0;

        loopCoroutine = StartCoroutine(MusiqueAndLoop());
        float lerp = musicLoose.volume;
        while (lerp > 0)
        {
            lerp -= Time.deltaTime / crossLostDuration;
            //Fade
            musicLoose.volume = lerp;
            yield return 0;
        }
        musicLoose.volume = 0;
    }

    IEnumerator GameToMenu_Corout()
    {
        firstLoad = false;
        float intVol = musicIntro.volume;
        float lopVol = musicLoop.volume;
        float losVol = musicLoose.volume;

        musicMenu.volume = 0;
        musicMenu.Play();
        float lerp = 0;
        while (lerp < 1)
        {
            lerp += Time.deltaTime / backMenuDuration;
            //Fade
            musicIntro.volume = intVol * (1 - lerp);
            musicLoop.volume = lopVol * (1 - lerp);
            musicLoose.volume = losVol * (1 - lerp);
            //Rise
            musicMenu.volume = lerp;
            yield return 0;
        }
        musicIntro.volume = 0;
        musicLoop.volume = 0;
        musicLoose.volume = 0;

        musicMenu.volume = 1;
    }

}
