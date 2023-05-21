using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyAI : MonoBehaviour
{
    Transform target;
    public Rigidbody2D rbody;
    public float acc;
    public float speed;

    public DamageScript dmgScript;
    public SpriteRenderer spriteRend;
    float hp_copy;

    // Start is called before the first frame update
    void Start()
    {
        hp_copy = dmgScript.hp;
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

        if(hp_copy!=dmgScript.hp)
        {
            hp_copy = dmgScript.hp;
            StartCoroutine(Flash());
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player")
        {
            target = col.transform;
        }
    }

    IEnumerator Flash()
    {
        spriteRend.color = new Color(1,0.53f,0.53f);
        yield return new WaitForSeconds(0.2f);
        spriteRend.color = Color.white;
    }
    
}
