using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLauncher : MonoBehaviour
{
    LevelManager lm;
    // Start is called before the first frame update
    void Start()
    {
        lm = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            lm.NextLevel();
        }

    }
}
