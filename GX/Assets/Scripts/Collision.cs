using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask collisionLayerMask;

    [Space]
    public bool onGround;
    public bool onWall;  
    public bool onEdge;
    public bool wrapCorner = false;   // disabled: bug cause: turns true when not using composite collision on room group
    public bool onRightWall;
    public bool onLeftWall;
    public bool onCeiling;
    public int wallSide;
    public bool onLeftEdge;
    public bool onRightEdge;

    [Space]
    [Header("Collision")]

    public float collisionRadius = 0.05f;
    public float collisionBoxX = 0.42f;
    public float collisionBoxY = 0.04f;
    public Vector2 bottomOffset = new Vector2(0f, -0.16f);
    public Vector2 rightOffset = new Vector2(0.25f, 0f);
    public Vector2 leftOffset = new Vector2(-0.25f, 0f);
    public Vector2 topOffset = new Vector2(0f, 0.32f);
    public float rayLength = 0.7f;
    public float rayOffset = 0.2f;

    private readonly Color debugCollisionColor = Color.red;

    private Rigidbody2D rb;
    private Gravity grav;
    private Movement movement;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        grav = GetComponent<Gravity>();
        movement = GetComponent<Movement>();
    }

    private void Update() {
        //LayerMask collisionLayerMask = terrainLayer1 | terrainLayer2;
        Vector2 point = (Vector2) transform.position;
        onGround = Physics2D.OverlapCircle(point + bottomOffset, collisionRadius, collisionLayerMask);
        //onGround = Physics2D.OverlapBox(point + bottomOffset, new Vector2(collisionBoxX,collisionBoxY), collisionLayerMask);
        onRightWall = Physics2D.OverlapCircle(point + rightOffset, collisionRadius, collisionLayerMask);
        onLeftWall = Physics2D.OverlapCircle(point + leftOffset, collisionRadius, collisionLayerMask);
        onCeiling = Physics2D.OverlapCircle(point + topOffset, collisionRadius, collisionLayerMask);
        onWall = onLeftWall || onRightWall || onCeiling;    

        if (onWall)
        {
            if (onCeiling)
            {
                wallSide = 2;
                grav.OnDashGravityChange();
            }
            else
            {
                wallSide = onRightWall ? 1 : -1;
                
                if(!onGround)
                    grav.OnDashGravityChange();

            }
        }
        else
        {
            wallSide = 0;
        }

        // wrap corner behavior
        DoubleRaycastDown(rayLength, collisionLayerMask, out RaycastHit2D leftHitInfo, out RaycastHit2D rightHitInfo);

        onEdge = onLeftEdge || onRightEdge;
        onRightEdge = (leftHitInfo == true && rightHitInfo == false);
        onLeftEdge = (leftHitInfo == false && rightHitInfo == true);
        

        if (onEdge)
        {
            if (!onLeftEdge && !onRightEdge && !movement.canFly)
            {
                //wrapCorner = true;
            }
        }

        if (onGround)
        {
            grav.gravityScale = grav.defaultGravityScale;
        }
    }



    private void OnDrawGizmos() {
        Gizmos.color = debugCollisionColor;
        var center = (Vector2) transform.position;
        Gizmos.DrawWireSphere(center + bottomOffset, collisionRadius);
        //Gizmos.DrawWireCube(center + bottomOffset, new Vector2(collisionBoxX,collisionBoxY));
        Gizmos.DrawWireSphere(center + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere(center + topOffset, collisionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center + leftOffset, collisionRadius);   // left is blue
    }

    private void DoubleRaycastDown(float rayLength, LayerMask groundLayer, out RaycastHit2D leftHitInfo, out RaycastHit2D rightHitInfo)
    {
        Vector2 transformUp = transform.up;
        Vector2 transformRight = transform.right;
        var position2D = new Vector2(transform.position.x, transform.position.y);

        Debug.DrawRay(position2D - transformRight * rayOffset, - transformUp * rayLength, Color.cyan);
        Debug.DrawRay(position2D + transformRight * rayOffset, - transformUp * rayLength, Color.magenta);

        leftHitInfo = Physics2D.Raycast(position2D - transformRight * rayOffset, -transformUp, rayLength, groundLayer);
        rightHitInfo = Physics2D.Raycast(position2D + transformRight * rayOffset, -transformUp, rayLength, groundLayer);

    }
}
