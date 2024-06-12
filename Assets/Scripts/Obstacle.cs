using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (gameObject.name.Substring(0, 3).Equals("rad"))
            {
                Destroy(gameObject);
            }
            if (pc.getCanRespawn())
            {
                pc.Respawn();
            }
        }
        if (other.gameObject.name.Substring(0, 3).Equals("rad") && gameObject.name.Substring(0, 3).Equals("rad"))
        {
            Destroy(gameObject);
        }
        // if (other.gameObject.tag.Equals("Ground"))
        // {
        //     Debug.Log("Hit");
        //     Collider2D collider = other.collider;

        //     Vector3 contactPoint = other.contacts[0].point;
        //     Vector3 center = collider.bounds.center;

        //     bool top = contactPoint.y > center.y;

        //     Debug.Log(contactPoint.y);

        //     if (top)
        //     {
        //         Destroy(gameObject);
        //     }
        //     else
        //     {

        //     }
        // }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            if (gameObject.name.Substring(0, 3).Equals("rad"))
            {
                Destroy(gameObject);
            }
            if (pc.getCanRespawn())
            {
                pc.Respawn();
            }
        }
    }
}
