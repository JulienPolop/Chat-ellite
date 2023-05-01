using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float timerCantBeCollected = 0.5f;
    private float timeRemaining = 0;

    public bool canBeCollected = false;

    // Update is called once per frame
    void Update()
    {
        if (!canBeCollected)
        {
            if (timeRemaining < timerCantBeCollected)
            {
                timeRemaining += Time.deltaTime;
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
