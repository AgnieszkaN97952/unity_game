using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvidenceUIManager : MonoBehaviour
{
    public static EvidenceUIManager Instance;

    [Header("UI")]
    public Image popupImage;
    public TMP_Text popupText;
    public Image backgroundImage;   // 👈 NOWE

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip popupSound;

    Coroutine showRoutine;

    void Awake()
    {
        Instance = this;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        HideInstant();
    }

    public void ShowEvidence(Sprite sprite, string text, float seconds = 10f)
    {
        if (audioSource != null && popupSound != null)
            audioSource.PlayOneShot(popupSound);

        if (showRoutine != null)
            StopCoroutine(showRoutine);

        showRoutine = StartCoroutine(ShowRoutine(sprite, text, seconds));
    }

    IEnumerator ShowRoutine(Sprite sprite, string text, float seconds)
    {
        // Background
        if (backgroundImage != null)
            backgroundImage.color = new Color(0, 0, 0, 0.6f);

        // Image
        if (popupImage != null && sprite != null)
        {
            popupImage.sprite = sprite;
            popupImage.color = new Color(1, 1, 1, 1);
        }

        // Text
        if (popupText != null)
        {
            popupText.text = text;
            popupText.color = new Color(1, 1, 1, 1);
        }

        yield return new WaitForSeconds(seconds);

        HideInstant();
        showRoutine = null;
    }

    void HideInstant()
    {
        if (popupImage != null)
            popupImage.color = new Color(1, 1, 1, 0);

        if (popupText != null)
            popupText.color = new Color(1, 1, 1, 0);

        if (backgroundImage != null)
            backgroundImage.color = new Color(0, 0, 0, 0); // ukryj
    }
}
