using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayer : MonoBehaviour
{

    SpriteRenderer srenderer;
    bool onLayer2 = false;

    // Start is called before the first frame update
    void Awake()
    {
        srenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!onLayer2 && srenderer.sortingLayerName == "Player L2")
        {
            srenderer.sortingLayerName = "Player L1";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Terrain Front 2"))
        {
            srenderer.sortingLayerName = "Player L2";
            onLayer2 = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onLayer2 = false;
    }
}
