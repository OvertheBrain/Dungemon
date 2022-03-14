using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    public Text Info, Score;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.GameWin)
            Info.text = "You win.";
        else
            Info.text = "You die.";
        Score.text = $"Score: {GameManager.instance.score.ToString()}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }
}
