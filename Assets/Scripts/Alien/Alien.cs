using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public SpriteRenderer visual;
    public List<Color> colorPossible;

    private CameraController cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CameraController>();
        RandomizePosAndCol();
    }

    void RandomizePosAndCol()
    {
        int randomRange = Random.Range(0, colorPossible.Count - 1);
        visual.color = colorPossible[randomRange];


        this.transform.Rotate(0, 0, Random.Range(0, 360));

        //For now : purely random pos.
        float xPos = Random.Range(cam.BoundLeft, cam.BoundRight);
        float yPos = Random.Range(cam.BoundBot, cam.BoundTop);
        this.transform.position = new Vector3(xPos, yPos, 0);
    }

}
