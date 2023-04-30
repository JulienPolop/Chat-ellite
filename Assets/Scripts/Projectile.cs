using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float timerCantBeCollected = 3f;
    private float timeRemaining = 0;

    private bool canBeCollected = false;
    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = timerCantBeCollected;
        canBeCollected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canBeCollected)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                canBeCollected = true;
                GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController controller = GetComponent<PlayerController>();
        if (collision != null && controller != null)
        {
            Debug.Log("Récupération du projectile, A Ajouter au compteur" + collision.gameObject.name);
            Destroy(this.gameObject);

            //Récupération
        }
    }
}
