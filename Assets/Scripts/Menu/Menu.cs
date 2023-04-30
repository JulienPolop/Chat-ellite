using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Update()
    {
    }
}
