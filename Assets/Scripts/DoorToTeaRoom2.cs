using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class DoorToTeaRoom2 : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string targetSceneName = "TeaRoom";

    [Header("Spawn In TeaRoom (exact)")]
    // Dokładne koordy z Twojego screena:
    [SerializeField] private Vector3 spawnPosition = new Vector3(6.487367f, 1.65f, 10.03566f);
    [SerializeField] private float spawnYaw = -120.802f; // rotacja Y

    [Header("Prompt UI (TMP Text)")]
    [SerializeField] private GameObject promptRoot;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private string message = "Przejdź [E]";

    [Header("Sound (Resources)")]
    [SerializeField] private string doorSoundPath = "Audio/DoorSound";
    [SerializeField] private float doorVolume = 0.5f;

    [Header("Delay")]
    [SerializeField] private float loadDelay = 0.5f;

    private bool playerInRange;
    private bool isLoading;
    private AudioClip doorSound;

    // --- STATIC (przetrwa zmianę sceny) ---
    private static bool pendingTeleport;
    private static string pendingScene;
    private static Vector3 pendingPos;
    private static float pendingYaw;
    private static bool subscribed;

    private void Awake()
    {
        if (promptRoot != null) promptRoot.SetActive(false);
        if (promptText != null) promptText.text = message;

        doorSound = Resources.Load<AudioClip>(doorSoundPath);
        if (doorSound == null)
            Debug.LogError($"DoorToTeaRoom2: Nie znaleziono dźwięku w Resources/{doorSoundPath}");

        // Zapisz wartości do statycznych pól (na wypadek, gdyby event odpalił zanim coroutine skończy)
        // (prawdziwe "uzbrojenie" robi się dopiero po E)
        EnsureSubscribed();
    }

    private void EnsureSubscribed()
    {
        if (subscribed) return;
        SceneManager.sceneLoaded += OnSceneLoaded;
        subscribed = true;
    }

    private void OnDestroy()
    {
        // Nie odpinamy eventu tutaj, bo obiekt drzwi znika przy LoadScene,
        // a my CHCEMY, żeby teleport wykonał się po wczytaniu TeaRoom.
        // Event sam przestanie być używany po wykonaniu teleportu (pendingTeleport = false).
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

        // Uzbrój teleport po wczytaniu sceny
        pendingTeleport = true;
        pendingScene = targetSceneName;
        pendingPos = spawnPosition;
        pendingYaw = spawnYaw;

        // 🔊 dźwięk
        if (doorSound != null && Camera.main != null)
        {
            AudioSource.PlayClipAtPoint(doorSound, Camera.main.transform.position, doorVolume);
        }

        // ⏱️ delay
        yield return new WaitForSeconds(loadDelay);

        // 🚪 zmiana sceny
        SceneManager.LoadScene(targetSceneName);
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!pendingTeleport) return;
        if (scene.name != pendingScene) return;

        // znajdź gracza i ustaw pozycję
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = pendingPos;
            player.transform.rotation = Quaternion.Euler(0f, pendingYaw, 0f);
        }
        else
        {
            Debug.LogWarning("DoorToTeaRoom2: Nie znaleziono Player (tag: Player) w scenie docelowej.");
        }

        // rozbrój
        pendingTeleport = false;
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
