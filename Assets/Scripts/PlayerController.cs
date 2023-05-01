using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movements")]
    public float positiveAddForce = 10.0f; // Force appliqu� � chaque frame lorsque le vaisseau doit avancer
    public float negativeAddForce = 10.0f; // Force appliqu� � chaque frame lorsque le vaisseau doit reculer
    public float rotationSpeed = 100.0f; // Vitesse de rotation du vaisseau
    public float startSpeed = 0;

    [Header("Projectiles")]
    public Projectile projectilePrefab;
    public int projectileCount = 0;
    public int collectibleCount = 0;
    public float projectileSpeed = 2;
    public GameObject projectileParticlePrefab;
    public Transform shootSpawnPoint;

    [Header("Collectibles")] //Pour en relacher quand on tape rop fort une plan�te
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
    private UIManager ui_man;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();

        ui_man = (LevelManager.instance == null ? Menu.instance.ui_man : LevelManager.instance.ui_man);
        ui_man.UpdateUIProjectiles(projectileCount);
        
        if (camera == null && LevelManager.instance != null)
            camera = LevelManager.instance.cam;

        myRigidBody.velocity = transform.up * startSpeed;
    }

    private void Update()
    {
        if(LevelManager.instance != null && LevelManager.instance.loose)
        {
            return;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (projectileCount > 0)
            {
                projectileCount--;
                ui_man.UpdateUIProjectiles(projectileCount);
                Projectile proj = Instantiate(projectilePrefab, myRigidBody.position, transform.rotation);
                proj.GetComponent<Rigidbody2D>().velocity = transform.up * projectileSpeed;

                MusicManager.instance.Shoot();
            }
            else
            {
                MusicManager.instance.Shoot_Empty();
            }

            //will kill themself by themself
            Instantiate(projectileParticlePrefab, shootSpawnPoint.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LevelManager.instance != null && LevelManager.instance.loose)
        {
            return;
        }
        // R�cup�ration des entr�es du joueur
        float vertical = Input.GetAxis("Vertical");
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;

        // Translation avant/arri�re
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

        //Debug.Log("velocity"+ myRigidBody.velocity.magnitude * 1.3f);
        if (camera == null) //early return in case of Menu
            return;
        if (camera.TargetCamera == cameraTarget)
        {
            cameraTarget.gameObject.transform.localPosition = Vector3.up * myRigidBody.velocity.magnitude * 4f;
            camera.cameraSize = Math.Clamp(myRigidBody.velocity.magnitude * 1.5f, 6, 14);
        }

        //Debug.Log("Player: " + myRigidBody.velocity + ", mag: " + myRigidBody.velocity.magnitude);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CelestialBody cb = collision.gameObject.GetComponent<CelestialBody>(); ;
        if (cb != null && cb.hasGravity)
        {
            InInfluenceSphereCelestialBodies.Add(cb);
            myRigidBody.drag = 0;

            CelestialBody newTarget = InInfluenceSphereCelestialBodies.OrderBy(p => p.gravityDistance).Last();
            camera.TargetCamera = newTarget.gameObject;
            camera.cameraSize = newTarget.gravityDistance * 1.2f;
        }

        Collectible collectible = collision.gameObject.GetComponent<Collectible>();
        if (collectible != null)
        {
            collectibleCount++;
            if(collectibleCount >= 3)
            {
                collectibleCount -= 3;
                projectileCount++;
                MusicManager.instance.Cook();
                ui_man.Cuisine();
                //Delay : it's call by the animation//LevelManager.instance.ui_man.UpdateUIProjectiles(projectileCount);
            }

            ui_man.UpdateUICollectibles(collectibleCount);
            collectible.DestroyThis();
        }

        Projectile pr = collision.gameObject.GetComponent<Projectile>();
        if (pr != null)
        {
            projectileCount++;
            ui_man.UpdateUIProjectiles(projectileCount);
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
            else
            {
                CelestialBody newTarget = InInfluenceSphereCelestialBodies.OrderBy(p => p.gravityDistance).Last();
                camera.TargetCamera = newTarget.gameObject;
                camera.cameraSize = newTarget.gravityDistance * 1.2f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude < 4)
        {
            if (camera)
                camera.ShakeCameraLittle();
        }
        else if (collision.relativeVelocity.magnitude >= 4 && collision.relativeVelocity.magnitude < 8)
        {
            if (camera)
                camera.ShakeCameraStandard();

            MusicManager.instance.HitPlanet();
        }
        else if (collision.relativeVelocity.magnitude >= 8)
        {
            if (camera)
                camera.ShakeCameraBig();

            if (collectibleCount > 0)
            {
                collectibleCount--;
                LevelManager.instance.ui_man.UpdateUICollectibles(collectibleCount);
                Collectible coll = Instantiate(collectiblePrefab, myRigidBody.position, transform.rotation);
                MusicManager.instance.HitPlanet();

                Vector2 direction = ((Vector2)collision.transform.position - myRigidBody.position).normalized;
                coll.GetComponent<Rigidbody2D>().velocity = -direction * collectibleSpeed;
            }
        }
    }
}
