using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyAI : MonoBehaviour
{
    Transform target;
    public Rigidbody2D rbody;
    public float acc;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector2 moveDir = target.position-transform.position;
            if(rbody.velocity.magnitude <= speed)
            {
                rbody.velocity += moveDir.normalized * acc;
            }
            //rbody.velocity = Quaternion.FromToRotation(rbody.velocity,moveDir) * rbody.velocity;
        }
        if(rbody.velocity.magnitude > speed)
        {
            rbody.velocity = Vector2.Lerp(rbody.velocity,Vector2.zero,0.25f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            target = col.transform;
        }
    }
}
