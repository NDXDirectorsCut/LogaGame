using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsChange : MonoBehaviour
{
    public BaseMovement playerScript;
    public Button itemButton;

    public float speed;
    public bool sMultiply;

    public float fireDelay; 
    public bool fdMultiply;

    public float damage;
    public bool dMultiply;

    public float projectileSpeed;
    public bool psMultiply;

    public AudioClip fireSound;
    public float projectileSize;
    
    public Sprite tearSprite;
    public GameObject hitParticle;
    
    public LayerMask layers;    
    // Start is called before the first frame update
    void Start()
    {
        Button btn = gameObject.GetComponent<Button>();
		btn.onClick.AddListener(ClickEvent);
    }

    // Update is called once per frame
    void ClickEvent()
    {
        //playerScript.speed = sMultiply ? playerScript.speed * speed : playerScript.speed + speed;

        playerScript.fireDelay = fdMultiply ? playerScript.fireDelay * fireDelay : playerScript.fireDelay + fireDelay;

        playerScript.projectileSpeed = psMultiply ? playerScript.projectileSpeed * projectileSpeed : playerScript.projectileSpeed + projectileSpeed;

        playerScript.damage = dMultiply ? playerScript.damage * damage : playerScript.damage + damage;

        playerScript.projectileSize = projectileSize;

        playerScript.tearSprite = tearSprite;
        
        playerScript.hitParticle = hitParticle;

        playerScript.layers = layers;

        Time.timeScale = 1;
        transform.parent.parent.gameObject.SetActive(false);
    }
}
