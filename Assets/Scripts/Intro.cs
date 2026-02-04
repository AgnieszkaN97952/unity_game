using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{

    [Header("Intro")]
    [TextArea(2, 5)] public string[] Introtxt;
    // Start is called before the first frame update
    public float typingSpeed = 0.04f;

    TMP_Text introText;
    string[] chosenLines;

    int lineIndex = 0;
    bool isTyping = false;
    bool introFinished = false;
    bool started = false;

    Coroutine typingCoroutine;

    void Start()
    {
        GameObject textObj = GameObject.Find("IntroText");

        if (textObj == null)
        {
            Debug.LogError("EpilogText not found!");
            return;
        }

        introText = textObj.GetComponent<TMP_Text>();
        started = true;

        chosenLines = Introtxt;
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
            introText.text = chosenLines[lineIndex];
            isTyping = false;
            return;
        }

        if (introFinished)
        {
            SceneManager.LoadScene("TeaRoom");
            return;
        }

        lineIndex++;

        if (lineIndex < chosenLines.Length)
        {
            StartTypingLine();
        }
        else
        {
            introFinished = true;
        }
    }

    void StartTypingLine()
    {
        typingCoroutine = StartCoroutine(TypeLine(chosenLines[lineIndex]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        introText.text = "";

        foreach (char c in line)
        {
            introText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
