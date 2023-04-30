using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{

    public float creditsPos = 8f;
    public float tuto1Pos = -8f;
    public float tuto2Pos = -16f;

    Vector3 startPos = new Vector3(0,0,-10);
    Vector3 targetPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.instance.player.transform.position.x > creditsPos)
        {
            targetPos = Vector3.right * creditsPos * 2 + Vector3.forward * -10;
        }
        else if(Menu.instance.player.transform.position.x < tuto2Pos)
        {
            targetPos = Vector3.left * creditsPos * 4 + Vector3.forward * -10;
        }
        else if(Menu.instance.player.transform.position.x < tuto1Pos)
        {
            targetPos = Vector3.left * creditsPos * 2 + Vector3.forward * -10;
        }
        else
        {
            targetPos = startPos;
        }


        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * 3);
    }
}
