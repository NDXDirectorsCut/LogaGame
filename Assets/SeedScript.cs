using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedScript : MonoBehaviour
{
    [SerializeField]
    public static int seed = 0;
    public TMP_InputField textBox;  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(textBox!=null)
        {
            //Debug.Log(textBox.text);
            if(int.TryParse(textBox.text, out seed))
            {
                //Debug.Log(seed);
            }
        }
        //Debug.Log("Current Seed: " + seed);
    }
}
