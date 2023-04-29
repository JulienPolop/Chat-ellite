using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float positiveAddForce = 10.0f; // Force appliqué à chaque frame lorsque le vaisseau doit avancer
    public float negativeAddForce = 10.0f; // Force appliqué à chaque frame lorsque le vaisseau doit reculer
    public float rotationSpeed = 100.0f; // Vitesse de rotation du vaisseau

    public int projectileCount = 0;
    public Projectile projectilePrefab;
    public float projectileSpeed = 2;

    private Rigidbody2D myRigidBody;

    private List<CelestialBody> InInfluenceSphereCelestialBodies = new List<CelestialBody>(); // La liste des corps qui on une force d'influence sur nous

    public List<Animator> reactors = new List<Animator>();

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        //myRigidBody.velocity = (this.transform.up * (float)Math.Sqrt(2f / 2));
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (projectileCount > 0)
            {
                projectileCount--;
                LevelManager.instance.ui_man.UpdateUIProjectiles(projectileCount);
                Projectile proj = Instantiate(projectilePrefab, myRigidBody.position, transform.rotation);
                proj.GetComponent<Rigidbody2D>().velocity = transform.up * projectileSpeed;
            }
        }
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
            
            if (InInfluenceSphereCelestialBodies.Count == 0)
                myRigidBody.AddForce(direction * positiveAddForce);
            else
                myRigidBody.AddForce(direction * positiveAddForce * 0.8f);
        }
        else if (vertical < 0)
        {
            myRigidBody.AddForce(direction * -negativeAddForce);
        }


        // Rotation sur l'axe vertical (yaw)
        myRigidBody.angularVelocity = -rotation;

        // Force des autres corps
        foreach (CelestialBody cb in InInfluenceSphereCelestialBodies)
        {
            Vector2 gravityDirection = (cb.GetComponent<Rigidbody2D>().position - myRigidBody.position).normalized;
            myRigidBody.AddForce(gravityDirection * cb.gravityForce);
        }

        foreach(Animator reactor in reactors)
        {
            reactor.SetFloat("Speed", myRigidBody.velocity.magnitude / 10f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CelestialBody cb = collision.gameObject.GetComponent<CelestialBody>(); ;
        if (cb != null)
        {
            InInfluenceSphereCelestialBodies.Add(cb);
            myRigidBody.drag = 0;
        }

        Collectible collectible = collision.gameObject.GetComponent<Collectible>();
        if (collectible != null)
        {
            projectileCount++;
            LevelManager.instance.ui_man.UpdateUIProjectiles(projectileCount);
            collectible.DestroyThis();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CelestialBody cb = collision.gameObject.GetComponent<CelestialBody>();
        if (cb != null)
        {
            InInfluenceSphereCelestialBodies.Remove(cb);
            if (InInfluenceSphereCelestialBodies.Count == 0)
                myRigidBody.drag = 0.5f;
        }
    }

    
}
