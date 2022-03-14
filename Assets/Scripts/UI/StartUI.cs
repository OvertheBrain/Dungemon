using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public GameObject Male;
    public GameObject Female;
    public GameObject GameManager;

    public AudioSource nameSelected;
    // Start is called before the first frame update
    void Start()
    {
        nameSelected = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeGender()
    {
        int gender = this.gameObject.GetComponent<Dropdown>().value;
        if (gender == 0)
        {
            Male.SetActive(true);
            Female.SetActive(false);
            GameManager.GetComponent<GameManager>().gender = 0;
        }
        else if(gender == 1)
        {
            Male.SetActive(false);
            Female.SetActive(true);
            GameManager.GetComponent<GameManager>().gender = 1;
        }
    }

    public void ChangeName()
    {
        string InputName = GameObject.Find("InputField").GetComponent<InputField>().text;
        GameManager.GetComponent<GameManager>().name = InputName;
        nameSelected.Play();
    }
}
