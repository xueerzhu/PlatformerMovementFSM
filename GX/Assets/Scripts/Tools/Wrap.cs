using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//credits to tutorial provided here:
//https://gamedevelopment.tutsplus.com/articles/create-an-asteroids-like-screen-wrapping-effect-with-unity--gamedev-15055

public class Wrap : MonoBehaviour
{
    private Renderer[] renderers;
    private bool isWrappingX = false;
    private bool isWrappingY = false;

    private float screenWidth;
    private float screenHeight;
    private Transform[] ghosts = new Transform[8];

    public string[] include = { };

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        //var cam = Camera.main;

        //var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        //var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        //screenWidth = screenTopRight.x - screenBottomLeft.x;
        //screenHeight = screenTopRight.y - screenBottomLeft.y;

        screenWidth = 15.0f;
        screenHeight = 15.0f;

        CreateGhosts();
        PositionGhosts();
    }

    bool CheckRenderers()
    {
        foreach(Renderer renderer in renderers)
        {
            if (renderer.isVisible)
            {
                return true;
            }
        }
        return false;
    }

    void ScreenWrap()
    {
        //bool isVisible = CheckRenderers();

        //if (isVisible)
        //{
        //    isWrappingX = false;
        //    isWrappingY = false;
        //    return;
        //}

        //if (isWrappingX && isWrappingY)
        //{
        //    return;
        //}
        //Vector3 newPosition = transform.position;

        if(gameObject.transform.position.x > 10.0f)
        {
            transform.position = new Vector2(transform.position.x - screenWidth, transform.position.y);
        }
        if (gameObject.transform.position.x < 0.0f)
        {
            transform.position = new Vector2(transform.position.x + screenWidth, transform.position.y);
        }
        if (gameObject.transform.position.y > 10.0f)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - screenHeight);
        }
        if (gameObject.transform.position.y < 0.0f)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + screenHeight);
        }
    }

    void CreateGhosts()
    {
        for (int i = 0; i < 8; i++)
        {
            ghosts[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;

            //Destroys all unnecessary components
            foreach (var component in ghosts[i].GetComponents<Component>())
            {
                //makes sure wanted components are not removed
                if (component is SpriteRenderer ||
                    component is Transform)
                {
                    continue;
                }
                Destroy(component);
            }
            foreach (var component in include)
            {
                Debug.Log("Attaching component: " + include);
                ghosts[i].gameObject.AddComponent(System.Type.GetType(component));
            }
        }
    }

    void PositionGhosts()
    {
        // All ghost positions will be relative to the original's (this) transform
        var ghostPosition = transform.position;

        // Right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[0].position = ghostPosition;

        // Bottom-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[1].position = ghostPosition;

        // Bottom
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[2].position = ghostPosition;

        // Bottom-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y - screenHeight;
        ghosts[3].position = ghostPosition;

        // Left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y;
        ghosts[4].position = ghostPosition;

        // Top-left
        ghostPosition.x = transform.position.x - screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[5].position = ghostPosition;

        // Top
        ghostPosition.x = transform.position.x;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[6].position = ghostPosition;

        // Top-right
        ghostPosition.x = transform.position.x + screenWidth;
        ghostPosition.y = transform.position.y + screenHeight;
        ghosts[7].position = ghostPosition;

        // All ghosts should have the same rotation as the main
        for (int i = 0; i < 8; i++)
        {
            ghosts[i].rotation = transform.rotation;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ScreenWrap();
        PositionGhosts();
    }
}
