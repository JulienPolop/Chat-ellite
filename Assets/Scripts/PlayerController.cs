using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movements")]
    public float positiveAddForce = 10.0f; // Force appliqué à chaque frame lorsque le vaisseau doit avancer
    public float negativeAddForce = 10.0f; // Force appliqué à chaque frame lorsque le vaisseau doit reculer
    public float rotationSpeed = 100.0f; // Vitesse de rotation du vaisseau

    [Header("Projectiles")]
    public Projectile projectilePrefab;
    public int projectileCount = 0;
    public int collectibleCount = 0;
    public float projectileSpeed = 2;

    [Header("Collectibles")] //Pour en relacher quand on tape rop fort une planète
    public Collectible collectiblePrefab;
    public float collectibleSpeed = 1;

    [Header("Reactors Effects")]
    public List<Animator> reactors = new List<Animator>();
    public List<TrailRenderer> reactorTrails = new List<TrailRenderer>();
    public List<TrailRenderer> reverseReactorTrails = new List<TrailRenderer>();

    [Header("References")]
    public new CameraController camera;
    public GameObject cameraTarget;
    private Rigidbody2D myRigidBody;
    private List<CelestialBody> InInfluenceSphereCelestialBodies = new List<CelestialBody>(); // La liste des corps qui on une force d'influence sur nous

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        if(LevelManager.instance != null)
            LevelManager.instance.ui_man.UpdateUIProjectiles(projectileCount);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (projectileCount > 0)
            {
                projectileCount--;
                if (LevelManager.instance != null)
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

            reactorTrails.ForEach(tr => { if (!tr.emitting) tr.emitting = true; });
            reverseReactorTrails.ForEach(tr => { if (tr.emitting) tr.emitting = false; });
        }
        else if (vertical < 0)
        {
            myRigidBody.AddForce(direction * -negativeAddForce);

            reactorTrails.ForEach(tr => { if (tr.emitting) tr.emitting = false; });
            reverseReactorTrails.ForEach(tr => { if (!tr.emitting) tr.emitting = true; });
        }
        else
        {
            reactorTrails.ForEach(tr => { if (tr.emitting) tr.emitting = false; });
            reverseReactorTrails.ForEach(tr => { if (tr.emitting) tr.emitting = false; });
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

        Debug.Log("velocity"+ myRigidBody.velocity.magnitude * 1.3f);
        if (camera.TargetCamera == cameraTarget)
            camera.cameraSize = Math.Clamp(myRigidBody.velocity.magnitude * 1.3f, 4,10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CelestialBody cb = collision.gameObject.GetComponent<CelestialBody>(); ;
        if (cb != null)
        {
            InInfluenceSphereCelestialBodies.Add(cb);
            Debug.Log("Remove Drag");
            myRigidBody.drag = 0;

            camera.TargetCamera = cb.gameObject;
            camera.cameraSize = cb.gravityDistance*1.2f;
        }

        Collectible collectible = collision.gameObject.GetComponent<Collectible>();
        if (collectible != null)
        {
            projectileCount++;
            LevelManager.instance.ui_man.UpdateUIProjectiles(projectileCount);
            collectible.DestroyThis();
        }

        Projectile pr = collision.gameObject.GetComponent<Projectile>();
        if (pr != null)
        {
            pr.DestroyThis();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CelestialBody cb = collision.gameObject.GetComponent<CelestialBody>();
        if (cb != null)
        {
            InInfluenceSphereCelestialBodies.Remove(cb);
            if (InInfluenceSphereCelestialBodies.Count == 0)
            {
                myRigidBody.drag = 0.5f;

                camera.TargetCamera = cameraTarget;
                camera.cameraSize = 5;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Projectile pr = collision.gameObject.GetComponent<Projectile>();
        //if (pr != null)
        //{
        //    pr.DestroyThis();
        //}
        //else
        //{
            Debug.Log(collision.relativeVelocity.magnitude);
            if (collision.relativeVelocity.magnitude < 4)
            {
                if (camera)
                    camera.ShakeCameraLittle();
            }
            else if (collision.relativeVelocity.magnitude >= 4 && collision.relativeVelocity.magnitude < 8)
            {
                if (camera)
                    camera.ShakeCameraStandard();
            }
            else if (collision.relativeVelocity.magnitude >= 8)
            {
                if (camera)
                    camera.ShakeCameraBig();

                if (projectileCount > 0)
                {
                    projectileCount--;
                    LevelManager.instance.ui_man.UpdateUIProjectiles(projectileCount);
                    Collectible proj = Instantiate(collectiblePrefab, myRigidBody.position, transform.rotation);

                    Vector2 direction = ((Vector2)collision.transform.position - myRigidBody.position).normalized;
                    proj.GetComponent<Collider2D>().enabled = false;
                    proj.GetComponent<Rigidbody2D>().velocity = -direction * collectibleSpeed;
                }
            }
        //}
    }
}
