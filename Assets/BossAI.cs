using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{

    public Sprite defaultSprite;
    public Sprite phase1Sprite;
    public SpriteRenderer spriteRend;

    public int phase;
    public bool inPhase;

    Vector3 startPos;
    int k;

    //public GameObject 
    public WiggleAnimScript wiggleScript;
    public Sprite fireProjectileSprite;
    public float projectileSpeed;

    public float phase1Length;
    public float phase2Length;
    public float phase3Length;



    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(phase == 1 && inPhase == false)
        {
            StartCoroutine(Phase1());
            //spriteRend.sprite = phase1Sprite;
        }
        if(phase == 2 && inPhase == false)
        {
            StartCoroutine(Phase2());
            //spriteRend.sprite = defaultSprite;
        }
        if(phase == 3 && inPhase == false)
        {
            StartCoroutine(Phase3());
        }

        if(phase == 2 || phase == 3)
        {
            transform.position = Vector3.Lerp(transform.position,startPos+Vector3.up*4,0.1f);
        }
        if(phase == 1)
        {
            transform.position = Vector3.Lerp(transform.position,startPos,0.1f);
        }

    }

    IEnumerator Phase1()
    {
        k=0;
        wiggleScript.sinStrength = new Vector3(0.2f,0,0);
        wiggleScript.cosStrength = new Vector3(0,0.2f,0);
        wiggleScript.speed = new Vector2(4,8);
        inPhase = true;
        StartCoroutine(Attack1());
        yield return new WaitForSeconds(phase1Length);
        phase = Random.Range(2,4);
        inPhase = false;
    }

    IEnumerator Attack1()
    {
        yield return new WaitForSeconds(2);
        while(phase ==1 && inPhase == true)
        {
            int i; Vector3 dir = Vector3.up;
            for(i=1;i<=12;i++)
            {
                dir = Quaternion.AngleAxis(360/12 + k*45f,Vector3.forward) * dir;
                //Debug.DrawRay(transform.position,dir,Color.red);
                GameObject fireProj = new GameObject("fireProjectile",typeof(SpriteRenderer),typeof(Rigidbody2D),typeof(CircleCollider2D));
                Debug.Log(k);
                fireProj.transform.parent = transform.parent.parent;
                fireProj.layer = LayerMask.NameToLayer("Projectiles");
                fireProj.tag = "Enemy Projectile";
                fireProj.GetComponent<SpriteRenderer>().sprite = fireProjectileSprite;
                fireProj.GetComponent<SpriteRenderer>().sortingOrder = 40;
                fireProj.GetComponent<Rigidbody2D>().gravityScale = 0;
                fireProj.GetComponent<Rigidbody2D>().mass = 1;
                fireProj.GetComponent<Rigidbody2D>().velocity = dir.normalized * projectileSpeed;
                fireProj.GetComponent<CircleCollider2D>().radius = .125f;
                fireProj.GetComponent<CircleCollider2D>().isTrigger = true;
                fireProj.transform.position = transform.position;
                Destroy(fireProj,4);
            }
            yield return new WaitForSeconds(1f);
            k++;
        }
    }


    IEnumerator Phase2()
    {
        wiggleScript.sinStrength = Vector3.zero;
        wiggleScript.cosStrength = Vector3.zero;
        wiggleScript.speed = Vector2.zero;
        inPhase = true;
        yield return new WaitForSeconds(phase2Length);
        phase = 1;
        inPhase = false;
    }

    IEnumerator Phase3()
    {
        wiggleScript.sinStrength = Vector3.zero;
        wiggleScript.cosStrength = Vector3.zero;
        wiggleScript.speed = Vector2.zero;
        inPhase = true;
        
        yield return new WaitForSeconds(phase3Length);
        phase = 1;
        inPhase = false;
    }

}
