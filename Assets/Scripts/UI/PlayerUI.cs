using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text PlayerName;
    public Text Life, Score, Status;
    public Image PlayerIcon, Skill, SkillMask;
    public Sprite Male, Female, KickBoard, Lightball;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        Life.text = $"X{GameManager.instance.life.ToString()}";
        SkillMask.fillAmount = 1 - GameManager.instance.CurrentCooldown / GameManager.instance.coolDown;
        Score.text = $"Score: {GameManager.instance.score.ToString()}";
        Status.text = $"{GameManager.instance.status.ToString()}";
    }

    private void Initialize()
    {
        PlayerName.text = GameManager.instance.name;
        if (GameManager.instance.gender == 0)
        {
            PlayerIcon.GetComponent<Image>().sprite = this.Male;
            Skill.GetComponent<Image>().sprite = this.KickBoard;
        }
        else if (GameManager.instance.gender == 1)
        {
            PlayerIcon.GetComponent<Image>().sprite = this.Female;
            Skill.GetComponent<Image>().sprite = this.Lightball;
        }

        GameManager.instance.status = GameManager.states.Normal;
    }
}
