using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndOfGamePanel : MonoBehaviour
{
    public TextMeshProUGUI endText;

        public void Activate(string text)
    {
        this.gameObject.SetActive(true);
        endText.text = text;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("quit was sent!");
        Application.Quit();
    }
}
