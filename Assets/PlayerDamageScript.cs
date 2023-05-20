using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageScript : MonoBehaviour
{
    public BaseMovement playerScript;
    //float hp;
    
    public string weakTo;
    public float invincibilityDuration = 1;
    public float invLength = .1f;
    Rigidbody2D rbody;
    public GameObject spriteRoot;
    //public float knockback;
    // Start is called before the first frame update
    void Awake()
    {
        rbody = playerScript.gameObject.GetComponent<Rigidbody2D>();
        //spriteRoot = playerScript.gameObject.Find("SpriteRoot");
    }

    // Update is called once per frame
    void Update()
    {
        if(playerScript.hp <= 0)
        {
            //playerScript.gameObject.SetActive(false);
            playerScript.canFire = false;
            rbody.simulated = false;
        }
    }
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Projectiles") && col.gameObject.tag == weakTo && playerScript.invincible == false)
        {
            Rigidbody2D cbody = col.gameObject.GetComponent<Rigidbody2D>();

            Debug.Log(cbody.name);
            StartCoroutine(TakeDamage(cbody.mass));
            StartCoroutine(InvincibilityAnim());
            ///rbody.velocity = cbody.velocity.normalized * knockback;
            //playerScript.hp = playerScript.hp - cbody.mass;
        }
    }

    IEnumerator TakeDamage(float damage)
    {
        playerScript.hp = playerScript.hp - damage;
        if(playerScript.hp > 0)
        {
            playerScript.invincible = true;
            playerScript.headAnim.SetBool("Hurt",true);
        }
        //StartCoroutine(InvincibilityAnim());
        yield return new WaitForSeconds(invincibilityDuration);
        playerScript.invincible = false;
        playerScript.headAnim.SetBool("Hurt",false);
    }

    IEnumerator InvincibilityAnim()
    {
        while(spriteRoot.activeSelf == true && playerScript.invincible == true)
        {
            playerScript.headAnim.SetBool("Hurt",true);
            yield return new WaitForSeconds(invLength);
            spriteRoot.SetActive(false);
            yield return new WaitForSeconds(invLength);
            spriteRoot.SetActive(true);
        }
    }
}
