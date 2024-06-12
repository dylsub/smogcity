using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{



    void Start()
    {
    }

    //restarts current level
    public void RestartLevel()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex);
    }

    //switches to next scene
    public void NextLevel()
    {
        if (GetLevel() != 1 && GetLevel() != 0)
        {
            FindObjectOfType<MusicManager>().StopMusic();
            FindObjectOfType<MusicManager>().setDelayCount(0);
        }
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }

    //switches to scene with index of parameter
    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    //returns level index
    public int GetLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
