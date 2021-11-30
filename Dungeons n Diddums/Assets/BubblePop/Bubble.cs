using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bubble : MonoBehaviour
{
    public Text number;
    public int value;
    
    private BubblePopManager gameManager;

    private void Start()
    {
        gameManager = BubblePopManager.gameManager;
    }

    public void SetNumber(int num)
    {
        value = num;
        number.text = num.ToString();
    }

    private void OnMouseDown()
    {
        gameManager.BubblePressed(value);
        Destroy(this.gameObject);
    }
}
