using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    int lifeCount = 0;
    BoxCollider2D bc;
    // [SerializeField] EnemyShoot es;
    bool startingRight = true;
    PlayerController pc;
    Rigidbody2D rb;
    float lastPos;
    float currentPos;
    int waitCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
        pc = FindObjectOfType<PlayerController>();
        rb = FindObjectOfType<Rigidbody2D>();

        if (pc.transform.position.x > transform.position.x) startingRight = true;
        else startingRight = false;

        currentPos = currentPos = Mathf.Round(transform.position.x * 100f) / 100f;
    }

    void FixedUpdate()
    {
        lastPos = currentPos;
        currentPos = Mathf.Round(transform.position.x * 100f) / 100f;

        lifeCount++;
        if (lifeCount == 7)
        {
            bc.isTrigger = false;
        }
        if (startingRight)
        {
            transform.position = new Vector3(transform.position.x + 0.1f, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - 0.1f, transform.position.y, transform.position.z);
        }

        // Debug.Log(lastPos + " " + currentPos);

        if (lastPos == currentPos && lifeCount >= 20)
        {
            waitCount += 1;
            if (waitCount == 3)
            {
                Destroy(gameObject);
            }
        }


        if (lifeCount == 200)
        {
            Destroy(gameObject);
        }
    }

    public void Detonate()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
