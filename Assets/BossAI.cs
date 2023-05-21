using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{

    public int phase;
    public bool inPhase;

    public WiggleAnimScript wiggleScript;

    public float phase1Length;
    public float phase2Length;
    public float phase3Length;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(phase == 1 && inPhase == false)
        {
            StartCoroutine(Phase1());
        }
        if(phase == 2 && inPhase == false)
        {
            StartCoroutine(Phase2());
        }

    }

    IEnumerator Phase1()
    {
        wiggleScript.speed = new Vector2(5,0);
        yield return new WaitForSeconds(phase1Length);

    }

    IEnumerator Phase2()
    {
        yield return new WaitForSeconds(phase2Length);
    }

}
