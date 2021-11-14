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

    public void ShowBattleStation(bool hasTurn = false)
    {
        this.gameObject.GetComponent<Renderer>().enabled = true;
        if(hasTurn)
            GetComponent<Renderer>().material.color = Color.black;
        else
            GetComponent<Renderer>().material.color = Color.red;
    }


    public void HideBattleStation()
    {
        this.gameObject.GetComponent<Renderer>().enabled = false;
    }
}
