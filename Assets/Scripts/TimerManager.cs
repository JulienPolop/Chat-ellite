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
    public int totalCash = 0;

    public void AddTimer(float timingAdd, int menuDeliver = 0)
    {
        totalCash += (int)timingAdd;
        timer += timingAdd;
        menuDeliver += menuDeliver;
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

        timerText.SetText((int)(timer * 100f) + " $");

    }
}
