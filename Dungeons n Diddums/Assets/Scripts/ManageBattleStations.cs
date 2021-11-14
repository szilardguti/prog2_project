﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBattleStations : MonoBehaviour
{
    public SelectBattleStation playerBattleStation1;
    public SelectBattleStation playerBattleStation2;
    public SelectBattleStation playerBattleStation3;
    public SelectBattleStation playerBattleStation4;

    public SelectBattleStation enemyBattleStation1;
    public SelectBattleStation enemyBattleStation2;
    public SelectBattleStation enemyBattleStation3;
    public SelectBattleStation enemyBattleStation4;

    private List<SelectBattleStation> BSList = new List<SelectBattleStation>();

    public Unit ActiveUnit;
    public Unit TurnUnit;
    private SelectBattleStation unitHasTurn;


    // Start is called before the first frame update
    void Start()
    {
        BSList.Add(playerBattleStation1);
        BSList.Add(playerBattleStation2);
        BSList.Add(playerBattleStation3);
        BSList.Add(playerBattleStation4);
        BSList.Add(enemyBattleStation1);
        BSList.Add(enemyBattleStation2);
        BSList.Add(enemyBattleStation3);
        BSList.Add(enemyBattleStation4);
        foreach(var station in BSList)
        {
            station.HideBattleStation();
        }
    }

    public void Selected(SelectBattleStation sBs)
    {
        ActiveUnit = sBs.transform.GetChild(0).GetComponent<Unit>();

        sBs.ShowBattleStation();
        foreach(var station in BSList)
        {
            if(station != sBs && station != unitHasTurn)
            {
                station.HideBattleStation();
            }
            if (station == sBs && station == unitHasTurn)
                station.ShowBattleStation(true);
        }
    }

    public void TurnSelected(Unit unit)
    {
        unitHasTurn = unit.transform.parent.gameObject.GetComponent<SelectBattleStation>();
        unitHasTurn.ShowBattleStation(true);
    }

    public Unit GetActiveUnit()
    {
        return ActiveUnit;
    }
}