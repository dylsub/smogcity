using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneManager : MonoBehaviour
{
    int count = 0;
    [SerializeField] LevelManager lm;
    [SerializeField] GameObject Lore1;
    [SerializeField] Image blackScreen;
    [SerializeField] TextMeshProUGUI sText;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI endText;
    [SerializeField] Image badEnding;
    [SerializeField] Image neutralEnding;
    [SerializeField] Image goodEnding;

    GameState gs;
    float finalScore = 0;
    float batteryCount = 0;
    float totalScore = 0;
    int gameScore = -1;

    // Start is called before the first frame update
    void Start()
    {

        gs = FindObjectOfType<GameState>();
    }

    void FixedUpdate()
    {
        count++;

        if (lm.GetLevel() == 1)
        {
            if (count == 400)
            {
                blackScreen.GetComponent<Animator>().SetBool("transition", true);

            }
            if (count == 415)
            {
                Destroy(Lore1);
            }
            if (count == 500)
            {
                blackScreen.GetComponent<Animator>().SetBool("transition", false);
            }
            if (count == 800)
            {
                blackScreen.GetComponent<Animator>().SetBool("transition", true);
            }
            if (count == 815)
            {
                lm.NextLevel();
            }
        }

        if (lm.GetLevel() == 5)
        {
            if (count == 1)
            {
                if (gs.getPollutionCount() >= 365)
                {
                    finalScore = 100;
                }
                else
                {
                    finalScore = Mathf.RoundToInt((gs.getPollutionCount() / 365) * 100);
                }
                batteryCount = gs.getBatteryCount();
                totalScore = finalScore + batteryCount * 2;

                if (totalScore <= 40f)
                {
                    gameScore = 1;
                }
                else if (totalScore > 40f && totalScore < 100f)
                {
                    gameScore = 2;
                }
                else if (totalScore >= 100f)
                {
                    gameScore = 3;
                }

                sText.text = "Smog Cleared: " + finalScore + "%\n\nBatteries Collected: " + batteryCount + "\nBattery Bonus: " + (batteryCount * 2) + "%\n\nFinal Game Score: " + totalScore + "%";

                if (gameScore == 1)
                {
                    text.color = new Color32(255, 93, 238, 255);
                    endText.color = new Color32(255, 93, 238, 255);
                    endText.text = "Bad Ending (<40%)";
                    Destroy(goodEnding);
                    Destroy(neutralEnding);
                }
                else if (gameScore == 2)
                {
                    text.color = new Color32(255, 175, 94, 255);
                    endText.color = new Color32(255, 175, 94, 255);
                    endText.text = "Neutral Ending (41%-99%)";
                    Destroy(goodEnding);
                    Destroy(badEnding);
                }
                else if (gameScore == 3)
                {
                    text.color = new Color32(135, 217, 95, 255);
                    endText.color = new Color32(135, 217, 95, 255);
                    endText.text = "Good Ending (>100%)";
                    Destroy(neutralEnding);
                    Destroy(badEnding);
                }


            }
            if (count == 1500)
            {
                gs.setBatteryCount(0);
                gs.setPollutionCount(0);
                lm.LoadLevel(0);
            }
        }

    }

    public int getGameScore()
    {
        return gameScore;
    }

}
