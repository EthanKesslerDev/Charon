using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PUIManager : MonoBehaviour
{
    public Image shroud;
    public GameObject endScreen;
    public GameObject score;

    /// <summary>
    /// Fade in shroud that covers player screen
    /// </summary>
    /// <param name="fadeAway"></param>
    /// <returns></returns>
    public IEnumerator FadeShroud(bool fadeAway)
    {
        if (fadeAway)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                shroud.color = new Color(0, 0, 0, i);
                
                yield return null;
            }
            
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                shroud.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        shroud.gameObject.SetActive(!fadeAway);
    }

    void Start()
    {
        StartCoroutine(FadeShroud(true));
        shroud.gameObject.SetActive(false);
        Debug.Log("Fading");
    }

    public void gameEndScreen()
    {
        Cursor.lockState = CursorLockMode.None;
        Debug.Log("Something is meant to be happening");
        Boat theBoat = GameObject.FindObjectOfType<Boat>();
        float distanceTravelled = theBoat.transform.position.z;
        score.GetComponent<TextMeshProUGUI>().SetText($"You travelled {distanceTravelled} metres.");
        endScreen.SetActive(true);
    }
}
