using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DoorToGarden : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string targetSceneName = "Garden";

    [Header("Prompt UI (TMP Text)")]
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private string message = "Przejdź [E]";

    [Header("Sound (Resources)")]
    [SerializeField] private string doorSoundPath = "Audio/DoorSound";
    [SerializeField] private float doorVolume = 0.5f;

    [Header("Delay")]
    [SerializeField] private float loadDelay = 0.5f; // ⏱️ czekamy po dźwięku

    private bool playerInRange = false;
    private bool isLoading = false;

    private AudioClip doorSound;

    private void Awake()
    {
        if (promptRoot != null) promptRoot.SetActive(false);
        if (promptText != null) promptText.text = message;

        doorSound = Resources.Load<AudioClip>(doorSoundPath);

        if (doorSound == null)
            Debug.LogError($"DoorToGarden: Nie znaleziono dźwięku w Resources/{doorSoundPath}");
    }

    private void Update()
    {
        if (!playerInRange || isLoading) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(OpenDoorAndLoad());
        }
    }

    private IEnumerator OpenDoorAndLoad()
    {
        isLoading = true;

        // 🔊 dźwięk
        if (doorSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(
                doorSound,
                Camera.main.transform.position,
                doorVolume
            );
        }

        // ⏱️ poczekaj
        yield return new WaitForSeconds(loadDelay);

        // 🚪 zmień scenę
        SceneManager.LoadScene(targetSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (promptText != null) promptText.text = message;
        if (promptRoot != null) promptRoot.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;

        if (promptRoot != null) promptRoot.SetActive(false);
    }
}
