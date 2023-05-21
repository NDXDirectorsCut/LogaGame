using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseMovement : MonoBehaviour
{
    Rigidbody2D rbody;
    public AudioSource music;
    public AudioClip bossTheme;
    public AudioClip pauseTheme;
    public AudioClip normalTheme;
    public Animator headAnim;
    public Animator bodyAnim;
    public int roomNumber = 0;
    int roomNumber_copy = 0;
    public GameObject cardUI;
    public RoomGeneratorScript roomGen;

    public GameObject deathPanel;
    public GameObject winPanel;
    //public GameObject bossRoom;
    public BossDamageScript bossHealthRef;
    public float bossHealth;

    public List<GameObject> items = new List<GameObject>();

    [Header("Movement")]
    public float hp; // start HP
    public float acc;
    public float speed;
    public bool invincible;
    public bool monke;
    public bool fat;
    [Space(10)]

    [Header("Attacking")]
    public bool canFire;
    public bool spinBullet;
    public float fireDelay; //start firing rate
    public float damage;
    public float range; // start Range
    public float projectileSpeed;
    public AudioClip fireSound;
    public float projectileSize;
    public float randomization;
    public LayerMask layers;
    public Sprite tearSprite;
    public GameObject hitParticle;
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
    void Awake()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        Debug.Log(transform.root);
        //sound = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float lHor = smoothInput ? Input.GetAxis("LeftHorizontal") : Input.GetAxisRaw("LeftHorizontal");
        float lVer = smoothInput ? Input.GetAxis("LeftVertical") : Input.GetAxisRaw("LeftVertical");
        float rHor = Input.GetAxisRaw("RightHorizontal");
        float rVer = Input.GetAxisRaw("RightVertical");
        bossHealthRef = roomGen.map.transform.Find("BossRoom(Clone)").Find("Enemies").Find("BOSS").Find("cal").GetComponent<BossDamageScript>();
        bossHealth = bossHealthRef.hp;
        //Debug.Log();
        //bossHealth = bossRoom.Find("Enemies").Find("BOSS").Find("cal");
        if(bossHealth<=0)
        {
            invincible = true;
            winPanel.SetActive(true);
            winPanel.GetComponent<Image>().color = Color.Lerp(winPanel.GetComponent<Image>().color,new Color(1,1,1,1),0.01f);
        }


        headAnim.SetBool("Monke",monke);
        bodyAnim.SetBool("Fat",fat);

        leftAxis = Vector3.ClampMagnitude(new Vector3(lHor,lVer,0),1);
        rightAxis = new Vector3(rHor,rVer,0);
        
        rbody.velocity += new Vector2(leftAxis.x,leftAxis.y) * acc;
        rbody.velocity = Vector3.ClampMagnitude(rbody.velocity,speed);

        if(leftAxis.magnitude > 0.1f)
        {
            headAnim.SetFloat("Horizontal", leftAxis.x);
            headAnim.SetFloat("Vertical",leftAxis.y);
            bodyAnim.SetBool("Walk",true);
        }
        else
        {
            rbody.velocity = Vector3.Lerp(rbody.velocity,Vector3.zero,0.2f);
            bodyAnim.SetBool("Walk",false);
        }

        if(rightAxis.magnitude > 0.1f)
        {

            StartCoroutine(Attack());
            headAnim.SetFloat("Horizontal", rightAxis.x);
            headAnim.SetFloat("Vertical",rightAxis.y);
            headAnim.SetBool("RightStick",true);
        }
        else
        {
            headAnim.SetBool("RightStick",false);
        }
        
        if(drawDebug == true)
        {
            Debug.DrawRay(transform.position,leftAxis,Color.blue);
            Debug.DrawRay(transform.position,rightAxis,Color.red);
        }

        if(roomNumber_copy!=roomNumber)
        {
            StartCoroutine(ChangeRoom());
            roomNumber_copy = roomNumber;
        }

    }

    IEnumerator Attack()
    {
        if(canFire == true)
        {
            GameObject tear = new GameObject("tear", typeof(SpriteRenderer), typeof(Rigidbody2D),typeof(CircleCollider2D));
            GameObject tearSound = new GameObject("tearSound",typeof(AudioSource));

            tear.layer = LayerMask.NameToLayer("Projectiles");
            tear.tag = "Player Projectile";
            tear.GetComponent<SpriteRenderer>().sprite = tearSprite;
            tear.GetComponent<SpriteRenderer>().sortingOrder = 10;
            if(spinBullet == false)
                tear.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            if(spinBullet == true)
                tear.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(1440,2880);
            tear.GetComponent<Rigidbody2D>().gravityScale = 0;
            tear.GetComponent<Rigidbody2D>().mass = damage;
            tear.GetComponent<Rigidbody2D>().velocity = rightAxis.normalized * projectileSpeed;
            tear.GetComponent<CircleCollider2D>().radius = projectileSize;
            tear.GetComponent<CircleCollider2D>().isTrigger = true;
            tearSound.GetComponent<AudioSource>().clip = fireSound;
            tearSound.GetComponent<AudioSource>().Play();
            tear.transform.position = transform.position + new Vector3(Random.Range(-randomization,randomization),Random.Range(-randomization,randomization),0);
            StartCoroutine(TearLife(tear,0.1f,rightAxis.normalized));
            Destroy(tear,range);
            Destroy(tearSound,range);
            //Debug.Log("Fire Tear");
            canFire = false;

            yield return new WaitForSeconds(fireDelay);
            canFire = true;
            yield return new WaitForSeconds(range-fireDelay-.1f);
            GameObject breakPart = Instantiate(hitParticle,tear.transform.position,Quaternion.identity);
            ParticleSystem particle = breakPart.GetComponent<ParticleSystem>();
            Destroy(breakPart,particle.duration*20);
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
                    GameObject breakPart = Instantiate(hitParticle,obj.transform.position,Quaternion.identity);
                    ParticleSystem particle = breakPart.GetComponent<ParticleSystem>();
                    Destroy(breakPart,particle.duration*20);
                    Destroy(obj,0.025f);
                }
                yield return new WaitForSeconds(0.1f);
                k+=0.1f;
            }
        }
    }

    IEnumerator ChangeRoom()
    {
        if(roomNumber != roomGen.firstDark && roomNumber != roomGen.firstSwamp && roomNumber != 29)
        {
            Time.timeScale = 0.005f;
            cardUI.SetActive(false);
            //music.clip = normalTheme;
            yield return new WaitForSecondsRealtime(.6f); //Time stopu
            Time.timeScale = 1f;
            //hp = 4;
            Cursor.lockState = CursorLockMode.Locked;
            foreach (Transform child in cardUI.transform.Find("CardHolder") )
            {
                Destroy(child.gameObject);
            }
        }
        else
        {
            Random.InitState(roomGen.seed);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            music.clip = pauseTheme;
            music.Play();
            int card1ID = Random.Range(0,items.Count);

            GameObject card1 = Instantiate(items[card1ID],Vector3.zero,Quaternion.identity );
            card1.transform.parent = cardUI.transform.Find("CardHolder");
            card1.transform.localScale = Vector3.one;
            StatsChange card1Script = card1.GetComponent<StatsChange>();
            card1Script.playerScript = gameObject.GetComponent<BaseMovement>();
            items.Remove(items[card1ID]);

            int card2ID = Random.Range(0,items.Count);

            GameObject card2 = Instantiate(items[card2ID],Vector3.zero,Quaternion.identity );
            card2.transform.parent = cardUI.transform.Find("CardHolder");
            card2.transform.localScale = Vector3.one;
            StatsChange card2Script = card2.GetComponent<StatsChange>();
            card2Script.playerScript = gameObject.GetComponent<BaseMovement>();
            items.Remove(items[card2ID]);

            cardUI.SetActive(true);
            hp = 4;
        }
    }
}
