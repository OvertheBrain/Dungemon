using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int gender;
    public string name;

    public bool end = false;
    public bool GameOver = false, GameWin = false;
    public int BossDefeat = 0;
    public int score = 0;
    public int life;
    public float CurrentCooldown, coolDown;

    public GameObject StartUI, PlayerUI, EndUI;
    public GameObject Player_BOY, Player_GIRL;

    public enum states { Faint, Burn, Normal, Dead, Cool };
    public states status;


    static public GameManager instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (BossDefeat >= 4)
            end = true;
        if (GameOver || GameWin) EndGame();
    }

    public void StartGame() {
        StartUI.SetActive(false);
        if(gender == 0)
            Instantiate(Player_BOY, Vector3.zero, Quaternion.identity);
        else
            Instantiate(Player_GIRL, Vector3.zero, Quaternion.identity);
        PlayerUI.SetActive(true);
    }

    public void EndGame()
    {
        EndUI.SetActive(true);
    }

}
