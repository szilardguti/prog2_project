using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleOrderManager : MonoBehaviour
{
    public TextMeshProUGUI orderText;
    Queue<Unit> battleOrder = new Queue<Unit>();
    private List<Unit> OrderUnits;

    public void SetupBattleOrder(List<Unit> Units)
    {
        foreach (var unit in Units)
        {
            unit.setSpeed();
        }

        OrderUnits = Units;
        RefreshBattleOrder();
    }

    public void RefreshBattleOrder()
    {

        for (int i = 0; i < 20; i++)
        {
            OrderUnits = OrderUnits.OrderByDescending(s => s.getSpeed()).ToList();
            battleOrder.Enqueue(OrderUnits[0]);
            OrderUnits[0].tookTurn();
        }

    }

    public Unit GetOnTurnUnit()
    {
        if (battleOrder.Count < 11)
            RefreshBattleOrder();

        string temp = "";
        foreach (var unit in battleOrder)
        {
            temp += unit.unitName + "    ";
        }

        orderText.text = temp;

        return battleOrder.Dequeue();
    }
}
