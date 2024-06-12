using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    LevelManager lm;
    AudioSource aus;
    [SerializeField] AudioClip song1;
    [SerializeField] AudioClip song2;
    [SerializeField] AudioClip song3;
    CutsceneManager csm;

    bool isLevel1 = false;
    bool isLevel2 = false;
    bool isLevel3 = false;
    bool isLevel4 = false;
    int delayCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        aus = GetComponent<AudioSource>();
        lm = FindObjectOfType<LevelManager>();
        csm = FindObjectOfType<CutsceneManager>();
    }

    public void setDelayCount(int count)
    {
        delayCount = count;
    }

    void FixedUpdate()
    {
        delayCount++;
        if ((lm.GetLevel() == 1 || lm.GetLevel() == 2) && isLevel1 == false && delayCount >= 1)
        {
            aus.clip = song1;
            aus.Play();
            isLevel1 = true;
            isLevel2 = false;
            isLevel3 = false;
            isLevel4 = false;
            delayCount = 0;
        }
        else if (lm.GetLevel() == 3 && isLevel2 == false && delayCount >= 50)
        {
            aus.clip = song2;
            aus.Play();
            isLevel2 = true;
            isLevel1 = false;
            isLevel3 = false;
            isLevel4 = false;
            delayCount = 0;
        }
        else if ((lm.GetLevel() == 4) && isLevel3 == false && delayCount >= 20)
        {
            aus.clip = song3;
            aus.Play();
            isLevel3 = true;
            isLevel1 = false;
            isLevel2 = false;
            isLevel4 = false;
            delayCount = 0;
        }
        else if ((lm.GetLevel() == 5) && isLevel4 == false && delayCount >= 10)
        {
            aus.clip = song1;
            aus.Play();
            isLevel3 = false;
            isLevel1 = false;
            isLevel2 = false;
            isLevel4 = true;
            delayCount = 0;
        }
    }

    public void StopMusic()
    {
        aus.Stop();
    }

    //Play Global
    private static MusicManager instance = null;
    public static MusicManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
