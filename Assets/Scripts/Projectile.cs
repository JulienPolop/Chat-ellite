using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float timerCantBeCollected = 3f;
    private float timeRemaining = 0;

    public bool canBeCollected = false;
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
                gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    public virtual void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
