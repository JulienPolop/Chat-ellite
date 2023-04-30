using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    // Start is called before the first frame update

    public float startSpeed;
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.up * startSpeed;
    }

    void FixedUpdate()
    {
        // Récupérer la velocity de l'objet
        Vector2 velocity = GetComponent<Rigidbody2D>().velocity;

        // Calculer l'angle à appliquer à l'objet
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        // Appliquer l'angle à l'objet
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        GetComponent<Rigidbody2D>().rotation = angle - 90;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Projectile proj = collision.gameObject.GetComponent<Projectile>();
        if (proj) {
            //Point en plus
            Debug.Log("BIEN REMPLIT");
            Destroy(proj.gameObject);

            //Ajout Timer
        }
    }
}
