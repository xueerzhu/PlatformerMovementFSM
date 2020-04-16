using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCombat : MonoBehaviour
{
    public float projectileSpeed = 10f;

    [Space]
    [Header("Input")]
    public string horrizontalCtrl = "Horizontal_P1";
    public string verticalCtrl = "Vertical_P1";
    public string shootTalisman = "Throw_Talisman_P1";
    public string shootShuriken = "Throw_Shuriken_P1";

    [Space]
    [Header("Attacks")]
    public Rigidbody2D talisman;
    public Rigidbody2D shuriken;

    private Rigidbody2D body;
    private float recovery = 0.2f; //recovery lasts 12 frames
    private float lastTimeStamp;
    private bool canThrow = true;
    private SpriteRenderer mySpriteRenderer;
    private Gravity grav;
    private Vector2 forwardDir; //specifies which direction the player is facing

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        grav = GetComponent<Gravity>();
        mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        lastTimeStamp = Time.time;
        forwardDir = new Vector2(1, 0);
    }

    private void Update()
    {
        //apply recovery frames
        if (Time.time - lastTimeStamp > recovery)
        {
            canThrow = true;
        }
        else
        {
            canThrow = false;
        }

        //code for throwing talisman. 
        if (Input.GetButtonDown(shootTalisman) && canThrow == true)
        {
            lastTimeStamp = Time.time;
            StartCoroutine("ThrowTalisman");
        }

        //code for throwing a shuriken. 
        if (Input.GetButtonDown(shootShuriken) && canThrow == true)
        {
            lastTimeStamp = Time.time;
            StartCoroutine("ThrowWeapon");
        }
    }

    //throws talisman
    IEnumerator ThrowTalisman()
    {
        //start-up lag is 16 frames
        yield return new WaitForSecondsRealtime(0.267f);

        //controls the direction the talisman flies in based on arrow keys
        Vector2 direction = new Vector2(Input.GetAxis(horrizontalCtrl), Input.GetAxis(verticalCtrl));
        direction.Normalize();

        //if user specifies which direction to throw talisman, throw in that direction
        if (direction.x != 0 || direction.y != 0)
        {
            //rotate talisman in the direction it is heading towards
            Vector3 turn = new Vector3(0, 0, direction.x * 90 + 90);
            //spawn talisman at player location
            Rigidbody2D MyTalisman = Instantiate(talisman, new Vector3(body.position.x, body.position.y, 1), Quaternion.Euler(turn)) as Rigidbody2D;

            //talisman flies in direction that user inputs
            MyTalisman.velocity = new Vector2(direction.x * projectileSpeed * 2f, direction.y * projectileSpeed * 2f);
        }
        else
        {
            //if user does not specify which direction to throw talisman, throw it forward
            //spawn talisman at player location
            Rigidbody2D MyTalisman = Instantiate(talisman, new Vector3(body.position.x, body.position.y, 1), Quaternion.identity) as Rigidbody2D;

            //talisman flies forward
            FindForward();
            MyTalisman.velocity = new Vector2(forwardDir.x * projectileSpeed * 2f, forwardDir.y * projectileSpeed * 2f);
        }
    }

    //throws shuriken
    IEnumerator ThrowWeapon()
    {
        //start-up lag is 16 frames
        yield return new WaitForSecondsRealtime(0.267f);

        //controls the direction the shuriken flies in based on arrow keys
        Vector2 direction = new Vector2(Input.GetAxis(horrizontalCtrl), Input.GetAxis(verticalCtrl));
        direction.Normalize();

        //if user specifies which direction to throw shuriken, throw in that direction
        if (direction.x != 0 || direction.y != 0)
        {
            //rotate shuriken in the direction it is heading towards
            Vector3 turn = new Vector3(0, 0, direction.x * 90 + 90);
            //spawn shuriken at player location
            Rigidbody2D MyShuriken = Instantiate(shuriken, new Vector3(body.position.x, body.position.y, 1), Quaternion.Euler(turn)) as Rigidbody2D;

            //talisman flies in direction that user inputs
            MyShuriken.velocity = new Vector2(direction.x * projectileSpeed * 2f, direction.y * projectileSpeed * 2f);
        }
        else
        {
            //if user does not specify which direction to throw shuriken, throw it forward
            //spawn shuriken at player location
            Rigidbody2D MyShuriken = Instantiate(shuriken, new Vector3(body.position.x, body.position.y, 1), Quaternion.identity) as Rigidbody2D;

            //shuriken flies forward
            FindForward();
            MyShuriken.velocity = new Vector2(forwardDir.x * projectileSpeed * 2f, forwardDir.y * projectileSpeed * 2f);
        }
    }

    //collision
    IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        //add code for when player gets hit by talisman and shuriken
        if (collision.gameObject.tag == "shuriken")
        {
            //player gets stunned when hit by shuriken
            Debug.Log("Player has been stunned");
            body.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSecondsRealtime(1f); //stun for 60 frames, will change later
            body.constraints = RigidbodyConstraints2D.None;
        }
    }

    //calculates which direction is forward for the player character
    private void FindForward()
    {
        if (grav.m_GravityDirection == Gravity.GravityDirection.Down)
        {
            if (mySpriteRenderer.flipX == false)
            {
                forwardDir = new Vector2(1, 0); //facing right
            }
            else
            {
                forwardDir = new Vector2(-1, 0); //facing left
            }
        }
        else if (grav.m_GravityDirection == Gravity.GravityDirection.Right)
        {
            if (mySpriteRenderer.flipX == false)
            {
                forwardDir = new Vector2(0, 1); //facing up
            }
            else
            {
                forwardDir = new Vector2(0, -1); //facing down
            }
        }
        else if (grav.m_GravityDirection == Gravity.GravityDirection.Up)
        {
            if (mySpriteRenderer.flipX == false)
            {
                forwardDir = new Vector2(-1, 0); //facing left
            }
            else
            {
                forwardDir = new Vector2(1, 0); //facing right
            }
        }
        else
        {
            if (mySpriteRenderer.flipX == false)
            {
                forwardDir = new Vector2(0, -1); //facing down
            }
            else
            {
                forwardDir = new Vector2(0, 1); //facing up
            }
        }
    }

}
