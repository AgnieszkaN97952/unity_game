using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;

    void Awake()
    {
        // Jeśli już istnieje – usuń duplikat
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Ustaw jako jedyny
        instance = this;

        // Nie niszcz przy zmianie sceny
        DontDestroyOnLoad(gameObject);
    }
}
