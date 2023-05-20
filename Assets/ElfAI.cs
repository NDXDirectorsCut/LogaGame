using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElfAI : MonoBehaviour
{
    public float hp = 25;
    //public float damage = 1;
    public Animator anim;
    public float acc;
    public float speed = 2;
    public LayerMask layer2;
    Transform target;
    Vector2 moveDir { get; set;}
    Vector2 direction;
    Rigidbody2D rbody;

    public bool canHit;
    public bool canSearch = true;
    Vector2 searchDir;

    [Header("Attacking")]
    public bool canFire;
    public float fireDelay; //start firing rate
    public float damage;
    public Vector3 offset;
    public float range; // start Range
    public float projectileSpeed;
    public AudioClip fireSound;
    public float projectileSize;
    public float randomization;
    public LayerMask layers;
    public Sprite tearSprite;

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
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position,moveDir,30,layers);
        if(hit.transform != null && hit.transform.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            rbody.velocity += moveDir.normalized * acc;
            StartCoroutine(Attack());
            anim.SetBool("Firing",true);
        }
        else
        {
             //rbody.velocity = Vector2.Lerp(rbody.velocity,Vector2.zero,0.05f);
             StartCoroutine(Search());
             rbody.velocity = searchDir*acc*8;
             anim.SetBool("Firing",false);
        }

        if(rbody.velocity.magnitude > speed)    
        {
            rbody.velocity = Vector2.Lerp(rbody.velocity,Vector2.zero,0.25f);
        }

        if(rbody.velocity.x <0)
        {
            transform.Find("elfSprite").GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            transform.Find("elfSprite").GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    void OnTriggerEnter2D( Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            target = col.transform;
        }
    }

    IEnumerator Search()
    {
        //bool searching = false;
        //Vector2 searchDir = transform.position;
        //rbody.velocity += searchDir*acc*2;
        if(canSearch == true)
        {
            searchDir = Random.insideUnitCircle.normalized;
            canSearch = false;
            yield return new WaitForSeconds(1);
            canSearch = true;
        }
    }

    IEnumerator Attack()
    {
        if(canFire == true)
        {
            GameObject tear = new GameObject("tear", typeof(SpriteRenderer), typeof(Rigidbody2D),typeof(CircleCollider2D));
            GameObject tearSound = new GameObject("tearSound",typeof(AudioSource));

            tear.layer = LayerMask.NameToLayer("Projectiles");
            tear.tag = "Enemy Projectile";
            tear.GetComponent<SpriteRenderer>().sprite = tearSprite;
            tear.GetComponent<SpriteRenderer>().sortingOrder = 10;
            tear.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            tear.GetComponent<Rigidbody2D>().gravityScale = 0;
            tear.GetComponent<Rigidbody2D>().mass = damage;
            tear.GetComponent<Rigidbody2D>().velocity = moveDir.normalized * projectileSpeed;
            tear.GetComponent<CircleCollider2D>().radius = projectileSize;
            tear.GetComponent<CircleCollider2D>().isTrigger = true;
            tearSound.GetComponent<AudioSource>().clip = fireSound;
            tearSound.GetComponent<AudioSource>().Play();
            tear.transform.position = transform.position + offset + new Vector3(Random.Range(-randomization,randomization),Random.Range(-randomization,randomization),0);
            StartCoroutine(TearLife(tear,0.1f,moveDir.normalized));
            Destroy(tear,range);
            Destroy(tearSound,range);
            //Debug.Log("Fire Tear");
            canFire = false;

            yield return new WaitForSeconds(fireDelay);
            canFire = true;
        }
    }

    IEnumerator TearLife(GameObject obj,float acc,Vector2 dir)
    {
        Rigidbody2D objBody = obj.GetComponent<Rigidbody2D>();
        float i,k=0;
        if(objBody != null)
        {
            for(i=0;i<=range;i+=acc)
            {
                objBody.velocity = Vector2.Lerp(dir * projectileSpeed,dir* projectileSpeed * 0.15f,i/range);
                obj.transform.localScale = Vector3.Lerp(Vector3.one,Vector3.one * 0.85f,i/range);
                if(Physics2D.OverlapCircle(obj.transform.position,projectileSize,layers) != null)
                {
                    objBody.velocity = Vector2.zero;
                    Destroy(obj);
                }
                yield return new WaitForSeconds(0.1f);
                k+=0.1f;
            }
        }
    }

}
