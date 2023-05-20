using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfAI : MonoBehaviour
{
    public float hp = 25;
    public float damage = 1;
    public float acc;
    public float speed = 2;
    public LayerMask layers;
    Transform target;
    Vector2 moveDir { get; set;}
    Vector2 direction;
    Rigidbody2D rbody;

    int dir;

    // Start is called before the first frame update
    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            moveDir = target.position-transform.position;
            rbody.velocity += moveDir.normalized * acc;
        }
        rbody.velocity = Vector3.ClampMagnitude(rbody.velocity,speed);
        
    }
    void OnTriggerEnter2D( Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            target = col.transform;
        }
    }
}
