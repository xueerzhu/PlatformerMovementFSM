using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMover : MonoBehaviour
{
    public int layerNumber;

    private GameObject movingLayer;
    private float expireTime;
    private bool completeX;
    private bool completeY;

    private int xMod;
    private int yMod;
    private float accel = 0.0002f;
    private float maxSpeed = 0.04f;
    private float xSpeed = 0.0f;
    private float ySpeed = 0.0f;


    void Awake()
    {
        movingLayer = this.gameObject;
        expireTime = Time.time + 10.0f;
        Debug.Log("Start time: " + Time.time);
        Debug.Log("End time: " + expireTime);
        xMod = Random.Range(-1, 2);
        yMod = Random.Range(-1, 2);
        if (xMod == 0 && yMod == 0)
        {
            xMod = 1;
        }
        completeX = false;
        completeY = false;
        //set layer equal to our child's (child of stage layer = stage grid)
        layerNumber = transform.GetChild(0).gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        if (expireTime < Time.time)
        {
            Debug.Log("Destroyed");
            deccelerate();
            if (completeX && completeY)
            {
                Destroy(this);
            }
        }
        else
        {
            Debug.Log("Moving");
            accelerate();
        }

        if (movingLayer != null)
        {
            print("translating");
            movingLayer.transform.Translate(new Vector2(xSpeed, ySpeed));
            //move players standing on this layer accordingly
            foreach (GameObject dependant in LayerDependantTracker.objectsPerLayer[layerNumber])
            {
                dependant.transform.Translate(new Vector2(xSpeed, ySpeed), Space.World);
            }
        }
    }

    private void accelerate()
    {
        if ((Mathf.Abs(xSpeed) < maxSpeed))
        {
            xSpeed += accel * xMod;
        }
        if ((Mathf.Abs(ySpeed) < maxSpeed))
        {
            ySpeed += accel * yMod;
        }
    }

    private void deccelerate()
    {
        if (Mathf.Sign(xSpeed) != Mathf.Sign(xMod))
        {
            xSpeed = 0.0f;
            completeX = true;
        }
        else if (!(Mathf.Abs(xSpeed) < 0))
        {
            xSpeed -= accel * xMod;
        }
        if (Mathf.Sign(ySpeed) != Mathf.Sign(yMod))
        {
            ySpeed = 0.0f;
            completeY = true;
        }
        else if (!(Mathf.Abs(ySpeed) < 0))
        {
            ySpeed -= accel * yMod;
        }
    }
}
