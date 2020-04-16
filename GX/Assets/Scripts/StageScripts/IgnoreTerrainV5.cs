using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreTerrainV5 : MonoBehaviour
{
    public string ignoreKey = "None";       //add in key character to be used in ignoring terrain
    public Collider2D terrainCollider;      //put here the player sprite's collision box that interacts with terrain
    public Collider2D trigger;              //put here the trigger collider that takes reign when the terrainCollider is disabled
    public CollisionManager manager;

    public bool touching_1 = false;
    public bool touching_2 = false;

    public bool enabled_1 = false;
    public bool enabled_2 = false;

    public bool buffered = false;

    private void Start()
    {
        DisableInteractions(LayerMask.NameToLayer("Terrain Front 1"));
        DisableInteractions(LayerMask.NameToLayer("Terrain Front 2"));
        touching_1 = false;
        touching_2 = false;
        enabled_1 = false;
        enabled_2 = false;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.0f / 30.0f);

        buffered = true;
    }

    private void Update()
    {
        if (!enabled_1 && !touching_1 && buffered)
        {
            EnableInteractions(LayerMask.NameToLayer("Terrain Front 1"));
            enabled_1 = true;
        }
        if (!enabled_2 && !touching_2 && enabled_1 && buffered)
        {
            EnableInteractions(LayerMask.NameToLayer("Terrain Front 2"));
            enabled_2 = true;
        }

        if (enabled_1 && enabled_2)
        {
            manager.SetLayerMask(LayerMask.NameToLayer("Terrain Front 1") | LayerMask.NameToLayer("Terrain Front 2"));
        }
        else if (enabled_1)
        {
            manager.SetLayerMask(LayerMask.NameToLayer("Terrain Front 1"));
        }
        else if (enabled_2)
        {
            manager.SetLayerMask(LayerMask.NameToLayer("Terrain Front 2"));
        }
        else
        {
            manager.SetLayerMask(LayerMask.NameToLayer("Nothing"));
        }
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
        //disables interactions with Layer 2 if on Layer 1
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 1")
        {
            //DisableInteractions(LayerMask.NameToLayer("Terrain Front 2"));
            enabled_2 = false;
        }

        if (ignoreKey == "None")
        {
            return;
        }
        if (Input.GetButtonDown(ignoreKey))
        {
            //Debug.Log("Disabled collisions");
            //Physics2D.IgnoreCollision(terrainCollider, collision.collider, true);
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 1")
            {
                enabled_1 = false;
                //DisableInteractions(LayerMask.NameToLayer("Terrain Front 2"));
            }
            enabled_2 = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 1")
        {
            touching_1 = true;
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 2")
        {
            touching_2 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 1")
        {
            touching_1 = false;
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Terrain Front 2")
        {
            touching_2 = false;
        }
    }
}
