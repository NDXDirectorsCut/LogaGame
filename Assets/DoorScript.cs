using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    
    //BoxCollider2D collider;
    public Sprite closedDoorSprite;
    GameObject closedDoor;
    GameObject closedTree;
    public GameObject enemyList;
    bool isEnabled = true;
    float fadeTime = .25f;
    // Start is called before the first frame update
    void Awake()
    {
        // Debug.Log(transform.root.name);
        enemyList = transform.parent.parent.Find("Enemies").gameObject;//.GetComponent<EnemyHandler>();
        closedDoor = new GameObject("closedDoor",typeof(SpriteRenderer),typeof(BoxCollider2D));
        closedDoor.GetComponent<SpriteRenderer>().sprite = transform.parent.Find("grass").GetComponent<SpriteRenderer>().sprite;
        closedDoor.GetComponent<SpriteRenderer>().sortingOrder = 20;
        closedDoor.GetComponent<BoxCollider2D>().size = new Vector2(1,1);
        closedDoor.transform.position = transform.position;
        closedDoor.transform.up = transform.up;
        closedDoor.transform.parent = transform.parent;
        if(transform.parent.Find("copac"))
        {
        closedTree = new GameObject("closedTree",typeof(SpriteRenderer));
        closedTree.GetComponent<SpriteRenderer>().sprite = transform.parent.Find("copac").GetComponent<SpriteRenderer>().sprite;
        closedTree.GetComponent<SpriteRenderer>().sortingOrder = 20;
        closedTree.transform.position = closedDoor.transform.position - transform.up;
        closedTree.transform.up = transform.up;
        closedTree.transform.parent = transform.parent;
        }
    }

    void Update()
    {
        if(enemyList.transform.childCount != 0)
        {
            isEnabled = false;
            closedDoor.SetActive(true);
            if(closedTree != null)
                closedTree.SetActive(true);
        }
        else
        {
            isEnabled = true;
            if(closedTree != null)
                closedTree.SetActive(false);
            closedDoor.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if(isEnabled == true)
        {
            if(col.tag == "Player")
            {
                Debug.Log(col.name);
                Rigidbody2D rbody = col.gameObject.GetComponent<Rigidbody2D>();
                Transform cam = col.transform.parent.Find("Main Camera");
                StartCoroutine(EnterDoor(rbody,col.transform,cam));

            }
        }
        
    }

    IEnumerator EnterDoor(Rigidbody2D rbody, Transform trans, Transform cam)
    {
        CameraScript camScript = cam.gameObject.GetComponent<CameraScript>();
        BaseMovement movScript = trans.gameObject.GetComponent<BaseMovement>();
        transform.root.GetChild(movScript.roomNumber).gameObject.SetActive(false);
        movScript.roomNumber += 1;
        transform.root.GetChild(movScript.roomNumber).gameObject.SetActive(true);
        camScript.target = transform.root.GetChild(movScript.roomNumber);
        //Time.timeScale = 0;
        //rbody.simulated = false;
        trans.position += -transform.up * 3;
        yield return new WaitForSecondsRealtime(.25f);

        //Debug.Log("Moved");
        //trans.position += -transform.up * 3;
        //rbody.simulated = true;
    }
}
