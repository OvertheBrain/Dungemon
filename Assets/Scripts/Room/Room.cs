using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject doorup, doordown, doorleft, dooright;
    public int RoomType;

    public bool roomup, roomdown, roomleft, roomright;

    public bool StartSpawn;

    public int doorNum;

    public int stepToStart;//距离初始点的网格距离
    // Start is called before the first frame update
    void Start()
    {
        doordown.SetActive(roomdown);
        dooright.SetActive(roomright);
        doorup.SetActive(roomup);
        doorleft.SetActive(roomleft);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateRoom(int xoffset, int yoffset)
    {
        stepToStart = (int)(Mathf.Abs(transform.position.x / xoffset) + Mathf.Abs(transform.position.y / yoffset));

        if (roomup)
            doorNum++;
        if (roomdown)
            doorNum++;
        if (roomleft)
            doorNum++;
        if (roomright)
            doorNum++;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            CameraController.instance.ChangeTarget(transform);
            StartSpawn = true;
        }
    }


}
