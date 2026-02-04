using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    [Header("Intro")]
    [TextArea(2, 5)] public string[] Introtxt;
    public float typingSpeed = 0.04f;

    [Header("Typing Sound (Resources)")]
    public string typingSoundPath = "Audio/EpilogSound"; // Assets/Resources/Audio/EpilogSound(.mp3/.wav)
    [Range(0f, 1f)] public float typingVolume = 0.2f;

    private TMP_Text introText;
    private string[] chosenLines;

    private int lineIndex = 0;
    private bool isTyping = false;
    private bool introFinished = false;
    private bool started = false;

    private Coroutine typingCoroutine;

    private AudioSource audioSource;
    private AudioClip typingClip;

    void Start()
    {
        GameObject textObj = GameObject.Find("IntroText");

        if (textObj == null)
        {
            Debug.LogError("IntroText not found!");
            return;
        }

        introText = textObj.GetComponent<TMP_Text>();

        // AudioSource (na tym samym obiekcie co Intro)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = true;
        audioSource.spatialBlend = 0f; // 2D
        audioSource.volume = typingVolume;

        typingClip = Resources.Load<AudioClip>(typingSoundPath);
        if (typingClip == null)
            Debug.LogError($"Intro: Nie znaleziono dźwięku w Resources/{typingSoundPath}");

        started = true;

        chosenLines = Introtxt;
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

            introText.text = chosenLines[lineIndex];
            isTyping = false;

            PauseTypingSound(); // ✅ pauza bez resetu
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
            PauseTypingSound(); // na wszelki wypadek
        }
    }

    void StartTypingLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(chosenLines[lineIndex]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        introText.text = "";

        ResumeTypingSound(); // ✅ wznów od miejsca

        foreach (char c in line)
        {
            introText.text += c;
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

        // UnPause wznawia, Play uruchamia jeśli jeszcze nigdy nie grał
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
