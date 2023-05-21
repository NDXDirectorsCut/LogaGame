using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusuroiAI : MonoBehaviour
{
    Vector3 shootDir;
    public Sprite antSprite;
    public float fireDelay;
    public float range;
    public float projectileSpeed;
    public float projectileSize;
    //public float 
    public LayerMask layers;
    public bool canFire;
    int i;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        if(canFire == true)
        {
            for(i=0;i<4;i++)
            {
                switch(i)
                {
                    case 0:
                        shootDir = Vector2.right;
                        break;
                    case 1:
                        shootDir = Vector2.up;
                        break;
                    case 2:
                        shootDir = Vector3.left;
                        break;
                    case 3:
                        shootDir = Vector3.down;
                        break;
                }
                GameObject ant = new GameObject("ant",typeof(SpriteRenderer),typeof(Rigidbody2D),typeof(CircleCollider2D));
                ant.layer = LayerMask.NameToLayer("Projectiles");
                ant.tag = "Enemy Projectile";
                ant.transform.position = transform.position;
                ant.GetComponent<SpriteRenderer>().sprite = antSprite;
                ant.GetComponent<SpriteRenderer>().sortingOrder = 10;
                ant.GetComponent<Rigidbody2D>().velocity = shootDir * projectileSpeed;
                ant.GetComponent<Rigidbody2D>().angularDrag = 0;
                ant.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-10,10);
                ant.GetComponent<Rigidbody2D>().gravityScale = 0;
                ant.GetComponent<CircleCollider2D>().radius = projectileSize;
                ant.GetComponent<CircleCollider2D>().isTrigger = true;
                StartCoroutine(TearLife(ant,0.1f,shootDir));
                Destroy(ant,range);
                //ant.layer = 

            }
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

