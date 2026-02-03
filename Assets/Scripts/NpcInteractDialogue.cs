using UnityEngine;

public class NpcInteractDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("References")]
    public DialogueUI dialogueUI;

    [Header("Settings")]
    public float interactDistance = 3f;

    [Header("Optional UI")]
    public GameObject interactHint;

    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (interactHint)
            interactHint.SetActive(false);
    }

    void Update()
    {
        if (player == null || dialogueUI == null) return;

        float dist = Vector3.Distance(player.position, transform.position);

        bool closeEnough = dist <= interactDistance;

        // Pokazuj hint
        if (interactHint)
            interactHint.SetActive(closeEnough && !dialogueUI.IsOpen());

        // Start dialogu
        if (closeEnough && Input.GetKeyDown(KeyCode.E) && !dialogueUI.IsOpen())
        {
            dialogueUI.StartDialogue(lines);
            if (interactHint) interactHint.SetActive(false);
        }
    }
}
