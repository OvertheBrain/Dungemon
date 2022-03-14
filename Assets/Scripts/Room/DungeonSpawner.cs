using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    public enum Modes { Start, Normal, Skeleton, Eye, Goblin, Mushroom, Trap, End}
    private Modes SpawnMode;
    private Room room;

    private int frogNum = 8, frogBossNum = 5, bossNum = 1, itemNum = 5;
    private bool bossAlive;

    public GameObject Frog, Skeleton, Eye, Goblin, Mushroon, Dragon;
    public GameObject food, exp, bomb, cool;
    // Start is called before the first frame update
    void Start()
    {
        room = this.gameObject.GetComponent<Room>();
        SpawnMode = (Modes)room.RoomType;
    }

    // Update is called once per frame
    void Update()
    {
        if (room.StartSpawn)
            Spawn();
        
        
    }

    private void Spawn()
    {
        if(SpawnMode == Modes.Normal)
        {
            if (frogNum == 0) return;
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = Random.Range(spawnPosition.y-2.5f, spawnPosition.y+2.5f);
            spawnPosition.x= Random.Range(spawnPosition.x - 6f, spawnPosition.x + 6f);
            Instantiate(Frog, spawnPosition, Quaternion.identity);
            frogNum--;
        }
        if (SpawnMode == Modes.Goblin)
        {
            if (bossNum == 0) return;
            if (frogBossNum > 0)
            {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = Random.Range(spawnPosition.y - 2.5f, spawnPosition.y + 2.5f);
                spawnPosition.x = Random.Range(spawnPosition.x - 6f, spawnPosition.x + 6f);
                Instantiate(Frog, spawnPosition, Quaternion.identity);
                frogBossNum--;
            }
            if(frogBossNum == 0)
            {
                Instantiate(Goblin, transform.position, Quaternion.identity);
                bossNum--;
            }
        }
        if (SpawnMode == Modes.Eye)
        {
            if (bossNum == 0) return;
            if (frogBossNum > 0)
            {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = Random.Range(spawnPosition.y - 2.5f, spawnPosition.y + 2.5f);
                spawnPosition.x = Random.Range(spawnPosition.x - 6f, spawnPosition.x + 6f);
                Instantiate(Frog, spawnPosition, Quaternion.identity);
                frogBossNum--;
            }
            if (frogBossNum == 0)
            {
                Instantiate(Eye, transform.position, Quaternion.identity);
                bossNum--;
            }
        }
        if (SpawnMode == Modes.Mushroom)
        {
            if (bossNum == 0) return;
            if (frogBossNum > 0)
            {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = Random.Range(spawnPosition.y - 2.5f, spawnPosition.y + 2.5f);
                spawnPosition.x = Random.Range(spawnPosition.x - 6f, spawnPosition.x + 6f);
                Instantiate(Frog, spawnPosition, Quaternion.identity);
                frogBossNum--;
            }
            if (frogBossNum == 0)
            {
                Instantiate(Mushroon, transform.position, Quaternion.identity);
                bossNum--;
            }
        }
        if (SpawnMode == Modes.Skeleton)
        {
            if (bossNum == 0) return;
            if (frogBossNum > 0)
            {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = Random.Range(spawnPosition.y - 2.5f, spawnPosition.y + 2.5f);
                spawnPosition.x = Random.Range(spawnPosition.x - 6f, spawnPosition.x + 6f);
                Instantiate(Frog, spawnPosition, Quaternion.identity);
                frogBossNum--;
            }
            if (frogBossNum == 0)
            {
                Instantiate(Skeleton, transform.position, Quaternion.identity);
                bossNum--;
            }
        }
        if (SpawnMode == Modes.Trap)
        {
            if (itemNum == 0) return; 
            
            if (itemNum > 0)
            {
                int prop = Random.Range(0, 20);
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = Random.Range(spawnPosition.y - 4f, spawnPosition.y + 4f);
                spawnPosition.x = Random.Range(spawnPosition.x - 7f, spawnPosition.x + 7f);
                if (prop <= 8)
                    Instantiate(food, spawnPosition, Quaternion.identity);
                if (prop > 8 && prop <= 13)
                    Instantiate(bomb, spawnPosition, Quaternion.identity);
                else if(prop >=15)
                    Instantiate(exp, spawnPosition, Quaternion.identity);
                itemNum--;
            }
        }
        if(SpawnMode == Modes.End)
        {
            if (GameManager.instance.end)
            {
                Vector3 spawnPosition = transform.position;
                spawnPosition.y = Random.Range(spawnPosition.y - 4f, spawnPosition.y + 4f);
                spawnPosition.x = Random.Range(spawnPosition.x - 7f, spawnPosition.x + 7f);
                int prop = Random.Range(0, 2100);
                if (prop == 1)
                    Instantiate(cool, spawnPosition, Quaternion.identity);
                if (prop == 2)
                    Instantiate(bomb, spawnPosition, Quaternion.identity);
                if (prop == 3)
                    Instantiate(food, spawnPosition, Quaternion.identity);
                
                if (bossNum == 0) return;
                Instantiate(Dragon, transform.position, Quaternion.identity);
                bossNum--;              
            }
        }
    }
}
