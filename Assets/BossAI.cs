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

    public Sprite dvdSprite;
    public float dvdSpeed;
    public LayerMask layers;

    public AudioClip neighhSound;
    public AudioClip gallopSound;
    public GameObject horseBullet;
    public float horseSpeed;

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
                //DDebug.Log(k);
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
        StartCoroutine(DVDAttack());
        yield return new WaitForSeconds(phase2Length);
        phase = 1;
        inPhase = false;
    }

    IEnumerator DVDAttack()
    {
        GameObject dvdLogo = new GameObject("dvdLogo",typeof(Rigidbody2D),typeof(BoxCollider2D),typeof(SpriteRenderer));
        dvdLogo.transform.parent = transform.parent.parent;
        dvdLogo.transform.position = startPos + Vector3.left*7.5f + Vector3.up *4f;
        dvdLogo.layer = LayerMask.NameToLayer("Projectiles");
        dvdLogo.tag = "Enemy Projectile";
        dvdLogo.GetComponent<SpriteRenderer>().sprite = dvdSprite;
        dvdLogo.GetComponent<SpriteRenderer>().sortingOrder = 40;
        dvdLogo.GetComponent<Rigidbody2D>().gravityScale = 0;
        dvdLogo.GetComponent<Rigidbody2D>().mass = 1;
        dvdLogo.GetComponent<Rigidbody2D>().velocity = Quaternion.AngleAxis(Random.Range(-45,45),Vector3.forward) * Vector3.right * dvdSpeed;//Random.insideUnitCircle * dvdSpeed;
        dvdLogo.GetComponent<BoxCollider2D>().size = new Vector2(2,1.5f);
        dvdLogo.GetComponent<BoxCollider2D>().isTrigger = true;
        Destroy(dvdLogo,phase2Length);
        StartCoroutine(DVDLife(dvdLogo));
        yield return null;
    }

    IEnumerator DVDLife(GameObject obj)
    {
        float i;
        Rigidbody2D objBody = obj.GetComponent<Rigidbody2D>();
        for(i=0;i<=phase2Length;i+=0.025f)
        {
            if(Physics2D.OverlapBox(obj.transform.position,new Vector2(2,1.5f),layers) != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(obj.transform.position,objBody.velocity,1,layers);
                if(hit.normal != null)
                {
                    objBody.velocity = Vector2.Reflect(objBody.velocity,hit.normal);
                    //Debug.Log(hit.transform.name);
                }
            }
            yield return new WaitForSeconds(0.025f);
        }
    }

    IEnumerator Phase3()
    {
        wiggleScript.sinStrength = Vector3.zero;
        wiggleScript.cosStrength = Vector3.zero;
        wiggleScript.speed = Vector2.zero;
        inPhase = true;
        StartCoroutine(HorseAttack());
        yield return new WaitForSeconds(phase3Length);
        phase = 1;
        inPhase = false;
    }

    IEnumerator HorseAttack()
    {
        GameObject soundObj = new GameObject("horseSounds",typeof(AudioSource));
        AudioSource soundSource = soundObj.GetComponent<AudioSource>();

        soundSource.clip = neighhSound;
        soundSource.Play();
        int dir = Random.Range(0,2);
        yield return new WaitForSeconds(2);
        soundSource.clip = gallopSound;
        soundSource.Play();
        Destroy(soundObj,phase1Length+1);
        switch(dir)
        {
            case 0:
                int i;
                for(i=1;i<=3;i++)
                {
                    GameObject horse = Instantiate(horseBullet,startPos + Vector3.right * 9 + Vector3.up * (i-2),Quaternion.identity);
                    Rigidbody2D rbody = horse.GetComponent<Rigidbody2D>();
                    rbody.velocity = -Vector3.right * horseSpeed;
                    rbody.gravityScale = 0;
                    rbody.mass = 1;
                    rbody.velocity = -Vector3.right * horseSpeed;
                    //horse.GetComponent<SpriteRenderer>().flipX = true;
                    Destroy(horse,phase3Length-2);
                }
                break;
            case 1:
                int j;
                for(j=1;j<=3;j++)
                {
                    GameObject horse = Instantiate(horseBullet,startPos - Vector3.right * 9 + Vector3.up * (j-2),Quaternion.identity);
                    Rigidbody2D rbody = horse.GetComponent<Rigidbody2D>();
                    rbody.gravityScale = 0;
                    rbody.mass = 1;
                    rbody.velocity = Vector3.right * horseSpeed;
                    horse.GetComponent<SpriteRenderer>().flipX = true;
                    Destroy(horse,phase3Length-2);
                }
                break;
        }
    }


}
