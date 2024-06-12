using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameState : MonoBehaviour
{
    Pollute[] p;
    static float pollutionCount;
    static float totalPollution = 365;
    static float totalBatteryCount;
    static float timer;
    float batteryCount = 0;
    [SerializeField] TextMeshProUGUI polluteText;
    [SerializeField] TextMeshProUGUI batteryText;
    public int targetFrameRate;
    LevelManager lm;

    bool hasStarted = false;

    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        lm = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log(pollutionCount);
        if (lm.GetLevel() != 1 && lm.GetLevel() != 5 && lm.GetLevel() != 0)
        {
            updateText();
        }

        if (hasStarted)
        {
            timer += Time.deltaTime;
            Debug.Log(timer);
        }
    }

    public void updateText()
    {
        polluteText = FindObjectOfType<polluteText>().GetComponent<TextMeshProUGUI>();
        batteryText = FindObjectOfType<batteryText>().GetComponent<TextMeshProUGUI>();

        // polluteText.text = Mathf.RoundToInt((pollutionCount / totalPollution) * 100) + "%";
        // batteryText.text = batteryCount + "/6";

        addToPollutionCount(0);
        addToBatteryCount(0);
    }

    public void addToPollutionCount(int num)
    {
        pollutionCount += num;
        if (pollutionCount >= totalPollution)
        {
            polluteText.text = "100%";
            return;
        }
        polluteText.text = Mathf.RoundToInt((pollutionCount / totalPollution) * 100) + "%";
    }

    public void setPollutionCount(float num)
    {
        pollutionCount = num;
    }

    public void setBatteryCount(float num)
    {
        totalBatteryCount = num;
    }

    public void addToBatteryCount(int num)
    {
        batteryCount += num;
        totalBatteryCount += num;
        batteryText.text = batteryCount + "/6";
    }

    public float getPollutionCount()
    {
        return pollutionCount;
    }

    public float getBatteryCount()
    {
        return totalBatteryCount;
    }


}
