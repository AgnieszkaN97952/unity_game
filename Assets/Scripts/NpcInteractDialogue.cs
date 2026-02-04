using UnityEngine;
using TMPro;

public class NpcInteractDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Dialogue Scene Image (PNG: background + character)")]
    public Sprite dialogueSceneSprite; // <- tu wrzucasz PNG dla tego NPC

    [Header("References")]
    public DialogueUI dialogueUI;

    [Header("Settings")]
    public float interactDistance = 3f;

    [Header("Screen Hint (shared)")]
    public GameObject interactHint;   // Jeden hint z Canvas (wspólny)
    public string hintText = "[E] Przesłuchaj";

    private Transform player;

    // Globalnie: który NPC aktualnie kontroluje hint
    private static NpcInteractDialogue activeNpc;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Ukryj hint na starcie
        if (interactHint != null)
            interactHint.SetActive(false);
    }

    void Update()
    {
        if (player == null || dialogueUI == null) return;

        // Jeśli dialog otwarty -> ukryj hint
        if (dialogueUI.IsOpen())
        {
            if (activeNpc == this)
                activeNpc = null;

            if (interactHint != null && activeNpc == null)
                interactHint.SetActive(false);

            return;
        }

        float dist = Vector3.Distance(player.position, transform.position);
        bool closeEnough = dist <= interactDistance;

        // Jeśli odszedłeś od aktywnego NPC -> zwolnij
        if (!closeEnough && activeNpc == this)
            activeNpc = null;

        // Jeśli jesteś blisko -> wybierz najbliższego jako activeNpc
        if (closeEnough)
        {
            if (activeNpc == null)
                activeNpc = this;
            else
            {
                float activeDist = Vector3.Distance(
                    player.position,
                    activeNpc.transform.position
                );

                if (dist < activeDist)
                    activeNpc = this;
            }
        }

        // ✅ TYLKO aktywny NPC pokazuje hint
        if (interactHint != null)
        {
            if (activeNpc == this)
            {
                interactHint.SetActive(true);

                TMP_Text tmp = interactHint.GetComponentInChildren<TMP_Text>();
                if (tmp != null)
                    tmp.text = hintText;
            }
            else
            {
                if (activeNpc == null)
                    interactHint.SetActive(false);
            }
        }

        // ✅ Start dialogu tylko dla aktywnego NPC
        if (activeNpc == this && Input.GetKeyDown(KeyCode.E))
        {
            // 🔹 ustaw obrazek sceny (background + postać)
            if (dialogueSceneSprite != null)
                dialogueUI.SetSceneSprite(dialogueSceneSprite);
            else
                dialogueUI.SetSceneSprite(null);

            // 🔹 start dialogu
            dialogueUI.StartDialogue(lines);

            if (interactHint != null)
                interactHint.SetActive(false);

            activeNpc = null;
        }
    }
}
