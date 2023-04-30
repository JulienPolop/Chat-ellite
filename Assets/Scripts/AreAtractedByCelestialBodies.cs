using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AreAtractedByCelestialBodies : MonoBehaviour
{
    public float gravityForceMultiplier = 0.1f;

    private Rigidbody2D myRigidBody;
    private List<CelestialBody> InInfluenceSphereCelestialBodies = new List<CelestialBody>(); // La liste des corps qui on une force d'influence sur nous

    private float drag;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        drag = myRigidBody.drag;
    }

    void FixedUpdate()
    {
        // Force des autres corps
        foreach (CelestialBody cb in InInfluenceSphereCelestialBodies)
        {
            Vector2 gravityDirection = (cb.GetComponent<Rigidbody2D>().position - myRigidBody.position).normalized;
            myRigidBody.AddForce(gravityDirection * cb.gravityForce * gravityForceMultiplier);
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
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CelestialBody cb = collision.gameObject.GetComponent<CelestialBody>();
        if (cb != null)
        {
            InInfluenceSphereCelestialBodies.Remove(cb);
            if (InInfluenceSphereCelestialBodies.Count == 0)
                myRigidBody.drag = drag;
        }
    }
}
