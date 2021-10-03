using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("TheVoid");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void AdjustSens(float newSens)
    {
        //200 is max, 100 is 0.5
        settings.lookSens = newSens;
    }
}
