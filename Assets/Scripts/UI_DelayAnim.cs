using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DelayAnim : MonoBehaviour
{
    // Start is called before the first frame update
    private void UpdateProjectileVisual()
    {
        if(LevelManager.instance != null)
            LevelManager.instance.ui_man.UpdateUIProjectiles(LevelManager.instance.player.projectileCount);
        else
            Menu.instance.ui_man.UpdateUIProjectiles(Menu.instance.player.projectileCount);
    }
}
