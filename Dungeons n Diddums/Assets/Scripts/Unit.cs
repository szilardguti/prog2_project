using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int lvl;
    public int damage;
    public int agility;
    private int speed;

    public int maxHP;
    public int currHP;

    public bool isDead = false;
    public bool isHero = false;

    public bool TakeDamage(int damg)
    {
        currHP -= damg;
        if (currHP <= 0)
        {
            isDead = true;
            return true;
        }

        return false;
    }

    public void SetSpeed()
    {
        speed = agility * 10;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public void TookTurn()
    {
        speed -= 20;
    }

    public void ReturnTurn()
    {
        speed += 20;
    }
}
