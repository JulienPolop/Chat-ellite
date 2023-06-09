using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float timer = 60f;
    public TMP_Text timerText;

    public int menuDeliver = 0;
    public int alienKill = 0;
    public int totalCook = 0;
    public float totalTime = 0;

    private void Start()
    {
        totalTime += timer;
    }

    public void AddTimer(float timingAdd, int addMenu = 0)
    {
        timer += timingAdd;
        totalTime += timingAdd;
        menuDeliver += addMenu;
    }

    public void AddMeal(int addMenu = 1)
    {
        totalCook += addMenu;
    }

    public void AddAlienScore(int numberOfAlienKill = 1)
    {
        alienKill += numberOfAlienKill;
    }

    private void Update()
    {
        if (LevelManager.instance != null && LevelManager.instance.loose && timer < -1)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                LevelManager.instance.ui_man.LoadGame();
            }


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                LevelManager.instance.ui_man.GameToMenu();
            }
            return;
        }

        if (timer < -1)
            return;


        timer -= Time.deltaTime;


        if (timer < 0)
        {
            timerText.SetText((int)(0 * 100f) + " $");
            timerText.color = Color.Lerp(Color.white - Color.black, Color.white, 1 + timer);

            if (!LevelManager.instance.loose)
            {
                LevelManager.instance.ui_man.DisplayLoose();
                LevelManager.instance.loose = true;
            }
            return;
        }
        
        int minute = Mathf.FloorToInt(timer / 60f);
        int second = Mathf.FloorToInt(timer % 60f);
        timerText.SetText((minute < 10 ? "0" : "") + minute + " : " + (second < 10 ? "0" : "") + second);
    }
}
