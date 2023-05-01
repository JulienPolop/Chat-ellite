using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public CanvasGroup blackScreen;

    public Animator collectibleGroup;
    public TMPro.TMP_Text numberCollectibles;
    private int lastNumberColl = 0;

    public Animator cuisine;

    public Animator projectileGroup;
    public TMPro.TMP_Text numberProjectile;
    private int lastNumberProjo = 0;

    [Header("Loose screen")]
    public CanvasGroup looseScreen;
    public TMPro.TMP_Text scoreMenuDelivered;
    public TMPro.TMP_Text scoreDollar;
    public TMPro.TMP_Text scoreAlien;
    bool fadeInStart = false;

    private void Start()
    {
        if(LevelManager.instance != null || (Menu.instance != null && !MusicManager.instance.firstLoad) )
            blackScreen.alpha = 1;
    }

    private void Update()
    {
        if (fadeInStart)
            return;

        blackScreen.alpha -= Time.deltaTime * 3f;
    }

    public void UpdateUICollectibles(int numberColl)
    {
        //for now, just add one in the ingredient list
        //and make it appear
        numberCollectibles.SetText(numberColl.ToString());

        collectibleGroup.SetBool("Show", numberColl != 0);

        if (lastNumberColl < numberColl)
        {
            collectibleGroup.SetTrigger("Add");
        }

        lastNumberColl = numberColl;
    }

    public void Cuisine()
    {
        cuisine.SetTrigger("Cook");

        if (LevelManager.instance != null)
            LevelManager.instance.timer.AddMeal(1);
    }
    

    public void UpdateUIProjectiles(int numberProjo)
    {
        //for now, just add one in the ingredient list
        //and make it appear
        numberProjectile.SetText(numberProjo.ToString());

        projectileGroup.SetBool("Show", numberProjo != 0);

        if(lastNumberProjo > numberProjo)
        {
            projectileGroup.SetTrigger("Shoot");
        }
        lastNumberProjo = numberProjo;
    }


    public void LoadGame()
    {
        MusicManager.instance.ReplayGame();
        FadeIn(2f, 1);
    }
    public void GameToMenu()
    {
        MusicManager.instance.GameToMenu();
        FadeIn(1f, 0);
    }

    public void FadeIn(float speed, int indexToLoad, float waitBefore = 0)
    {
        StartCoroutine(FadeIn_Corout(speed, indexToLoad, waitBefore));
    }

    IEnumerator FadeIn_Corout(float speed, int indexToLoad, float waitBefore)
    {
        fadeInStart = true;
        yield return new WaitForSeconds(waitBefore);

        float lerp = blackScreen.alpha;
        while(lerp < 1)
        {
            lerp += speed * Time.deltaTime;
            blackScreen.alpha = lerp;
            yield return 0;
        }
        blackScreen.alpha = 1;

        Debug.Log("yep, we finish. yes yes");
        yield return new WaitForSeconds(1f);

        if (Menu.instance != null)
            Menu.instance.FadeFinish();
        else
        {
            SceneManager.LoadScene(indexToLoad);
        }
    }


    public void OnVolumeSliderChange(float value)
    {
        MusicManager.instance.volume = value;
    }

    public void DisplayLoose()
    {
        StartCoroutine(DisplayLoose_Coroutine());
    }

    IEnumerator DisplayLoose_Coroutine()
    {
        int minute = Mathf.FloorToInt((float)LevelManager.instance.timer.totalTime / 60f);
        int second = Mathf.FloorToInt((float)LevelManager.instance.timer.totalTime % 60f);
        string s = (minute < 10 ? "0" : "") + minute + " minute" + (minute == 0 ? "s" : "") + " and " + (second < 10 ? "0" : "") + second;

        scoreMenuDelivered.SetText(LevelManager.instance.timer.menuDeliver + " menus delivered.");
        scoreDollar.SetText("Work for " + s + " second" + (second == 0 ? "s" : "") + ".");
        scoreAlien.SetText(LevelManager.instance.timer.alienKill + " alien crushed.");

        float lerp = 0;
        looseScreen.blocksRaycasts = true;
        while (lerp < 1)
        {
            lerp += Time.deltaTime;
            looseScreen.alpha = lerp;
            yield return 0;
        }
        looseScreen.interactable = true;
    }
}
