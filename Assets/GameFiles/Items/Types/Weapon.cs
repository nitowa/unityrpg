using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : EquippableItem
{
    protected int attackValue;

    public Weapon(string name, string description, int color, int attackValue) : base(name, description, color)
    {
        this.attackValue = attackValue;
    }

    public int getAttack()
    {
        return this.attackValue;
    }
}
