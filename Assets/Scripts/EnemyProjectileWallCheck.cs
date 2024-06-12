using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileWallCheck : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Running");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Hit1");
        if (other.gameObject.tag.Equals("Ground"))
        {
            Debug.Log("Hit");
            Destroy(projectile);
        }
    }
}
