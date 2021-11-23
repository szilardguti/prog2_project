using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndOfGamePanel : MonoBehaviour
{
    public TextMeshProUGUI endText;
    public Button nextLevelButton;
    public bool unSkippable;

    public void Activate(string text, bool hasWon = false)
    {
        this.gameObject.SetActive(true);
        endText.text = text;
        unSkippable = true;

        if (hasWon)
            nextLevelButton.gameObject.SetActive(true);
    }

    public void NextLevelButton()
    {
        this.gameObject.SetActive(false);
        nextLevelButton.gameObject.SetActive(false);
        unSkippable = false;
    }

    public void GoToMenu()
    {
        nextLevelButton.gameObject.SetActive(false);
        unSkippable = false;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("quit was sent!");
        Application.Quit();
    }
}
