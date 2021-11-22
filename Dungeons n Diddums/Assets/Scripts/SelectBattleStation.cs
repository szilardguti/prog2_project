using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBattleStation : MonoBehaviour
{
    public ManageBattleStations manager;

    private void OnMouseDown()
    {
        manager.Selected(this);
    }

    public void ShowBattleStation(Color color)
    {
        this.gameObject.GetComponent<Renderer>().enabled = true;
        GetComponent<Renderer>().material.color = color;
    }


    public void HideBattleStation()
    {
        this.gameObject.GetComponent<Renderer>().enabled = false;
    }

    public void DoNotTrigger()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
}
