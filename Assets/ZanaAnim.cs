using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZanaAnim : MonoBehaviour
{
    public float strength;
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 refPos = transform.parent.position;
        transform.position = refPos + Vector3.up * Mathf.Sin(Time.time*speed)*strength;
    }
}
