using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlerteAndArrow : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    public GameObject warning;
    public GameObject arrow;
    public float offset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;
        Vector3 Objectpos = Camera.main.WorldToScreenPoint(target.position);
        if (Objectpos.z > 0 && Objectpos.x > 0 && Objectpos.x < Screen.width && Objectpos.y > 0 && Objectpos.y < Screen.height)
        {

            arrow.SetActive(false);
            warning.transform.position = Objectpos;
        }
        else
        {
            arrow.SetActive(true);

            Vector3 screenCenter = new Vector3(Screen.width, Screen.height, 0) / 2;

            Objectpos -= screenCenter;

            if (Objectpos.z < 0)
            {
                Objectpos *= -1;
            }

            float angle = Mathf.Atan2(Objectpos.y, Objectpos.x);
            angle -= 90 * Mathf.Deg2Rad;

            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            Objectpos = screenCenter + new Vector3(sin * 150, cos * 150, 0);

            float m = cos / sin;
            Vector3 screenBounds = screenCenter * 1f;

            Objectpos = cos > 0 ? new Vector3(-screenBounds.y / m, screenCenter.y, 0) : new Vector3(screenBounds.y / m, -screenCenter.y, 0);

            if (Objectpos.x > screenBounds.x)
            {
                Objectpos = new Vector3(screenBounds.x, -screenBounds.x * m, 0);
            }
            else if (Objectpos.x < -screenBounds.x)
            {
                Objectpos = new Vector3(-screenBounds.x, screenBounds.x * m, 0);
            }

            Objectpos += screenCenter;
            float endposx = Objectpos.x;
            float endposy = Objectpos.y;
            float endposx2 = Objectpos.x;
            float endposy2 = Objectpos.y;
            warning.transform.position = Objectpos;
            arrow.transform.position = Objectpos;

            //print(Objectpos + " scren height = " + Screen.height + " screen weight = " + Screen.width);
            if (Objectpos.x <= 0)
            {
                endposx += offset;
                endposx2 += offset * 2;
            }
            if (Objectpos.x >= Screen.height)
            {
                endposx += -offset;
                endposx2 += -(offset * 2);
            }
            if (Objectpos.y <= 0)
            {
                endposy += +offset;
                endposy2 += offset * 2;
            }
            if (Objectpos.y >= Screen.height)
            {
                endposy += -offset;
                endposy2 += -(offset * 2);
            }
            warning.transform.position = new Vector3(endposx2, endposy2, Objectpos.z);
            arrow.transform.position = new Vector3(endposx, endposy, Objectpos.z);
            
            /*warning.transform.position = Objectpos;
            arrow.transform.position = Objectpos;*/
            arrow.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        }
    }
}
