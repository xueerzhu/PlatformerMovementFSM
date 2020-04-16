using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector2(Random.Range(0.2f, 9.8f), Random.Range(0.2f, 9.8f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Pellet trigger");
        if (collision.gameObject.tag == "Player")
        {
            gameObject.transform.position = new Vector2(Random.Range(0.2f, 9.8f), Random.Range(0.2f, 9.8f));
        }
    }
}
