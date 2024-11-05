using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bulletScript : MonoBehaviour
{

    public float speed = 5f;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        rb.velocity = transform.right * speed * -1;
    }

    void FixedUpdate()
    {
        Destroy(gameObject, 0.5f);// This is so bullets don't travel forever
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        
        if(other.gameObject.CompareTag("zombie"))
        {
            ZombieScript zScript = other.gameObject.GetComponent<ZombieScript>();
            if(zScript.isVeryDead == false)//if zombie isn't dead yet
            {
                Animator animZombie = other.gameObject.GetComponent<Animator>();//This wan't working in the zombie script for some reason
                animZombie.SetBool("isDead", true);

                zScript.gotShot();

                Destroy(gameObject);
            }
        }
        else if(!other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("bullet"))
        {
            Destroy(gameObject);
        }

    }
}
