using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMovement : MonoBehaviour
{
    Rigidbody2D rbody;


    [Header("Movement")]
    public float hp; // start HP
    public float acc;
    public float speed;
    public bool invincible;
    [Space(10)]

    [Header("Attacking")]
    public bool canFire;
    public float fireDelay; //start firing rate
    public float range; // start Range
    public float projectileSpeed;
    public float projectileSize;
    public float randomization;
    public LayerMask layers;
    public Sprite tearSprite;
    [Space(10)]

    [Header("Settings")]
    public bool smoothInput;
    [Space(50)]

    [Header("Debug")]
    [SerializeField]
    public bool drawDebug;

    Vector3 leftAxis { get; set; }
    Vector3 rightAxis { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float lHor = smoothInput ? Input.GetAxis("LeftHorizontal") : Input.GetAxisRaw("LeftHorizontal");
        float lVer = smoothInput ? Input.GetAxis("LeftVertical") : Input.GetAxisRaw("LeftVertical");
        float rHor = Input.GetAxisRaw("RightHorizontal");
        float rVer = Input.GetAxisRaw("RightVertical");

        leftAxis = Vector3.ClampMagnitude(new Vector3(lHor,lVer,0),1);
        rightAxis = new Vector3(rHor,rVer,0);
        
        rbody.velocity += new Vector2(leftAxis.x,leftAxis.y) * acc;
        rbody.velocity = Vector3.ClampMagnitude(rbody.velocity,speed);

        if(leftAxis.magnitude > 0.1f)
        {

        }
        else
        {
            rbody.velocity = Vector3.Lerp(rbody.velocity,Vector3.zero,0.2f);
        }

        if(rightAxis.magnitude > 0.1f)
        {

            StartCoroutine(Attack());
        }
        


        if(drawDebug == true)
        {
            Debug.DrawRay(transform.position,leftAxis,Color.blue);
            Debug.DrawRay(transform.position,rightAxis,Color.red);
        }
    }

    IEnumerator Attack()
    {
        if(canFire == true)
        {
            GameObject tear = new GameObject("tear", typeof(SpriteRenderer), typeof(Rigidbody2D),typeof(CircleCollider2D));
            tear.layer = LayerMask.NameToLayer("Player");
            tear.GetComponent<SpriteRenderer>().sprite = tearSprite;
            tear.GetComponent<SpriteRenderer>().sortingOrder = 10;
            tear.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            tear.GetComponent<Rigidbody2D>().gravityScale = 0;
            tear.GetComponent<Rigidbody2D>().velocity = rightAxis.normalized * projectileSpeed;
            tear.GetComponent<CircleCollider2D>().radius = projectileSize;
            tear.GetComponent<CircleCollider2D>().isTrigger = true;
            tear.transform.position = transform.position + new Vector3(Random.Range(-randomization,randomization),Random.Range(-randomization,randomization),0);
            StartCoroutine(TearLife(tear,0.1f,rightAxis.normalized));
            Destroy(tear,range);
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
