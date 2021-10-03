using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidController : MonoBehaviour
{
    bool gameOver = false;
    public void GameEnd()
    {
        if(!gameOver)
        {
            PlayDeath();
            Time.timeScale = 0;
            PUIManager pUIManager = GameObject.FindObjectOfType<PUIManager>();
            pUIManager.gameEndScreen();
            gameOver = true;
        }

    }
    void PlayDeath()
    {
        //0.2 pitch variation
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.Play();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("TheVoid");
    }

    public void Menu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
