using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI textComponent;

    [Header("Scene Image (one PNG: background + character)")]
    public Image sceneImage;              // podepnij DialogueSceneImage
    [Range(0f, 1f)] public float sceneAlpha = 1f;

    [Header("Typing")]
    public float textSpeed = 0.05f;

    [Header("Audio (typing loop)")]
    public AudioSource dialogueSource;
    [Range(0f, 1f)] public float dialogueVolume = 0.2f;

    private AudioClip dialogueClip;
    private const string DialogueClipPath = "Audio/DialogueSound";

    private string[] lines;
    private int index;
    private Coroutine typingRoutine;
    private bool isOpen;

    void Awake()
    {
        // ukryj na start
        if (sceneImage != null)
        {
            sceneImage.enabled = false;
            sceneImage.sprite = null;
        }

        if (dialogueSource == null)
            dialogueSource = GetComponent<AudioSource>();

        dialogueClip = Resources.Load<AudioClip>(DialogueClipPath);
        StopTypingSound();

        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isOpen) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
                NextLine();
            else
                SkipTyping();
        }
    }

    // ✅ NPC ustawia obrazek sceny PRZED startem dialogu
    public void SetSceneSprite(Sprite sprite)
    {
        if (sceneImage == null) return;

        sceneImage.sprite = sprite;
        sceneImage.color = new Color(1f, 1f, 1f, sceneAlpha);
        sceneImage.enabled = (sprite != null);
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
        StartTypingSound();

        foreach (char c in line)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        StopTypingSound();
        typingRoutine = null;
    }

    private void SkipTyping()
    {
        if (typingRoutine != null) StopCoroutine(typingRoutine);
        typingRoutine = null;

        textComponent.text = lines[index];
        StopTypingSound();
    }

    private void NextLine()
    {
        StopTypingSound();

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

        StopTypingSound();

        // ✅ schowaj obraz po dialogu
        if (sceneImage != null)
        {
            sceneImage.enabled = false;
            sceneImage.sprite = null;
        }

        gameObject.SetActive(false);
    }

    private void StartTypingSound()
    {
        if (dialogueSource == null || dialogueClip == null) return;

        dialogueSource.loop = true;
        dialogueSource.volume = dialogueVolume;
        dialogueSource.clip = dialogueClip;

        dialogueSource.Stop();
        dialogueSource.Play();
    }

    private void StopTypingSound()
    {
        if (dialogueSource != null && dialogueSource.isPlaying)
            dialogueSource.Stop();
    }

    public bool IsOpen() => isOpen;
}
