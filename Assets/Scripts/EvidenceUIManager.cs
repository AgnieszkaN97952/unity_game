using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvidenceUIManager : MonoBehaviour
{
    public static EvidenceUIManager Instance;

    public Image popupImage;
    public TMP_Text popupText;

    Coroutine showRoutine;

    void Awake()
    {
        Instance = this;
        HideInstant();
    }

    public void ShowEvidence(Sprite sprite, string text, float seconds = 10f)
    {
        if (showRoutine != null)
            StopCoroutine(showRoutine);

        showRoutine = StartCoroutine(ShowRoutine(sprite, text, seconds));
    }

    IEnumerator ShowRoutine(Sprite sprite, string text, float seconds)
    {
        popupImage.sprite = sprite;
        popupImage.color = new Color(1, 1, 1, 1);

        popupText.text = text;
        popupText.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(seconds);

        HideInstant();
        showRoutine = null;
    }

    void HideInstant()
    {
        popupImage.color = new Color(1, 1, 1, 0);
        popupText.color = new Color(1, 1, 1, 0);
    }
}
