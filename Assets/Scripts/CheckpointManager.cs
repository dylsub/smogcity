using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    Checkpoint[] cp;

    Vector3 currRespawnPoint = new Vector3(-34.24f, -1.2f, -1f);
    // Start is called before the first frame update
    void Start()
    {
        cp = FindObjectsOfType<Checkpoint>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TriggerNewCheckpoint(Checkpoint currCP)
    {
        // Debug.Log(currCP.transform.position);
        currRespawnPoint = new Vector3(currCP.transform.position.x, currCP.transform.position.y, -1f);
        for (int i = 0; i < cp.Length; i++)
        {
            if (currCP.transform.position == cp[i].transform.position)
            {
                cp[i].setActivated();
                continue;
            }
            cp[i].setUnactivated();
        }
    }

    public Vector3 getRespawnPosition()
    {
        return currRespawnPoint;
    }
}
