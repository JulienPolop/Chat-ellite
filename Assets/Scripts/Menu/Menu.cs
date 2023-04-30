using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public static Menu instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            instance.player = player;
            Destroy(this.gameObject);
        }
    }

    public PlayerController player;
    public MenuCamera cam;
    public UIManager ui_man;

    // Update is called once per frame
    public void LaunchGame()
    {
        MusicManager.instance.MenuToGame();

        ui_man.FadeIn(2f, 1, 1f);
    }

    public void FadeFinish()
    {
        SceneManager.LoadScene(1);
    }
}
