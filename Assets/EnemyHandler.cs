using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public float enemyCount;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);
        StartCoroutine(EnableEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = transform.childCount;
    }

    IEnumerator EnableEnemies()
    {
        int i;
        yield return new WaitForSeconds(0.005f);
        for(i=0;i<enemyCount;i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
