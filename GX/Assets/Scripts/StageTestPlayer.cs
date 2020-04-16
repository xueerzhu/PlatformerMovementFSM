using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTestPlayer : MonoBehaviour
{

    BoxCollider2D box;
    Rigidbody2D body;
    Sprite image;
    bool onTerrain;

    public int stageLayer = 8;

    // Start is called before the first frame update
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        image = GetComponent<Sprite>();
        onTerrain = false;
    }

    // Update is called once per frame
    void Update()
    {

        float translateX = 0.0f;
        float translateY = 0.0f;

        if (Input.GetKey("a"))
        {
            translateX -= 0.05f;
        }
        if (Input.GetKey("d"))
        {
            translateX += 0.05f;
        }
        if (Input.GetKey("s"))
        {
            translateY -= 0.05f;
        }
        if (Input.GetKey("w"))
        {
            translateY += 0.05f;
        }

        transform.Translate(new Vector2(translateX, translateY));
    }
}