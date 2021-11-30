using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string unitName;

    public int lvl;
    public int damage;
    public int agility; private int speed;
    public int protection; public bool isDefending;
    public int specialLoad; public bool hasSpecial;

    public int maxHP;
    public int currHP;

    public bool isDead = false;
    public bool isHero = false;

    public bool TakeDamage(int damg)
    {
        if (isDefending) this.RemoveDefense();

        if (damg < 0)
            return false;

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

    public void SetDefense()
    {
        isDefending = true;
        protection += 10;
    }

    public void RemoveDefense()
    {
        isDefending = false;
        protection -= 10;
    }
}
