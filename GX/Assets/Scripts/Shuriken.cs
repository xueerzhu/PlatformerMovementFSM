using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private Rigidbody2D shuBody;
    private float x;
    private float y;

    // Start is called before the first frame update
    void Start()
    {
        shuBody = GetComponent<Rigidbody2D>();
        x = shuBody.velocity[0];
        y = shuBody.velocity[1];
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(shuBody.velocity);
    }

    //collision
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //if shuriken hits a platform, it sticks to the that platform
        if (collision.gameObject.layer == 11 || collision.gameObject.layer == 12)
        {
            shuBody.constraints = RigidbodyConstraints2D.FreezeAll;

            //shuriken disappears in 1 second
            Destroy(gameObject, 1f);
        }

        //shuriken disappears when it hits a player
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }

        //shuriken bounces off sword and other shurikens
        if (collision.gameObject.tag == "shuriken" || collision.gameObject.tag == "slash")
        {
            shuBody.velocity = new Vector2(x * -1f, y * -1f);
        }
    }
}