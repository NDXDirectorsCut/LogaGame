using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageScript : MonoBehaviour
{
    public float hp = 1000;
    public BossAI aiScript;
    public bool invincible;
    public SpriteRenderer spriteRend;
    public WiggleAnimScript wiggleScript;

    Vector2 prevSpeed;
    Vector3 prevSinStrength;

    public float hp_copy = 1000;

    // Start is called before the first frame update
    void Start()
    {
        hp_copy = hp;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp_copy != hp)
        {
            hp_copy = hp;
            StartCoroutine(DamageEff());
        }

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player Projectile")
        {
            Rigidbody2D colBody = col.GetComponent<Rigidbody2D>();
            hp = hp - colBody.mass;
        }
    }

    IEnumerator DamageEff()
    {
        spriteRend.color = new Color(1,0.4f,0.4f);
        yield return new WaitForSeconds(0.25f);
        spriteRend.color = Color.white;
    }
}
