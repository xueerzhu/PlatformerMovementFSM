using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    private Rigidbody2D slashbody;
    private Animator anim;

    public static bool hit;
    //public int id = 0; //player id of the slash
    public float animationSpeed = 5f; //higher number = faster animation speed

    [Space]
    [Header ("Sounds")]
    //public AudioSource hitPlayerSound; //the sound that plays when sword strikes a player
    public AudioSource parrySound; //the sound that plays when sword strikes another sword
    public AudioSource swingSound; //the sound that plays when sword swings but doesnt hit anything

    // Start is called before the first frame update
    void Start()
    {
        slashbody = GetComponent<Rigidbody2D>();
        hit = false;

        //controls animation speed
        anim = gameObject.GetComponent<Animator>();
        anim.speed = animationSpeed;

        AudioSource swingAudio = Instantiate(swingSound.GetComponent<AudioSource>()); //plays sword swing sound

        Destroy(gameObject, 1.0f/animationSpeed); //object destroys itself after 12 frames
    }  

    //handles collision
    public void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource myAudio;
        /*if (collision.gameObject.tag == "slash")
        {
            //play audio
            myAudio = Instantiate(parrySound.GetComponent<AudioSource>());

            Debug.Log("sword struck another sword");
            hit = true;
        }*/
        if (collision.gameObject.tag == "Player")
        {
            //play audio
            //myAudio = Instantiate(hitPlayerSound.GetComponent<AudioSource>());
            //myAudio = Instantiate(parrySound.GetComponent<AudioSource>());

            Debug.Log("sword struck a player");
            hit = true;
        }
        else
        {
            myAudio = Instantiate(parrySound.GetComponent<AudioSource>());
            Debug.Log("sword struck a shuriken");
            hit = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("hit: " + hit);
    }
}
