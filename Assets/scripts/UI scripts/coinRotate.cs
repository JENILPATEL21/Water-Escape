using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinRotate : MonoBehaviour
{
    public float spinSpeed = 5f;
    public float minScale = 0.1f;

    // Update is called once per frame
    void Update()
    {
        float scaleX = Mathf.Cos(Time.time * spinSpeed);
        scaleX = Mathf.Sign(scaleX) * Mathf.Max(minScale, Mathf.Abs(scaleX)); // smooth scale without flipping
        transform.localScale = new Vector3(scaleX, 1f, 1f);
        
    }
}
