using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : MonoBehaviour
{

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.parent != null)
        {
            return;
        }
        collision.transform.parent = gameObject.transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.parent = null;
    }
}
