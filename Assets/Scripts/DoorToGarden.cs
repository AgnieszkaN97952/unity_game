using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DoorToGarden : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string targetSceneName = "Garden";

    [Header("Prompt UI (TMP Text)")]
    [SerializeField] private GameObject promptRoot; // np. cały obiekt Text (żeby go włączać/wyłączać)
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private string message = "Przejdź [E]";

    private bool playerInRange = false;

    private void Awake()
    {
        // Bezpiecznie ukryj prompt na starcie
        if (promptRoot != null) promptRoot.SetActive(false);
        if (promptText != null) promptText.text = message;
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene(targetSceneName);
        }
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
