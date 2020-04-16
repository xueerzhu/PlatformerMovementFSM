using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwitchTerrain : MonoBehaviour
{

    public KeyCode switchKey;
    public Collider2D terrainCollider;
    public Collider2D trigger;
    public TileroomBuilder stage;

    private bool l = false;
    private SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            l = !l;
        }

        if (l)
        {
            sprite.sortingLayerName = "Player L2";
            Physics2D.IgnoreCollision(terrainCollider, stage.UpperLayer().GetComponent<CompositeCollider2D>(), true);
            Physics2D.IgnoreCollision(terrainCollider, stage.LowerLayer().GetComponent<CompositeCollider2D>(), false);
            Color temp1 = stage.UpperLayer().GetComponent<Tilemap>().color;
            Color temp2 = stage.LowerLayer().GetComponent<Tilemap>().color;

            temp1.a = 0.5f;
            temp2.a = 1;
            stage.UpperLayer().GetComponent<Tilemap>().color = temp1;
            stage.LowerLayer().GetComponent<Tilemap>().color = temp2;
        }
        else
        {
            sprite.sortingLayerName = "Player L1";
            Physics2D.IgnoreCollision(terrainCollider, stage.UpperLayer().GetComponent<CompositeCollider2D>(), false);
            Physics2D.IgnoreCollision(terrainCollider, stage.LowerLayer().GetComponent<CompositeCollider2D>(), true);
            Color temp1 = stage.UpperLayer().GetComponent<Tilemap>().color;
            Color temp2 = stage.LowerLayer().GetComponent<Tilemap>().color;

            temp1.a = 1;
            temp2.a = 1;
            stage.UpperLayer().GetComponent<Tilemap>().color = temp1;
            stage.LowerLayer().GetComponent<Tilemap>().color = temp2;
        }
    }


}
