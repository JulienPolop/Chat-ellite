using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKill : MonoBehaviour
{
    private TMPro.TMP_Text text;
    ParticleSystem particule;

    public float upSpeed = 2f;
    public float timingBeforeDeath = 2f;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        particule = GetComponent<ParticleSystem>();
        text = GetComponent<TMPro.TMP_Text>();
        timer = 0;
    }

    public void Init(int toGain)
    {
        text.SetText("+"+(int)toGain+"s");
    }

    private void Update()
    {
        if(particule != null)
        {
            if (!particule.isPlaying)
                Destroy(this.gameObject); 
        }

        if(text != null)
        {
            this.transform.position += Vector3.up * upSpeed * Time.deltaTime;
            if (timer > timingBeforeDeath)
                Destroy(this.gameObject);
        }


        timer += Time.deltaTime;
    }

}
