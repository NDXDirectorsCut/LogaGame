using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    BoxCollider2D collider;
    bool isEnabled = true;
    float fadeTime = .25f;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.root.name);
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
        //rbody.simulated = false;
        trans.position += -transform.up * 3;
        yield return new WaitForSeconds(.25f);
        Debug.Log("Moved");
        trans.position += -transform.up * 3;
        rbody.simulated = true;
    }
}
