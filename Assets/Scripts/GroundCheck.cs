using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] PlayerController pc;

    bool isGrounded = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (gameObject.tag.Equals("GroundCheck") && (collision.gameObject.tag.Equals("FallablePlatform") || collision.gameObject.tag.Equals("Ground")))
        {
            // Debug.Log("Grounded");
            pc.SetIsGrounded(true);
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.tag.Equals("Ground")) return;
        if (gameObject.tag.Equals("WallCheck") && !isGrounded)
        {
            pc.setIsTouchingWall(false);
        }
        if (gameObject.tag.Equals("SlidingCheck") && !isGrounded)
        {
            pc.setIsSlideTouching(false);
        }
        if (gameObject.tag.Equals("WallBackCheck") && !isGrounded)
        {
            pc.setIsBackTouchingWall(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.tag.Equals("Ground")) return;
        if (gameObject.tag.Equals("WallCheck") && !isGrounded)
        {
            pc.setIsTouchingWall(true);
        }
        if (gameObject.tag.Equals("SlidingCheck") && !isGrounded)
        {
            pc.setIsSlideTouching(true);
        }
        if (gameObject.tag.Equals("WallBackCheck") && !isGrounded)
        {
            pc.setIsBackTouchingWall(true);
        }
    }
}
