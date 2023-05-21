using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WiggleAnimScript : MonoBehaviour
{
    public Vector3 sinStrength;
    public Vector3 cosStrength;
    public Vector2 speed;

    void Start()
    {
        //startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {

        float sinTime = Mathf.Sin(Time.time*speed.x);
        float cosTime = Mathf.Cos(Time.time*speed.y);
        transform.position = transform.parent.position + (sinStrength*sinTime) + (cosStrength*cosTime);
    }
}
