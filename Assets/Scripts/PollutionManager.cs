using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PollutionManager : MonoBehaviour
{
    PlayerAnimator pa;
    List<GameObject> disabledPollution = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        pa = FindObjectOfType<PlayerAnimator>();

        StartCoroutine("CheckDisabled");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PassInDisabled(GameObject go)
    {
        disabledPollution.Add(go);
    }

    IEnumerator CheckDisabled()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < disabledPollution.Count; i++)
            {
                if ((pa.GetPosition().x + 15 >= disabledPollution[i].transform.position.x && pa.GetPosition().x - 15 <= disabledPollution[i].transform.position.x))
                {
                    disabledPollution[i].SetActive(true);
                    disabledPollution.Remove(disabledPollution[i]);
                }
            }
        }
    }
}
