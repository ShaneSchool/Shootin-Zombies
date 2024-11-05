using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieScript : MonoBehaviour
{
    public float movespeed = 1.25f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    public GameObject bulletPrefab;

    public GameRules rules;

    Vector2 lookInput;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Vector2 playerLocation;

    float spawnTime;

    
    float angleRad;//This is used for the angle of the last direction of the player
    Vector2 moveGoal;// I use this for the direction to move

    bool isAwake = false;

    public bool isVeryDead = false;

    public AudioSource zombieDead;



    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rules = GameObject.FindAnyObjectByType<GameRules>();

        playerLocation = GameObject.FindGameObjectWithTag("Player").transform.position;

        movespeed = 0;//This is to stop it from moving while it's spawning
        spawnTime = Time.time;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isAwake)//If the zombie is still coming out of the ground
        {
            freshSpawn();
        }
        
        playerLocation = GameObject.FindGameObjectWithTag("Player").transform.position;
        
        angleRad = Mathf.Atan2(playerLocation.y-rb.position.y, playerLocation.x-rb.position.x);//This gets the angle that the zombie needs to move to touch the player
        moveGoal.x = Mathf.Cos(angleRad);
        moveGoal.y = Mathf.Sin(angleRad);

        bool canMove = false;
        if(moveGoal != Vector2.zero)
        {
             canMove = TryMove(moveGoal);

            if(!canMove)//tries to slide along x axis wall
            {
                canMove = TryMove(new Vector2 (moveGoal.x, 0));
            }

            if(!canMove)//tries to slide along y axis wall
            {
                canMove = TryMove(new Vector2 (0, moveGoal.y));
            }

            //animator.SetBool("isWalking", canMove);// if walking, walking anim
            
        }

        //setting the direction of the animations
        if(!isVeryDead)
        {       
            if(moveGoal.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if(moveGoal.x < 0)
            {
                spriteRenderer.flipX = false;
            }
        }
        
    }

    private bool TryMove(Vector2 direction)
    {
        int count = rb.Cast(
            direction,// X and Y values between -1 and 1 that represent the direction from the body to look for collisions
            movementFilter, //The settings that determine where a collision can occur on such as layers to collide with
            castCollisions, //List of collisions to store the found collisions into after the Cast is finished
            movespeed * Time.fixedDeltaTime + collisionOffset); //The amount to cast equal to the movement plus an offset
        if(count == 0)//movement occurs
        {
            rb.MovePosition(rb.position + direction * movespeed * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void gotShot()
    {
        //animator.SetBool("isDead", true); This didn't work, so I put it in the bulletscript
        movespeed = 0f;
        isVeryDead = true;
        zombieDead.Play();
        rules.deadZombies++;
        gameObject.GetComponent<Collider2D>().enabled = false;//This is so it doesn't eat more bullets when it's dead and people can walk over it
        Destroy(gameObject, 3f);
    }

    void freshSpawn()
    {
        if(Time.time > spawnTime + 1 && !isVeryDead)
        {
            movespeed = 1.25f;
            isAwake = true;
        }
    }

}
