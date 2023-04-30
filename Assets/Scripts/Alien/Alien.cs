using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Collectible
{
    public SpriteRenderer visual;
    public List<Sprite> colorPossible;

    private CameraController cam;
    public int indexSpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        cam = LevelManager.instance.cam;
        RandomizePosAndCol();
    }

    void RandomizePosAndCol()
    {
        int randomRange = Random.Range(0, colorPossible.Count - 1);
        visual.sprite = colorPossible[randomRange];


        this.transform.Rotate(0, 0, Random.Range(0, 360));

        //Alien spawner decide for this
        ////For now : purely random pos.
        //float xPos = Random.Range(cam.BoundLeft, cam.BoundRight);
        //float yPos = Random.Range(cam.BoundBot, cam.BoundTop);
        //this.transform.position = new Vector3(xPos, yPos, 0);
    }


    public override void DestroyThis()
    {
        LevelManager.instance.spawner.KillAlien(this);
        //make some particule
        //or even animation !

        //then : 
        base.DestroyThis();
    }
}
