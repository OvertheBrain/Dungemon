using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public bool ignite = false;
    private float duration = 2;
    private Animator anim;

    private AudioSource boom; 
    // Start is called before the first frame update
    void Start()
    {
        boom = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        if (ignite)
        {
            StartCoroutine(warn());
            StartCoroutine(blast());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator warn()
    {
        yield return new WaitForSeconds(duration - 0.5f);
        GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
    }

    IEnumerator blast()
    {
        yield return new WaitForSeconds(duration);
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
        boom.Play();
        this.gameObject.tag = "Explosion";
        anim.SetTrigger("Explosion");
        yield return new WaitForSeconds(0.9f);
        Destroy(gameObject);
    }

}
