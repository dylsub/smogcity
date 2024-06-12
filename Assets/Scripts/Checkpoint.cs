using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    CheckpointManager cpm;
    SpriteRenderer sr;
    [SerializeField] Sprite activated;
    [SerializeField] Sprite unactivated;
    SoundManager sm;
    // Start is called before the first frame update
    void Start()
    {
        cpm = FindObjectOfType<CheckpointManager>();
        sr = GetComponent<SpriteRenderer>();
        sm = FindObjectOfType<SoundManager>();
    }

    public void setUnactivated()
    {
        sr.sprite = unactivated;
    }

    public void setActivated()
    {
        sr.sprite = activated;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Triggered!");
            if (sr.sprite == unactivated)
            {
                sm.playCheckpoint();
            }
            cpm.TriggerNewCheckpoint(GetComponent<Checkpoint>());
        }
    }
}
