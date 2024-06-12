using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryCollect : MonoBehaviour
{
    GameState gs;
    SoundManager sm;

    // Start is called before the first frame update
    void Start()
    {
        gs = FindObjectOfType<GameState>();
        sm = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            gs.addToBatteryCount(1);
            sm.playPickup();
            Destroy(gameObject);
        }
    }
}
