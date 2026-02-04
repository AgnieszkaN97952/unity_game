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

    [Header("Typing")]
    public float typingSpeed = 0.04f;

    [Header("Typing Sound (Resources)")]
    public string typingSoundPath = "Audio/EpilogSound"; // Assets/Resources/Audio/EpilogSound(.mp3/.wav)
    [Range(0f, 1f)] public float typingVolume = 0.2f;

    private TMP_Text epilogText;
    private string[] chosenLines;

    private int lineIndex = 0;
    private bool isTyping = false;
    private bool epilogFinished = false;
    private bool started = false;

    private Coroutine typingCoroutine;

    private AudioSource audioSource;
    private AudioClip typingClip;

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

        // AudioSource (na tym samym obiekcie co Epilog)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // 2D
        audioSource.volume = typingVolume;

        typingClip = Resources.Load<AudioClip>(typingSoundPath);
        if (typingClip == null)
            Debug.LogError($"Epilog: Nie znaleziono dźwięku w Resources/{typingSoundPath}");

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
        if (!started) return;
        if (!Input.GetMouseButtonDown(0)) return;

        if (isTyping)
        {
            // Skip typing
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = null;

            epilogText.text = chosenLines[lineIndex];
            isTyping = false;

            PauseTypingSound(); // ✅ pauza bez resetu
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
            PauseTypingSound();
        }
    }

    void StartTypingLine()
    {
        if (chosenLines == null || chosenLines.Length == 0) return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(chosenLines[lineIndex]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        epilogText.text = "";

        ResumeTypingSound(); // ✅ wznów od miejsca

        foreach (char c in line)
        {
            epilogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        PauseTypingSound(); // ✅ pauza bez resetu
        typingCoroutine = null;
    }

    private void ResumeTypingSound()
    {
        if (audioSource == null || typingClip == null) return;

        if (audioSource.clip != typingClip)
            audioSource.clip = typingClip;

        audioSource.volume = typingVolume;
        audioSource.loop = true;

        audioSource.UnPause();
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    private void PauseTypingSound()
    {
        if (audioSource == null) return;
        audioSource.Pause(); // ✅ zapamiętuje miejsce
    }
}
