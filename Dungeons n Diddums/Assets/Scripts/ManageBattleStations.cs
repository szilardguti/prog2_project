using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBattleStations : MonoBehaviour
{
    public BattleSystem BattleSystem;

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

    private Color SelectedColor = Color.red;
    private Color TurnColor = Color.black;



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
        if(sBs.name.Contains("Enemy"))
            ActiveUnit = sBs.transform.GetChild(BattleSystem.GetSceneLevel()-1).GetComponent<Unit>();
        else
            ActiveUnit = sBs.transform.GetChild(0).GetComponent<Unit>();


        sBs.ShowBattleStation(SelectedColor);
        foreach(var station in BSList)
        {
            if(station != sBs && station != unitHasTurn)
            {
                station.HideBattleStation();
            }
            if (station == sBs && station == unitHasTurn)
                station.ShowBattleStation(TurnColor);
        }
    }

    public void TurnSelected(Unit unit)
    {
        unitHasTurn = unit.transform.parent.gameObject.GetComponent<SelectBattleStation>();
        unitHasTurn.ShowBattleStation(TurnColor);
    }

    public void RemoveShieldFrom(Unit unit)
    {
        for (int i = 0; i < unit.gameObject.transform.parent.childCount; i++)
        {
            if (unit.gameObject.transform.parent.GetChild(i).name.Contains("Shield"))
            {
                Destroy(unit.gameObject.transform.parent.GetChild(i).gameObject);
            }
        }
    }

    public Unit GetActiveUnit()
    {
        return ActiveUnit;
    }
}
