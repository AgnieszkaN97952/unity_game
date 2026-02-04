using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Epilog : MonoBehaviour
{

    [Header("Dialogue Variants")]
    [TextArea(2, 5)] public string[] Ryu;
    [TextArea(2, 5)] public string[] Sora;
    [TextArea(2, 5)] public string[] Aiko;
    [TextArea(2, 5)] public string[] Miko;
    [TextArea(2, 5)] public string[] Zero;

    public float typingSpeed = 0.04f;

    TMP_Text epilogText;
    string[] chosenLines;

    int lineIndex = 0;
    bool isTyping = false;
    bool epilogFinished = false;
    bool started = false;

    Coroutine typingCoroutine;

    void Start()
    {
        GameObject textObj = GameObject.Find("EpilogText");

        if (textObj == null)
        {
            Debug.LogError("EpilogText not found!");
            return;
        }

        epilogText = textObj.GetComponent<TMP_Text>();

        Debug.Log("Chosen = " + ButtonBehavior.ChosenKiller);

        started = true;

        switch (ButtonBehavior.ChosenKiller)
        {
            case "Ryu": chosenLines = Ryu; break;
            case "Sora": chosenLines = Sora; break;
            case "Aiko": chosenLines = Aiko; break;
            case "Miko": chosenLines = Miko; break;
            default: chosenLines = Zero; break;
        }

        StartTypingLine();
    }

    void Update()
    {
        if (!started)
            return;

        if (!Input.GetMouseButtonDown(0))
            return;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            epilogText.text = chosenLines[lineIndex];
            isTyping = false;
            return;
        }

        if (epilogFinished)
        {
            SceneManager.LoadScene("End");
            return;
        }

        lineIndex++;

        if (lineIndex < chosenLines.Length)
        {
            StartTypingLine();
        }
        else
        {
            epilogFinished = true;
        }
    }

    void StartTypingLine()
    {
        typingCoroutine = StartCoroutine(TypeLine(chosenLines[lineIndex]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        epilogText.text = "";

        foreach (char c in line)
        {
            epilogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}