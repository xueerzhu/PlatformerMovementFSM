using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreTerrain : MonoBehaviour
{

    public string ignoreKey = "e";          //add in key character to be used in ignoring terrain
    public Collider2D terrainCollider;      //put here the player sprite's collision box that interacts with terrain
    public Collider2D trigger;              //put here the trigger collider that takes reign when the terrainCollider is disabled

    private bool touchingLayer1 = false;
    private bool touchingLayer2 = false;

    private void Update()
    {
    }

    //disables interactions with all objects in passed layer.
    void DisableInteractions(LayerMask layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        //disables collisions with the objects in same layer
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            {
                Physics2D.IgnoreCollision(terrainCollider, goArray[i].GetComponent<CompositeCollider2D>());
            }
        }
    }

    //enables interactions with all objects in passed layer.
    void EnableInteractions(LayerMask layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        //disables collisions with the objects in same layer
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].layer == layer)
            {
                Physics2D.IgnoreCollision(terrainCollider, goArray[i].GetComponent<CompositeCollider2D>(), false);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Input.GetButtonDown(ignoreKey))
        {
            Debug.Log("Disabled collisions");
            Physics2D.IgnoreCollision(terrainCollider, collision.collider, true);
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 1")
            {
                DisableInteractions(LayerMask.NameToLayer("Terrain Front 2"));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(terrainCollider, collision, false);

        if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 1")
        {
            touchingLayer1 = false;
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 2")
        {
            touchingLayer2 = false;
        }

        if (!touchingLayer2)
        {
            EnableInteractions(LayerMask.NameToLayer("Terrain Front 2"));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 1")
        {
            touchingLayer1 = true;
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 2")
        {
            touchingLayer2 = true;
        }
    }
}
