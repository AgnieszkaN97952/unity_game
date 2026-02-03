using UnityEngine;

public class EvidencePickup : MonoBehaviour
{
    [Header("Evidence")]
    public Sprite popupSprite;
    public float popupSeconds = 10f;

    [Header("Message")]
    [TextArea(2, 4)]
    public string pickupMessage = "Zebrano dowód!";

    public KeyCode pickupKey = KeyCode.E;

    bool playerInRange;

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(pickupKey))
        {
            EvidenceUIManager.Instance?.ShowEvidence(
                popupSprite,
                pickupMessage,
                popupSeconds
            );

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
