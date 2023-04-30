using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Animator collectibleGroup;
    public TMPro.TMP_Text numberCollectibles;
    private int lastNumberColl = 0;

    public Animator cuisine;

    public Animator projectileGroup;
    public TMPro.TMP_Text numberProjectile;
    private int lastNumberProjo = 0;

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
    


    public void OnVolumeSliderChange(float value)
    {
        MusicManager.instance.volume = value;
    }
}
