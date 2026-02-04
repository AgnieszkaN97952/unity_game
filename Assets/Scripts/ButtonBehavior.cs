using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonBehavior : MonoBehaviour
{
    public static string ChosenKiller;

    public void RyuClicked()
    {
        ChosenKiller = "Ryu";
        SceneManager.LoadScene("Epilog");
    }

    public void SoraClicked()
    {
        ChosenKiller = "Sora";
        SceneManager.LoadScene("Epilog");
    }

    public void AikoClicked()
    {
        ChosenKiller = "Aiko";
        SceneManager.LoadScene("Epilog");
    }

    public void MikoClicked()
    {
        ChosenKiller = "Miko";
        SceneManager.LoadScene("Epilog");
    }
}
