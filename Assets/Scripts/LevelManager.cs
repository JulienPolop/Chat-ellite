using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            SetUpMissingElement();

            Screen.fullScreen = true;
        }
        else
        {
            Debug.LogError("Two gameManager, this is a problem.");
            Destroy(this.gameObject);
        }
    }



    public PlayerController player;
    public CameraController cam;
    public AlienSpawner spawner;
    public UIManager ui_man;
    public MusicManager music;

    
    private void SetUpMissingElement()
    {
        if(player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        if (cam == null)
        {
            cam = FindObjectOfType<CameraController>();
        }
        if (spawner == null)
        {
            spawner = FindObjectOfType<AlienSpawner>();
        }
        if (ui_man == null)
        {
            ui_man = FindObjectOfType<UIManager>();
        }
        if (music == null)
        {
            music = FindObjectOfType<MusicManager>();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.fullScreen = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
    }
}
