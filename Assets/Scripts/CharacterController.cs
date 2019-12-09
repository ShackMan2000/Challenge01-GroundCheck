using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//the idea of shooting down a bunch of rays is taken from the 2D controller series by Sebastian Lague. This is a highly simplified version
//Sebastian actually uses the rays to move and stop the character, I just took the idea for a simply, yet reliable ground check
public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float jumpForce;

    [SerializeField]
    private int numberOfRays;

    [SerializeField]
    private float rayLength;

    [SerializeField]
    private LayerMask platformLayer;

    private Collider2D collider;
    private Rigidbody2D body;

    private void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsStandingOnGround())
            {
                //resetting vertical speed to 0 since the player might hit jump when the character is falling down right before hitting the ground
                //in that case the gravity force might overpower the jump force, and the result is that it "feels" like the jump button didn't work
                body.velocity = new Vector2(body.velocity.x, 0f);
                body.AddForce(Vector2.up * jumpForce);
            }
        }
    }




    //This method shoots down rays, starting from the left side of the boxCollider. The number is adjustable:
    //if the game has only platforms that are wider than the player, 2 is enough. 
    private bool IsStandingOnGround()
    {
        Bounds boxBounds = collider.bounds;
        Vector2 rayOrigin = new Vector2(boxBounds.center.x - boxBounds.extents.x, boxBounds.center.y - boxBounds.extents.y);

        bool atLeastOneRayHittingGround = false;

        for (int i = 0; i < numberOfRays; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayLength, platformLayer);
            Debug.DrawRay(rayOrigin, Vector2.down * rayLength, Color.red);

            if (hit)
            {
                atLeastOneRayHittingGround = true;
            }
            float nextRaySpacing = (boxBounds.extents.x * 2.0f) / (numberOfRays - 1f);
            rayOrigin += Vector2.right * nextRaySpacing;
        }

        return atLeastOneRayHittingGround;
    }
}
