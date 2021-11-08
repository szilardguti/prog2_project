using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int lvl;
    public int damage;

    public int maxHP;
    public int currHP;

    public bool takeDamage(int damg)
    {
        currHP -= damg;
        if (currHP <= 0)
            return true;

        return false;
    }
}
