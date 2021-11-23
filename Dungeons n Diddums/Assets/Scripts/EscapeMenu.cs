using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenu : MonoBehaviour
{
    public GameObject endPanel;
    private bool isShowing = false;
    void Update()
    {
        if (Input.GetKeyDown("escape") && endPanel.GetComponent<EndOfGamePanel>().unSkippable == false)
        {
            isShowing = !isShowing;
            endPanel.SetActive(isShowing);
        }
    }
}
