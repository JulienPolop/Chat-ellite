using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKill : MonoBehaviour
{

    ParticleSystem particule;
    // Start is called before the first frame update
    void Start()
    {
        particule = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(particule != null)
        {
            if (!particule.isPlaying)
                Destroy(this.gameObject); 
        }
    }

}
