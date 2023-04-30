using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DelayAnim : MonoBehaviour
{
    // Start is called before the first frame update
    private void UpdateProjectileVisual()
    {
        LevelManager.instance.ui_man.UpdateUIProjectiles(LevelManager.instance.player.projectileCount);
    }
}
