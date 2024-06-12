using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    int shootCount = 0;
    [SerializeField] GameObject project;
    SpriteRenderer sr;
    [SerializeField] Sprite open;
    [SerializeField] Sprite closed;
    PlayerController pc;
    [SerializeField] GameObject pollution;
    GameState gs;
    EnemyProjectile[] ep;
    bool isVacuum = false;
    bool inside = false;

    bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closed;
        pc = FindObjectOfType<PlayerController>();
        gs = FindObjectOfType<GameState>();
        ep = FindObjectsOfType<EnemyProjectile>();

    }

    void FixedUpdate()
    {
        shootCount++;
        if (shootCount == 100)
        {
            sr.sprite = open;
            Instantiate<GameObject>(project, transform.position + new Vector3(0f, 0f, -1f), Quaternion.identity);
        }
        else if (shootCount == 130)
        {
            sr.sprite = closed;
            shootCount = 0;
        }

        isVacuum = pc.GetIsVacuum();
        if (isVacuum && inside)
        {
            Debug.Log("Hit");
            Instantiate<GameObject>(pollution, transform.position + new Vector3(-1.5f, -1.5f, -1f), Quaternion.identity);
            ep = FindObjectsOfType<EnemyProjectile>();
            for (int i = 0; i < ep.Length; i++)
            {
                ep[i].Detonate();
            }
            Destroy(gameObject);
        }

        if (pc.transform.position.x > transform.position.x && !facingRight)
        {
            Rotate(0, 180f, 0);
        }
        else if (pc.transform.position.x <= transform.position.x && facingRight)
        {
            Rotate(0, 180f, 0);
        }
    }

    public bool getFacingRight()
    {
        return facingRight;
    }

    public void Rotate(float x, float y, float z)
    {
        facingRight = !facingRight;
        transform.Rotate(x, y, z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name.Substring(0, 3).Equals("vac"))
        {
            inside = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name.Substring(0, 3).Equals("vac"))
        {
            inside = false;
        }
    }
}
