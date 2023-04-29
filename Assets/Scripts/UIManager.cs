using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Animator projectileGroup;
    public TMPro.TMP_Text numberProjectile;
    private int lastNumberProjo = 0;

    public void UpdateUICollectibles()
    {
        //for now, just add one in the ingredient list
        //and make it appear
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
    
}
