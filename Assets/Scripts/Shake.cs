using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Shake : MonoBehaviour
{
    bool start = false;
    public float duration = 0.1f;
    public AnimationCurve curve;
    CinemachineVirtualCamera cvc;

    // Start is called before the first frame update
    void Start()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine("Shaking");
        }
    }

    IEnumerator Shaking()
    {
        Vector2 startingPosition = new Vector3(FindObjectOfType<PlayerController>().transform.position.x, 1.05f, -10f);
        float elapsedTime = 0f;

        cvc.Follow = null;
        while (elapsedTime < duration)
        {
            startingPosition = new Vector3(FindObjectOfType<PlayerController>().transform.position.x, 1.05f, -10f);
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            Vector3 newPos1 = startingPosition + Random.insideUnitCircle * strength * 0.35f;
            transform.position = newPos1 + new Vector3(0f, 0f, -10f);
            yield return null;
        }

        transform.position = startingPosition;

        cvc.Follow = FindObjectOfType<PlayerController>().transform;
    }

    public void setStart(bool isStart)
    {
        start = isStart;
    }
}
