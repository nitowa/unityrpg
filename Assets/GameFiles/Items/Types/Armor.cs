using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : EquippableItem
{
    protected int armorValue;

    public Armor(string name, string description, int color, int armorValue) : base(name, description, color)
    {
        this.armorValue = armorValue;
    }

    public int getArmorValue()
    {
        return armorValue;
    }
}
