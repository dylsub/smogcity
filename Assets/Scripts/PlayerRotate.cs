using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    // PlayerController pc;
    // void Start()
    // {
    //     pc = FindObjectOfType<PlayerController>();
    // }

    // void FixedUpdate()
    // {
    //     transform.position = pc.transform.position;
    // }

    public void Rotate(float x, float y, float z)
    {
        transform.Rotate(x, y, z);
    }
}
