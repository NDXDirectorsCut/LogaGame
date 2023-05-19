using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public float hp;

    public string weakTo;
    public Rigidbody2D rbody;
    public float knockback;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Projectiles") && col.gameObject.tag == weakTo)
        {
            Rigidbody2D cbody = col.gameObject.GetComponent<Rigidbody2D>();
            rbody.velocity = cbody.velocity.normalized * knockback;

        }
    }
}
