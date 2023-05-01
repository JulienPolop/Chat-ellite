using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public float timerGainWhenFeed = 15f;

    public GameObject particleVictory;
    public GameObject particleWhenNotFeed;

    private Vector2 lastPos;
    public bool updateRotation = true;

    [Header("AlerteSystem")]
    public bool isTarget;
    public Transform ui;
    public GameObject cam;
    public GameObject alerte;
    private GameObject currentAlarms;

    public bool NeedToBeFed = false;

    // Start is called before the first frame update

    public float startSpeed;
    void Start()
    {
        if (startSpeed > 0)
            GetComponent<Rigidbody2D>().velocity = transform.up * startSpeed;
    }

    private void FixedUpdate()
    {
        if (updateRotation)
        {
            // Récupérer la velocity de l'objet
            Vector2 pos = transform.position;
            Vector2 velocity = (pos - lastPos);

            if (velocity.magnitude > 0.01)
            {
                // Calculer l'angle à appliquer à l'objet
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

                // Appliquer l'angle à l'objet
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                GetComponent<Rigidbody2D>().rotation = angle - 90;

                lastPos = pos;
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Projectile proj = collision.gameObject.GetComponent<Projectile>();
        if (proj) {
            MusicManager.instance.FeedShip();

            GameObject particle = particleWhenNotFeed;

            if (Menu.instance != null)
            {
                Menu.instance.LaunchGame();
            }
            else
            {
                if (NeedToBeFed)
                {
                    //Also spawn some good vide and a +20$ (textmesh pro) who disappear
                    LevelManager.instance.timer.AddTimer(timerGainWhenFeed, 1);
                    RemoveAlert();
                    NeedToBeFed = false;
                    LevelManager.instance.ChoseCustomerToFeed();
                    particle = particleVictory;
                }
            }
            //Point en plus
            Destroy(proj.gameObject);
            Instantiate(particle, this.transform.position + Vector3.up * 1, Quaternion.identity, null);
        }
    }

    public void StartAlerte()
    {
        Debug.Log("StartAlerte");
        currentAlarms = Instantiate(alerte);
        currentAlarms.transform.parent = ui.transform;
        currentAlarms.GetComponent<AlerteAndArrow>().cam = cam.GetComponent<Camera>();
        currentAlarms.GetComponent<AlerteAndArrow>().target = this.gameObject.transform;

        alerte.SetActive(true);
        isTarget = true;
    }

    public void RemoveAlert()
    {
        isTarget = false;
        if (currentAlarms)
            Destroy(currentAlarms);
    }
}
