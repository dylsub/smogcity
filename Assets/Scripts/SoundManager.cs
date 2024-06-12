using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource aus;
    [SerializeField] AudioClip pickup;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip interact;
    [SerializeField] AudioClip dash;
    [SerializeField] AudioClip suck;
    [SerializeField] AudioClip checkpoint;
    [SerializeField] AudioClip death;

    // Start is called before the first frame update
    void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    public void playPickup()
    {
        aus.volume = 0.45f;
        aus.PlayOneShot(pickup);
    }

    public void playJump()
    {
        aus.volume = 0.45f;
        aus.PlayOneShot(jump);

    }

    public void playInteract()
    {
        aus.volume = 0.45f;
        aus.PlayOneShot(interact);
    }

    public void playDash()
    {
        aus.volume = 0.45f;
        aus.PlayOneShot(dash);
    }

    public void playSuck()
    {
        aus.volume = 0.3f;
        aus.PlayOneShot(suck);
    }

    public void playCheckpoint()
    {
        aus.volume = 0.45f;
        aus.PlayOneShot(checkpoint);
    }

    public void playDeath()
    {
        aus.volume = 0.45f;
        aus.PlayOneShot(death);
    }

}
