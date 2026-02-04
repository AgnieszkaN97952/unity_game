using UnityEngine;
using UnityEngine.SceneManagement;

public class EvidencePickup : MonoBehaviour
{
    [Header("Unique ID (must be unique across whole game!)")]
    public string evidenceId; // np. "TeaRoom_Scroll01"

    [Header("Evidence")]
    public Sprite popupSprite;
    public float popupSeconds = 10f;

    [Header("Message")]
    [TextArea(2, 4)]
    public string pickupMessage = "Zebrano dowód!";

    [Header("Sound (Resources)")]
    public string evidenceSoundPath = "Audio/evidencesound";
    public float evidenceVolume = 0.1f;

    public KeyCode pickupKey = KeyCode.E;

    private bool playerInRange;
    private bool pickedUp;

    private AudioClip evidenceSound;

    void Awake()
    {
        evidenceSound = Resources.Load<AudioClip>(evidenceSoundPath);

        if (string.IsNullOrEmpty(evidenceId))
        {
            // awaryjnie – lepiej ustawiać ręcznie w Inspectorze
            evidenceId = SceneManager.GetActiveScene().name + "_" + gameObject.name;
        }

        // jeśli już zebrane wcześniej -> ukryj od razu
        if (EvidenceCounter.Instance != null && EvidenceCounter.Instance.IsCollected(evidenceId))
        {
            gameObject.SetActive(false);
            return;
        }
    }

    void Update()
    {
        if (!playerInRange || pickedUp) return;

        if (Input.GetKeyDown(pickupKey))
        {
            pickedUp = true;

            // dźwięk
            if (evidenceSound != null && Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(
                    evidenceSound,
                    Camera.main.transform.position,
                    evidenceVolume
                );
            }

            // popup
            EvidenceUIManager.Instance?.ShowEvidence(
                popupSprite,
                pickupMessage,
                popupSeconds
            );

            // zapamiętaj jako zebrane
            EvidenceCounter.Instance?.Collect(evidenceId);

            // usuń przedmiot
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
    }
}
