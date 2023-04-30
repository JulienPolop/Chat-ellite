using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : Collectible
{
    public SpriteRenderer visual;
    public List<Sprite> colorPossible;
    public List<Color> colorPossible_col;
    private Color col;

    private CameraController cam;
    public int indexSpawnPoint;

    public ParticleSystem crushPrefab;

    // Start is called before the first frame update
    void Start()
    {
        cam = LevelManager.instance.cam;
        RandomizePosAndCol();
    }

    void RandomizePosAndCol()
    {
        int randomRange = Random.Range(0, colorPossible.Count);
        visual.sprite = colorPossible[randomRange];
        col = colorPossible_col[randomRange];


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
        Instantiate(crushPrefab, this.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)), null).startColor = col;
        LevelManager.instance.music.CrushAlien();

        //then : 
        base.DestroyThis();
    }
}
