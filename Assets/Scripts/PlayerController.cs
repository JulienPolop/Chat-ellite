using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float positiveAddForce = 10.0f; // Force appliqué à chaque frame lorsque le vaisseau doit avancer
    public float negativeAddForce = 10.0f; // Force appliqué à chaque frame lorsque le vaisseau doit reculer
    public float rotationSpeed = 100.0f; // Vitesse de rotation du vaisseau

    private Rigidbody2D myRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Récupération des entrées du joueur
        float vertical = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Translation avant/arrière
        Vector2 direction = this.transform.up;
        if (vertical > 0) { 
            myRigidBody.AddForce(direction* positiveAddForce); 
        }
        else if (vertical < 0)
        {
            myRigidBody.AddForce(direction * -negativeAddForce);
        }
        

        // Rotation sur l'axe vertical (yaw)
        rotation *= Time.deltaTime;
        transform.Rotate(0, 0, -rotation);
    }
}
