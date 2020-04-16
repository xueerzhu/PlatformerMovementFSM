using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talisman : MonoBehaviour
{
    private Rigidbody2D talBody;
    private bool stuck = false; //if talisman stuck itself into a wall

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("talisman working");
        talBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //collision
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Colliding");

        //if talisman hits a platform, it sticks to the that platform
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 12)
        {
            talBody.constraints = RigidbodyConstraints2D.FreezeAll;
            stuck = true;

            //talisman disappears in 20 seconds
            Destroy(gameObject, 20f);
        }
        //if player touches a talisman stuck to a wall (ex: player teleported to talisman's position), the talisman disappears 
        if (collision.gameObject.tag == "Player" && stuck == true)
        {
            Debug.Log("player touched talisman");
            Destroy(gameObject);
        }
    }
}
