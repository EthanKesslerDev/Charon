using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidController : MonoBehaviour
{
    bool gameOver = false;
    public void GameEnd()
    {
        if(!gameOver)
        {
            Time.timeScale = 0;
            PUIManager pUIManager = GameObject.FindObjectOfType<PUIManager>();
            StartCoroutine(pUIManager.FadeShroud(false));
            pUIManager.gameEndScreen();
            StartCoroutine(pUIManager.FadeShroud(true));
            gameOver = true;
        }
    }
}
