using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallablePlatform : MonoBehaviour
{
    PlayerAnimator pa;
    PlayerController pc;
    BoxCollider2D bc;

    // Start is called before the first frame update
    void Start()
    {
        pa = FindObjectOfType<PlayerAnimator>();
        pc = FindObjectOfType<PlayerController>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pa.GetPosition().y - 0.84f >= transform.position.y)
        {
            bc.isTrigger = false;
        }
        else
        {
            bc.isTrigger = true;
        }

        // if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Z)) && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
        // {
        //     bc.isTrigger = true;
        // }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.collider.tag.Equals("GroundCheck"))
        {
            pc.setIsOnFallingPlatform(true);
        }

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag.Equals("GroundCheck"))
        {
            pc.setIsOnFallingPlatform(false);
        }
    }
}
