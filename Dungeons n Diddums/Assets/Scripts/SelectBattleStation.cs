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

    public void ShowBattleStation()
    {
        this.gameObject.GetComponent<Renderer>().enabled = true;
    }

    public void HideBattleStation()
    {
        this.gameObject.GetComponent<Renderer>().enabled = false;
    }
}
