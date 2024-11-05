using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class playercontroller : MonoBehaviour
{
    public float movespeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    public GameObject bulletPrefab;

    public GameObject crosshairPrefab;

    public int playerHealth = 10;

    GameObject crosshair;//This is the crosshair we will use in the game

    float angleRad = Mathf.Atan2(0, -1);//This is used for the angle of the last direction looked. I set it to be leftwards by default to match the crosshair
    Vector2 crossPosition;// I use this for the crosshair's position

    float lastShotTime = -30f;//This is used to make sure you don't rapidfire shit, start at -30 so you can shoot immediately

    float lastDamageTime = -30f; //This is so you don't get hit 100 times a second;


    Vector2 movementInput;

    Vector2 lookInput;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;

    Collider2D playerCollider;

    public AudioSource throwSound;
    public AudioSource ouchSound;

    bool isVeryDead = false;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        crossPosition = rb.position;//I use this to get the position from rb so I can modify the values
        crossPosition.x = crossPosition.x - 1;
        crosshair = Instantiate(crosshairPrefab, crossPosition, Quaternion.identity); //spawns crosshair
        playerCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()//called at a fixed number of times per second
    {
        spriteRenderer.color = Color.white;

        bool canMove = false;
        if(movementInput != Vector2.zero)
        {
             canMove = TryMove(movementInput);

            if(!canMove)//tries to slide along x axis wall
            {
                canMove = TryMove(new Vector2 (movementInput.x, 0));
            }

            if(!canMove)//tries to slide along y axis wall
            {
                canMove = TryMove(new Vector2 (0, movementInput.y));
            }

            animator.SetBool("isWalking", canMove);// if walking, walking anim
            
        }
        else//if not moving idle anim
        {
            animator.SetBool("isWalking", false);
        }
        
        //setting the direction of the animations
        if(movementInput.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(movementInput.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        
        checkZombieClose();
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
            moveCrosshair();//I put this here so the crosshair stays with the player
            return true;
        }
        else
        {
            //moveCrosshair();//I put this here so the crosshair stays with the player
            return false;
        }
    }

    void OnMove(InputValue movementValue)//Grabs from unity
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnLook(InputValue lookValue)
    {
        if(lookValue.Get<Vector2>() != Vector2.zero)
        {    lookInput = lookValue.Get<Vector2>();

            //This section makes sure the crosshair is always 1 distance away 
            angleRad = Mathf.Atan2(lookInput.y, lookInput.x);
            crossPosition.x = Mathf.Cos(angleRad);
            crossPosition.y = Mathf.Sin(angleRad);

            moveCrosshair();//This moves the crosshair
        }
    }

    void moveCrosshair()
    {
        //print(crossPosition);
        crosshair.transform.position = crossPosition + rb.position;
    }

    void OnFire()//Grabs from unity
    {
        ShotgunFire();
    }

    void ShotgunFire()
    {
        if(lastShotTime + 0.75f <= Time.time)//If it's been 0.25 seconds since the last shotgun shot
        {
            animator.SetTrigger("shootGun");
            lastShotTime = Time.time;
            Instantiate(bulletPrefab, rb.position + (crossPosition * 0.1f), Quaternion.Euler(0,0,Mathf.Rad2Deg*angleRad + 180));//This spawns the bullet in the right spot and angle

            //These create a spread of bullets
            Instantiate(bulletPrefab, rb.position + (crossPosition * 0.1f), Quaternion.Euler(0,0,Mathf.Rad2Deg*angleRad + 190));
            Instantiate(bulletPrefab, rb.position + (crossPosition * 0.1f), Quaternion.Euler(0,0,Mathf.Rad2Deg*angleRad + 200));
            Instantiate(bulletPrefab, rb.position + (crossPosition * 0.1f), Quaternion.Euler(0,0,Mathf.Rad2Deg*angleRad + 170));
            Instantiate(bulletPrefab, rb.position + (crossPosition * 0.1f), Quaternion.Euler(0,0,Mathf.Rad2Deg*angleRad + 160));

            throwSound.Play();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("zombie"))
        {
            print("Zombie collided with the player!");
        }
    }

    void checkZombieClose()
    {
        if(!isVeryDead){
            Collider2D checkNearby = Physics2D.OverlapCircle(playerCollider.bounds.center, 0.2f);

            if(checkNearby != null && checkNearby.CompareTag("zombie") && (lastDamageTime + 1 < Time.time))
            {
                spriteRenderer.color = Color.red;
                lastDamageTime = Time.time;//damage cooldown
                playerHealth--;
                ouchSound.Play();
                
                if(playerHealth < 1) // you died
                {
                    playerDied();
                }
            }
        }
    } 



    void playerDied()
    {
        animator.SetBool("isDead", true);
        movespeed = 0f;
        lastShotTime = float.MaxValue;//can't shoot, died
        isVeryDead = true;
    }
}
