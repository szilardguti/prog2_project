using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialButton : MonoBehaviour
{
    public Slider counterSlider;

    private void Start()
    {
        counterSlider.GetComponent<Slider>();    
    }

    public void SetSpecialSlider(Unit unit)
    {
        counterSlider.value = unit.specialLoad;
        if(unit.hasSpecial)
            this.gameObject.GetComponent<Image>().color = Color.red;
        else
            this.gameObject.GetComponent<Image>().color = Color.white;
    }

    public void IncrementSpecial(Unit unit)
    {
        unit.specialLoad++;
        counterSlider.value = unit.specialLoad;

        this.gameObject.GetComponent<Image>().color = Color.white;

        if (counterSlider.value == counterSlider.maxValue && !unit.hasSpecial)
        {
            this.gameObject.GetComponent<Image>().color = Color.red;
            unit.hasSpecial = true;
            return;
        }

        if (unit.hasSpecial)
        {
            this.gameObject.GetComponent<Image>().color = Color.red;
        }
    }

    public void DidSpecialAttack(Unit unit)
    {
        unit.hasSpecial = false;
        counterSlider.value = 0;
        Debug.Log("do special attack");
    }
}
