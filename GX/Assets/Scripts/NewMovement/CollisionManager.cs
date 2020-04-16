using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public float groundRayLength = 0.2f;
    public float ceilingRayLength = 0.2f;
    public float rightRayLength = 0.2f;
    public float leftRayLength = 0.2f;
    public float groundRayOffset = 0.2f;
    public float ceilingRayOffset = 0.2f;
    public float rightRayOffsetTop = 0.2f;
    public float rightRayOffsetBottom = 0.2f;
    public float leftRayOffsetTop = 0.2f;
    public float leftRayOffsetBottom = 0.2f;
    
    public LayerMask collisionLayerMask;
    
    public bool onGround;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onCeiling;
    public bool onWall;

    //public property to get orientation
    public Vector2 transformUp;
    public Vector2 transformRight;

    public Vector2 correctVector2;
    private Vector2 position2D;
    
    //tracks if we are on any surface
    private bool standing = false;
    //tracks which layer we're stading on
    private int currentLayer = 0;

    private int numberOfRay = 50;
    List<RaycastHit2D> groundHitInfo = new List<RaycastHit2D>();

    void FixedUpdate()
    {
        Vector2 grav = gameObject.GetComponent<GravityManager>().GetGravityVector2();
        position2D = new Vector2(transform.position.x, transform.position.y);
        
        transformUp = new Vector2(-grav.x, -grav.y);
        transformRight = new Vector2(-grav.y, grav.x);
        
        DoubleRaycastDownToGround(groundRayLength, collisionLayerMask, groundHitInfo);
        DoubleRaycastDownToRight(rightRayLength, collisionLayerMask, out RaycastHit2D rightLeftHitInfo, out RaycastHit2D rightRightHitInfo);
        DoubleRaycastDownToLeft(leftRayLength, collisionLayerMask, out RaycastHit2D leftLeftHitInfo, out RaycastHit2D leftRightHitInfo);
        DoubleRaycastDownToCeiling(ceilingRayLength, collisionLayerMask, out RaycastHit2D ceilingLeftHitInfo, out RaycastHit2D ceilingRightHitInfo);

        //tracks the most recent layer we collided with
        int recentHitLayer = 0;
        standing = false;

        int groundHitCount = 0;
        foreach (var hit in groundHitInfo)
        {
            if (hit)
            {
                recentHitLayer = hit.transform.gameObject.layer;
                groundHitCount++;
            }
        }

        if (groundHitCount > numberOfRay * 0.5f)
        {
            onGround = true;
            SetLayerTo(recentHitLayer);
        }
        else onGround = false;
        
        if (ceilingLeftHitInfo && ceilingRightHitInfo)
        {
            onCeiling = true;
            SetLayerTo(ceilingLeftHitInfo.transform.gameObject.layer);
        }
        else onCeiling = false;
        
        if (rightLeftHitInfo || rightRightHitInfo)
        {
            onRightWall = true;
            RaycastHit2D correctHitInfo = rightLeftHitInfo ? rightLeftHitInfo : rightRightHitInfo;
            SetLayerTo(correctHitInfo.transform.gameObject.layer);
        }
        else onRightWall = false;
        
        if (leftLeftHitInfo || leftRightHitInfo)
        {
            onLeftWall = true;
            RaycastHit2D correctHitInfo = leftLeftHitInfo ? leftLeftHitInfo : leftRightHitInfo;
            SetLayerTo(correctHitInfo.transform.gameObject.layer);
        }
        else onLeftWall = false;

        onWall = onLeftWall || onRightWall;

        if (onGround && onLeftWall)
        {
            correctVector2 = (leftLeftHitInfo.point -
                              (position2D - transformUp * leftRayOffsetBottom + leftRayLength * -transformRight));
        }

        //if we're no longer standing, remove ourselves from the current layer
        if (standing == false && currentLayer != 0)
        {
            LayerDependantTracker.RemoveObjectFromLayer(gameObject, currentLayer);
            currentLayer = 0;
        }
        
        Debug.DrawRay(transform.position,transform.forward, Color.black, 5f);
        
    }
    
    private void DoubleRaycastDownToGround(float rayLength, LayerMask groundLayer, List<RaycastHit2D> groundHitInfo)
    {
        groundHitInfo.Clear();
        for (float i = -groundRayOffset; i < groundRayOffset; i += 2*groundRayOffset/numberOfRay)
        {
            groundHitInfo.Add(Physics2D.Raycast(position2D - transformRight * i, -transformUp, rayLength, groundLayer));       
            Debug.DrawRay(position2D - transformRight * i, - transformUp * rayLength, Color.cyan);
        }
    }
    
    private void DoubleRaycastDownToCeiling(float rayLength, LayerMask groundLayer, out RaycastHit2D leftHitInfo, out RaycastHit2D rightHitInfo)
    {
        leftHitInfo = Physics2D.Raycast(position2D - transformRight * ceilingRayOffset, transformUp, rayLength, groundLayer);
        rightHitInfo = Physics2D.Raycast(position2D + transformRight * ceilingRayOffset, transformUp, rayLength, groundLayer);
        
        Debug.DrawRay(position2D - transformRight * ceilingRayOffset, transformUp * rayLength, Color.red);
        Debug.DrawRay(position2D + transformRight * ceilingRayOffset, transformUp * rayLength, Color.red);

    }
    
    private void DoubleRaycastDownToRight(float rayLength, LayerMask groundLayer, out RaycastHit2D leftHitInfo, out RaycastHit2D rightHitInfo)
    {
        leftHitInfo = Physics2D.Raycast(position2D - transformUp * rightRayOffsetBottom, transformRight, rayLength, groundLayer);
        rightHitInfo = Physics2D.Raycast(position2D + transformUp * rightRayOffsetTop, transformRight, rayLength, groundLayer);
        
        Debug.DrawRay(position2D - transformUp * rightRayOffsetBottom, transformRight * rayLength, Color.blue);
        Debug.DrawRay(position2D + transformUp * rightRayOffsetTop, transformRight * rayLength, Color.blue);
    }
    
    private void DoubleRaycastDownToLeft(float rayLength, LayerMask groundLayer, out RaycastHit2D leftHitInfo, out RaycastHit2D rightHitInfo)
    {
        leftHitInfo = Physics2D.Raycast(position2D - transformUp * leftRayOffsetBottom, -transformRight, rayLength, groundLayer);
        rightHitInfo = Physics2D.Raycast(position2D + transformUp * leftRayOffsetTop, -transformRight, rayLength, groundLayer);
        
        Debug.DrawRay(position2D - transformUp * leftRayOffsetBottom, -transformRight * rayLength, Color.yellow);
        Debug.DrawRay(position2D + transformUp * leftRayOffsetTop, -transformRight * rayLength, Color.yellow);
    }

    public void SetLayerMask(LayerMask newMask)
    {
        collisionLayerMask = newMask;
    }

    private void SetLayerTo(int newLayer)
    {
        standing = true;
        if (newLayer != currentLayer)
        {
            //handle move to new layer
            LayerDependantTracker.RemoveObjectFromLayer(gameObject, currentLayer);
            currentLayer = newLayer;
            LayerDependantTracker.AddObjectToLayer(gameObject, newLayer);
        }
    }

}
