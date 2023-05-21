using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public float hp;

    public string weakTo;
    public Rigidbody2D rbody;
    public float knockback;
    public GameObject DeathParticle;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hp<=0)
        {
            GameObject deathEffect = Instantiate(DeathParticle,transform.position + new Vector3(0,0,-2),Quaternion.identity);
            ParticleSystem particle = deathEffect.GetComponent<ParticleSystem>();
            Destroy(deathEffect,particle.duration*20);
            Destroy(transform.parent.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Projectiles") && col.gameObject.tag == weakTo)
        {
            Rigidbody2D cbody = col.gameObject.GetComponent<Rigidbody2D>();
            rbody.velocity = cbody.velocity.normalized * knockback;
            Destroy(cbody.gameObject,.25f);
            hp = hp - cbody.mass;
        }
    }
}
