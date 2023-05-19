using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGeneratorScript : MonoBehaviour
{
    int n;

    public int roomNumber;
    public int seed;
    public int firstDark;
    public int firstSwamp;
    public Vector3 position;
    public GameObject startRoom;
    public GameObject bossRoom;
    GameObject map;
    public GameObject[] forestRooms;
    public GameObject[] darkForestRooms;
    public GameObject[] swampRooms;

    // Start is called before the first frame update
    void Start()
    {
        map = new GameObject("Map");
        StartCoroutine(GenerateLayout());
        for(n=2;n<roomNumber;n++)
        {
            map.transform.GetChild(n).gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GenerateLayout()
    {
        Random.InitState(seed);
        //GameObject map;// = new GameObject("Map");
        int i,j; int prevDir = 0;
        GameObject room = startRoom;
        int dir = 0;
        position = Vector3.zero;
        for(i=1;i<=roomNumber;i++)
        {
            Vector3 roomPos = position;
            
            if(i==1)
            {
                room = Instantiate(startRoom,position,Quaternion.identity);
                room.transform.SetParent(map.transform);
            }

            if(i>1 && i <=firstDark)
            {
                int id = Random.Range(0,15);
                room = Instantiate(forestRooms[id],position,Quaternion.identity);
                room.transform.SetParent(map.transform);
                //Debug.Log("Generated Room with ID " + id);

            }

            if(i>firstDark && i <=firstSwamp)
            {
                int id = Random.Range(0,15);
                room = Instantiate(darkForestRooms[id],position,Quaternion.identity);
                room.transform.SetParent(map.transform);
                //Debug.Log("Generated Room with ID " + id);

            }

            if(i>firstSwamp && i <roomNumber)
            {
                int id = Random.Range(0,15);
                room = Instantiate(swampRooms[id],position,Quaternion.identity);
                room.transform.SetParent(map.transform);
                //Debug.Log("Generated Room with ID " + id);

            }

            if(i==roomNumber)
            {
                room = Instantiate(bossRoom,position,Quaternion.identity);
                room.transform.SetParent(map.transform);
                
            }

            dir = Random.Range(0,4);

                while( Physics2D.OverlapBox(position,new Vector2(14,8),0) == true)
                {
                    dir = Random.Range(0,4);
                    switch(dir)
                    {
                        case 0:
                            position = roomPos + Vector3.right * 15;
                            break;
                        case 1:
                            position = roomPos + Vector3.up * 9;
                            break;
                        case 2:
                            position = roomPos + Vector3.right * -15;
                            break;
                        case 3:
                            position = roomPos + Vector3.up * -9;
                            break;
                    }
                }

                for(j=0;j<4;j++)
                {
                    room.transform.Find("Wall"+j).gameObject.SetActive(true);
                    if(j==dir)
                    {
                        room.transform.Find("Wall"+j).gameObject.SetActive(false);
                        room.transform.Find("Door"+j).gameObject.SetActive(true);
                    }

                }
                prevDir = dir;
        }
        yield return null;
    }
}
