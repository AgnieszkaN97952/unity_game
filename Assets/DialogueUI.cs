using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;

    [Header("Typing")]
    public float textSpeed = 0.05f;

    private string[] lines;
    private int index;
    private Coroutine typingRoutine;
    private bool isOpen;

    void Awake()
    {
        gameObject.SetActive(false); // na starcie ukryty
    }

    void Update()
    {
        if (!isOpen) return;

        // LPM przechodzi dalej / skipuje pisanie
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
                NextLine();
            else
                SkipTyping();
        }
    }

    public void StartDialogue(string[] newLines)
    {
        if (newLines == null || newLines.Length == 0) return;

        lines = newLines;
        index = 0;

        gameObject.SetActive(true);
        isOpen = true;

        textComponent.text = "";
        StartTypingCurrentLine();
    }

    private void StartTypingCurrentLine()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = StartCoroutine(TypeLine(lines[index]));
    }

    private IEnumerator TypeLine(string line)
    {
        textComponent.text = "";
        foreach (char c in line)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        typingRoutine = null;
    }

    private void SkipTyping()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = null;
        textComponent.text = lines[index];
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartTypingCurrentLine();
        }
        else
        {
            Close();
        }
    }

    public void Close()
    {
        isOpen = false;
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = null;

        gameObject.SetActive(false);
    }

    public bool IsOpen() => isOpen;
}
