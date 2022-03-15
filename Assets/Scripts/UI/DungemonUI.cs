using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungemonUI : MonoBehaviour
{
    public Text Level, score, life, Status;
    public Image SkillMask1, SkillMask2, SkillMask3;
    public Image DeadMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        life.text = $"X{GameManager.instance.dun.life.ToString()}";
        Level.text = $"lv. {GameManager.instance.dun.level.ToString()}";
        score.text = $"Score: {GameManager.instance.dun.score.ToString()}";
        SkillMask1.fillAmount = 1 - GameManager.instance.dun.currentCooldown1 / GameManager.instance.dun.cooldown1;
        SkillMask2.fillAmount = 1 - GameManager.instance.dun.currentCooldown2 / GameManager.instance.dun.cooldown2;
        SkillMask3.fillAmount = 1 - GameManager.instance.dun.currentCooldown3 / GameManager.instance.dun.cooldown3;
        DeadMask.fillAmount = 1 - GameManager.instance.dun.dead / GameManager.instance.dun.deathCooldown;
        Status.text = $"{GameManager.instance.dun.status.ToString()}";
    }
}
