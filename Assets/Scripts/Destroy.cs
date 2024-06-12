using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    PlayerController pc;
    PlayerAnimator pa;
    bool isInside = false;
    Animator anim;
    bool isPlaying = true;
    PollutionManager pm;
    GameState gs;
    SoundManager sm;

    void Awake()
    {
        // Debug.Log(Random.Range(0, 3));
        anim = GetComponent<Animator>();
        anim.Play("pollution", 0, Random.Range(0, 1f));
        pc = FindObjectOfType<PlayerController>();
        pa = FindObjectOfType<PlayerAnimator>();
        pm = FindObjectOfType<PollutionManager>();
        gs = FindObjectOfType<GameState>();
        sm = FindObjectOfType<SoundManager>();
    }

    private void OnEnable()
    {
        isPlaying = true;
    }
    void Update()
    {
        if ((pa.GetPosition().x + 15 < transform.position.x || pa.GetPosition().x - 15 > transform.position.x) && isPlaying)
        {
            pm.PassInDisabled(gameObject);
            isPlaying = false;
            gameObject.SetActive(false);
        }
    }

    public void setIsPlaying(bool playing)
    {
        isPlaying = playing;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("WallCheck") || other.tag.Equals("Ground"))
        {
            isInside = true;
            if (pc.GetIsVacuum())
            {
                sm.playSuck();
                gs.addToPollutionCount(1);
                Destroy(gameObject);
            }
            StartCoroutine("CheckForVacuumOn");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("WallCheck"))
        {
            isInside = false;
            StopAllCoroutines();
        }
    }

    public IEnumerator CheckForVacuumOn()
    {
        yield return new WaitForSeconds(0.05f);
        if (isInside && pc.GetIsVacuum())
        {
            sm.playSuck();
            gs.addToPollutionCount(1);
            Destroy(gameObject);
        }
        StartCoroutine("CheckForVacuumOn");
    }


}
