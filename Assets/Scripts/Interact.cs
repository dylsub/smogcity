using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    PlayerController player;
    [SerializeField] GameObject target;
    Vector3 pos;
    Vector3 playerPos;
    [SerializeField] float xRange = 25;
    [SerializeField] float yRange = 25;
    [SerializeField] SpriteRenderer interact;
    bool isTriggered = false;
    DialogueTrigger dialogueTrigger;
    DialogueManager dialogueManager;
    SoundManager sm;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        pos = target.transform.position;
        dialogueTrigger = GetComponent<DialogueTrigger>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        sm = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        if (playerPos.x > pos.x - xRange
                && playerPos.x < pos.x + xRange
                && playerPos.y > pos.y - yRange
                && playerPos.y < pos.y + yRange)
        {
            if (Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.K))
            {
                if (!dialogueManager.getCurrentDialogueStatus())
                {
                    dialogueTrigger.TriggerDialogue();
                    sm.playInteract();
                }
                else dialogueManager.DisplayNextSentence();
            }
            interact.color = new Color(255f, 255f, 255f, 255f);
            isTriggered = true;

            if (player.getIsTalking())
            {
                interact.color = new Color(255f, 255f, 255f, 0f);
            }
        }
        else
        {
            interact.color = new Color(255f, 255f, 255f, 0f);
            isTriggered = false;
        }
    }

    public bool getIsTriggered()
    {
        return isTriggered;
    }


}
