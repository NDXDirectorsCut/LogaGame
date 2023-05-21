using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SeedGetScript : MonoBehaviour
{
    public RoomGeneratorScript seedCarrier;
    public TextMeshProUGUI textBox;
    // Start is called before the first frame update
    void Start()
    {
        textBox.text = seedCarrier.seed.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
