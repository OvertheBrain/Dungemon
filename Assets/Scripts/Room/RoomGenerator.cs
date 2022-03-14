using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallType
{
    public GameObject singleLeft, singleRight, singleUp, singleBottom,
                      doubleLU, doubleLR, doubleLB, doubleUR, doubleUB, doubleRB,
                      tripleLUR, tripleLUB, tripleURB, tripleLRB,
                      fourDoors;
}

public class RoomGenerator : MonoBehaviour
{
    public enum Direction { up, down, right, left };
    public Direction direction;

    public GameObject roomPrefab;
    public int roomNumber;
    public Color endColor, gangColor;
    private GameObject EndRoom;

    public Transform GeneratorPoint;
    public float xOffset, yOffset;
    public LayerMask roomLayer;
    private int maxStep;

    public List<Room> rooms = new List<Room>();
    public List<GameObject> walls = new List<GameObject>();

    List<GameObject> farRooms = new List<GameObject>();
    List<GameObject> oneWayRooms = new List<GameObject>();
    List<GameObject> lessFarRooms = new List<GameObject>();

    private int EndIndex;
    public WallType wallType;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < roomNumber; i++)
        {
            rooms.Add(Instantiate(roomPrefab, GeneratorPoint.position, Quaternion.identity).GetComponent<Room>());
            ChangePointPos();
        }

        EndRoom = rooms[0].gameObject;
        foreach(var room in rooms)
        {
            //if (room.transform.position.sqrMagnitude > EndRoom.transform.position.sqrMagnitude)
            //{
            //    EndRoom = room.gameObject;
            //}
            SetupRoom(room, room.transform.position);
        }

        EndIndex = FindEndRoom();

        SetRoomType();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangePointPos()
    {
        do {

            direction = (Direction)Random.Range(0, 4);

            switch (direction)
            {
                case Direction.up:
                    GeneratorPoint.position += new Vector3(0, yOffset, 0);
                    break;
                case Direction.down:
                    GeneratorPoint.position += new Vector3(0, -yOffset, 0);
                    break;
                case Direction.right:
                    GeneratorPoint.position += new Vector3(xOffset, 0, 0);
                    break;
                case Direction.left:
                    GeneratorPoint.position += new Vector3(-xOffset, 0, 0);
                    break;
            }

        } while (Physics2D.OverlapCircle(GeneratorPoint.position, 0.2f, roomLayer));
    }

    public void SetupRoom(Room newroom, Vector3 roomPosition)
    {
        newroom.roomup = Physics2D.OverlapCircle(roomPosition + new Vector3(0, yOffset, 0), 0.2f, roomLayer);
        newroom.roomdown = Physics2D.OverlapCircle(roomPosition + new Vector3(0, -yOffset, 0), 0.2f, roomLayer);
        newroom.roomleft = Physics2D.OverlapCircle(roomPosition + new Vector3(-xOffset, 0, 0), 0.2f, roomLayer);
        newroom.roomright = Physics2D.OverlapCircle(roomPosition + new Vector3(xOffset, 0, 0), 0.2f, roomLayer);

        newroom.UpdateRoom((int)xOffset, (int)yOffset);


        switch (newroom.doorNum)
        {
            case 1:
                if (newroom.roomup)
                    walls.Add(Instantiate(wallType.singleUp, roomPosition, Quaternion.identity));
                if (newroom.roomdown)
                    walls.Add(Instantiate(wallType.singleBottom, roomPosition, Quaternion.identity));
                if (newroom.roomleft)
                    walls.Add(Instantiate(wallType.singleLeft, roomPosition, Quaternion.identity));
                if (newroom.roomright)
                    walls.Add(Instantiate(wallType.singleRight, roomPosition, Quaternion.identity));
                break;
            case 2:
                if (newroom.roomleft && newroom.roomup)
                    walls.Add(Instantiate(wallType.doubleLU, roomPosition, Quaternion.identity));
                if (newroom.roomleft && newroom.roomright)
                    walls.Add(Instantiate(wallType.doubleLR, roomPosition, Quaternion.identity));
                if (newroom.roomleft && newroom.roomdown)
                    walls.Add(Instantiate(wallType.doubleLB, roomPosition, Quaternion.identity));
                if (newroom.roomup && newroom.roomright)
                    walls.Add(Instantiate(wallType.doubleUR, roomPosition, Quaternion.identity));
                if (newroom.roomup && newroom.roomdown)
                    walls.Add(Instantiate(wallType.doubleUB, roomPosition, Quaternion.identity));
                if (newroom.roomright && newroom.roomdown)
                    walls.Add(Instantiate(wallType.doubleRB, roomPosition, Quaternion.identity));
                break;
            case 3:
                if (newroom.roomleft && newroom.roomup && newroom.roomright)
                    walls.Add(Instantiate(wallType.tripleLUR, roomPosition, Quaternion.identity));
                if (newroom.roomleft && newroom.roomright && newroom.roomdown)
                    walls.Add(Instantiate(wallType.tripleLRB, roomPosition, Quaternion.identity));
                if (newroom.roomdown && newroom.roomup && newroom.roomright)
                    walls.Add(Instantiate(wallType.tripleURB, roomPosition, Quaternion.identity));
                if (newroom.roomleft && newroom.roomup && newroom.roomdown)
                    walls.Add(Instantiate(wallType.tripleLUB, roomPosition, Quaternion.identity));
                break;
            case 4:
                if (newroom.roomleft && newroom.roomup && newroom.roomright && newroom.roomdown)
                    walls.Add(Instantiate(wallType.fourDoors, roomPosition, Quaternion.identity));
                break;
        }

    }

    private void SetRoomType()
    {
        rooms[0].RoomType = 0;
        rooms[1].RoomType = 1;

        int currentModel = 2;
        List<int> tmp = new List<int>();

        rooms[EndIndex].RoomType = 7;
        walls[EndIndex].GetComponentInChildren<SpriteRenderer>().color = endColor;
        EndRoom.GetComponent<SpriteRenderer>().color = endColor;

        do
        {
            int index = Random.Range(2, 9);
            if(!tmp.Contains(index) && index != EndIndex)
            {
                rooms[index].RoomType = currentModel;
                walls[index].GetComponentInChildren<SpriteRenderer>().color = gangColor;
                currentModel++;
                tmp.Add(index);
            }
            
        } while (currentModel < 6);

        for(int i=2; i<rooms.Count; i++)
        {
            if(!tmp.Contains(i) && i != EndIndex)
            {
                int rd = Random.Range(0, 2);
                if (rd == 0)
                    rooms[i].RoomType = 1;
                else
                    rooms[i].RoomType = 6;
            }
        }
    }

    public int FindEndRoom()
    {
        //最大数值 最远距离数字
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].stepToStart > maxStep)
                maxStep = rooms[i].stepToStart;
        }

        //获得最远房间和第二远
        foreach (var room in rooms)
        {
            if (room.stepToStart == maxStep)
                farRooms.Add(room.gameObject);
            if (room.stepToStart == maxStep - 1)
                lessFarRooms.Add(room.gameObject);
        }

        for (int i = 0; i < farRooms.Count; i++)
        {
            if (farRooms[i].GetComponent<Room>().doorNum == 1)
                oneWayRooms.Add(farRooms[i]);//最远房间里的单侧门加入
        }

        for (int i = 0; i < lessFarRooms.Count; i++)
        {
            if (lessFarRooms[i].GetComponent<Room>().doorNum == 1)
                oneWayRooms.Add(lessFarRooms[i]);//第二远远房间里的单侧门加入
        }

        if (oneWayRooms.Count != 0)
        {
            EndRoom = oneWayRooms[Random.Range(0, oneWayRooms.Count)];
        }
        else
        {
            EndRoom = farRooms[Random.Range(0, farRooms.Count)];
        }

        int roomcount = 0;
        foreach (var room in rooms)
        {
            if(room.transform.position == EndRoom.transform.position)
            {
                break;
            }
            roomcount++;
        }

        return roomcount;
    }
}
