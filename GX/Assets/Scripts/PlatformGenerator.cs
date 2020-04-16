using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    private Transform[] platforms = new Transform[8];
    // Start is called before the first frame update
    void Start()
    {
        var cam = Camera.main;

        var screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        var screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        var screenWidth = screenTopRight.x - screenBottomLeft.x;
        var screenHeight = screenTopRight.y - screenBottomLeft.y;

        for (int i = 0; i < 8; i++)
        {
            platforms[i] = Instantiate(transform, Vector3.zero, Quaternion.identity) as Transform;
            Destroy(platforms[i].GetComponent<PlatformGenerator>());
            platforms[i].transform.position = new Vector2(Random.Range(0, screenWidth), Random.Range(0, screenHeight));
            platforms[i].transform.Rotate(0, 0, Random.Range(0, 3) * 90);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
