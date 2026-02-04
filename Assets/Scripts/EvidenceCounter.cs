using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EvidenceCounter : MonoBehaviour
{
    public static EvidenceCounter Instance;

    [Header("Progress")]
    public int target = 15;
    public string nextSceneName = "WhoKilled";

    // trzymamy ID zebranych dowodów
    private HashSet<string> collectedIds = new HashSet<string>();

    public int CollectedCount => collectedIds.Count;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsCollected(string id)
    {
        if (string.IsNullOrEmpty(id)) return false;
        return collectedIds.Contains(id);
    }

    public void Collect(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        // jeśli już było zebrane, nic nie rób
        if (!collectedIds.Add(id)) return;

        // osiągnięto cel?
        if (collectedIds.Count >= target)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    public void ResetAll()
    {
        collectedIds.Clear();
    }
}
