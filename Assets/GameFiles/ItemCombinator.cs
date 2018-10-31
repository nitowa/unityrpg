using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemCombinator
{

    private static Dictionary<Type, Dictionary<Type, Type>> combinations = new Dictionary<Type, Dictionary<Type, Type>>();

    static ItemCombinator()
    {
        addCombination(typeof(Comb1), typeof(Comb2), typeof(Frostmourne));
    }

    public static Item combine(Item i1, Item i2)
    {
        return (Item)Activator.CreateInstance(combinations[i1.GetType()][i2.GetType()]);

    }

    private static void addCombination(Type i1, Type i2, Type result)
    {
        Dictionary<Type,Type> dict;
        if(!combinations.TryGetValue(i1, out dict))
        {
            combinations.Add(i1, new Dictionary<Type, Type>());
        }
        combinations[i1][i2] = result;
        dict = null;

        if (!combinations.TryGetValue(i2, out dict))
        {
            combinations.Add(i2, new Dictionary<Type, Type>());
        }
        combinations[i2][i1] = result;
    }

}
