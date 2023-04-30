using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeFontSize : MonoBehaviour
{
    public TMPro.TMP_Text targetText;
    public TMPro.TMP_Text myText;

    // Update is called once per frame
    void Update()
    {
        myText.fontSize = targetText.fontSize;
    }
}
