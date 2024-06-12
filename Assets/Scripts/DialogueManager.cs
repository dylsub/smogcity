using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    bool currentDialogueStatus = false;
    [SerializeField] TextMeshProUGUI text;
    Image dialogueBox;
    PlayerController move;
    GameObject NPC1;
    // GameState gameState;
    // LevelLoader levelLoader;
    Animator fade;
    Animator fadeText;
    int delayCount = 0;
    int sentenceCount = 0;

    bool isTyping = false;

    char[] letters;

    // Start is called before the first frame update
    void Start()
    {
        NPC1 = GameObject.FindGameObjectWithTag("NPC1");
        dialogueBox = GameObject.FindWithTag("Dialogue").GetComponent<Image>();
        move = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sentences = new Queue<string>();
    }

    void FixedUpdate()
    {
        delayCount++;

        if (delayCount == 8)
        {
            move.setIsTalking(false);
            move.setCanMove(true);
        }

        if (isTyping && sentenceCount < letters.Length - 2)
        {
            text.text += letters[sentenceCount];
            text.text += letters[sentenceCount + 1];
            text.text += letters[sentenceCount + 2];
            sentenceCount += 3;
        }
        else if (isTyping && sentenceCount < letters.Length - 1)
        {
            text.text += letters[sentenceCount];
            text.text += letters[sentenceCount + 1];
            sentenceCount += 2;
        }
        else if (isTyping && sentenceCount < letters.Length)
        {
            text.text += letters[sentenceCount];
            sentenceCount += 1;
        }
        else if (isTyping)
        {
            isTyping = false;
        }


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            sentences.Clear();
            DisplayNextSentence();
            EndDialogue();
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        delayCount = 30;
        move.setCanMove(false);
        move.setIsTalking(true);
        currentDialogueStatus = true;
        dialogueBox.color = new Color(255f, 255f, 255f, 255f);
        text.color = new Color(0f, 0f, 0f, 255f);
        Debug.Log("Starting conversation with " + dialogue.name);
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            currentDialogueStatus = false;

            isTyping = false;
            text.text = "";
            // dialogueBox.color = new Color(255f, 255f, 255f, 255f);
            StopAllCoroutines();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();

        text.text = "";
        letters = sentence.ToCharArray();
        sentenceCount = 0;
        isTyping = true;
    }

    void EndDialogue()
    {
        delayCount = 0;
        Debug.Log("Ending conversation");
        dialogueBox.color = new Color(255f, 255f, 255f, 0f);
        text.color = new Color(0f, 0f, 0f, 0f);
    }

    public bool getCurrentDialogueStatus()
    {
        return currentDialogueStatus;
    }

}
