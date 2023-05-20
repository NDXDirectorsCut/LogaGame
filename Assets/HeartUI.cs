using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartUI : MonoBehaviour
{
    public BaseMovement playerScript;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int i;
        for(i=0;i<4;i++)
        {
            //Sprite heartSprite = transform.GetChild(i).GetComponent<Image>().sprite;
            if(i>playerScript.hp-1)
            {
                transform.GetChild(i).GetComponent<Image>().sprite = emptyHeart;
            }
            else
            {
                transform.GetChild(i).GetComponent<Image>().sprite = fullHeart;
            }
        }
    }
}
