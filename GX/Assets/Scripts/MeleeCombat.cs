using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCombat : MonoBehaviour
{
    [Space]
    [Header("Player settings")]
    public bool canDie = true; //toggle if player can die or not for testing purposes
    public int lives = 3;

    [Space]
    [Header("Input Controls")]
    public string horrizontalCtrl = "Horizontal_P1";
    public string verticalCtrl = "Vertical_P1";
    public string meleeAttack = "melee_P1";

    [Space]
    [Header("Attacks")]
    public Rigidbody2D slash;

    [Space]
    [Header("Sounds")]
    public AudioSource stockTaken; //the sound that plays when sword strikes a player
    public AudioSource playerDies; //the sound that plays when sword strikes a player
    public AudioSource parry; //the sound that plays when sword strikes another sword

    private Rigidbody2D body;
    private bool startSlash = true; //controls whether player has start-up lag of not when attacking
    private float recovery = 0.5f; //recovery lasts 30 frames
    private float lastTimeStamp;
    private bool canAttack = true;
    private bool swordhit;
    //private Gravity grav;
    //private SpriteRenderer mySpriteRenderer;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        //grav = GetComponent<Gravity>();
        //mySpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        lastTimeStamp = Time.time;
    }

    private void Update()
    {
        //check if recovery frames are finished before allowing player to attack
        //Debug.Log(Time.time - lastTimeStamp);
        if (startSlash == false || Time.time - lastTimeStamp > recovery)
        {
            canAttack = true;
        }
        else
        {          
            canAttack = false;
        }

        //code for attacking with sword
        if (Input.GetButtonDown(meleeAttack) && canAttack == true)
        {
            lastTimeStamp = Time.time;
            StartCoroutine("Attack");
        }

        //Debug.Log(grav.m_GravityDirection);
        //Debug.Log(mySpriteRenderer.flipX);
    }

    //basic swing of sword, player swings sword 360 degrees around
    IEnumerator Attack()
    {
        Rigidbody2D MySlash;

        //direction of attack
        Vector2 direction = new Vector2(Input.GetAxis(horrizontalCtrl), Input.GetAxis(verticalCtrl));
        direction.Normalize();
        //angle of attack
        Vector3 turn;
        if (direction.y >= 0)
        {
            turn = new Vector3(0, 0, 90 * direction.x * -1);
        }
        else
        {
            if (direction.x == 0)
            {
                turn = new Vector3(0, 0, 180);
            }
            else
            {
                turn = new Vector3(0, 0, 180 * direction.x * -1);
            }
        }

        //start-up slash, this part executes if user hasn't unseathed their sword yet or if they finished recovery
        if (startSlash == true)
        {
            yield return new WaitForSecondsRealtime(0.2f); //start-up lag is 12 frames

            //after start-up frames, do a regular attack that deals damage            
            MySlash = Instantiate(slash, new Vector3(body.position.x + direction.x*0.15f, body.position.y + direction.y*0.15f, 1), Quaternion.Euler(turn)) as Rigidbody2D;

            //make player invincible while swordstrike animation plays
            if (canDie == true)
            {
                canDie = false;
                yield return new WaitForSecondsRealtime(0.2f);
                canDie = true;
            }

            //if MySlash hit the opponent, remove start-up for next attack. Otherwise, do start-up
            swordhit = Slash.hit;
            Debug.Log(swordhit);
            if (swordhit == true)
            {
                Debug.Log("start-up removed next turn");
                //remove start-up lag for player's next attack. will move/edit this line later
                startSlash = false;
            }
            else
            {
                startSlash = true;
            }
        }
        else
        {//counter + attack, this part executes if user successfully landed attack and therefore skips recovery
            Debug.Log("counter");
            yield return new WaitForSecondsRealtime(0.1f); //shorter lag, 6 frames
            MySlash = Instantiate(slash, new Vector3(body.position.x + direction.x * 0.15f, body.position.y + direction.y * 0.15f, 1), Quaternion.Euler(turn)) as Rigidbody2D;

            //make player invincible while swordstrike animation plays
            if (canDie == true)
            {
                canDie = false;
                yield return new WaitForSecondsRealtime(0.2f);
                canDie = true;
            }

            //if MySlash hit the opponent, remove start-up for next attack. Otherwise, do start-up
            swordhit = Slash.hit;
            Debug.Log(swordhit);
            if (swordhit == true)
            {
                Debug.Log("start-up removed next turn");
                //remove start-up lag for player's next attack. will move/edit this line later
                startSlash = false;
            }
            else
            {
                startSlash = true;
            }
        }
    }

    //collision
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if player got hit by sword, they die
        if (collision.gameObject.tag == "slash" && canDie == true)
        {
            AudioSource myAudio = Instantiate(stockTaken.GetComponent<AudioSource>());
            Debug.Log("player lost a stock");
            lives--;

            //if player runs out of lives, they die FOREVERRRRR
            if (lives <= 0)
            {
                AudioSource myAudio2 = Instantiate(playerDies.GetComponent<AudioSource>());
                Destroy(gameObject);
            }
        }

        //if player's sword hits another sword, parry sound plays
        if (collision.gameObject.tag == "slash" && canDie == false)
        {
            AudioSource myAudio = Instantiate(parry.GetComponent<AudioSource>());
        }
        

        //ignore player colliding into another player
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

}
