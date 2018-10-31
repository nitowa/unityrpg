using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileManager{

    private static Dictionary<int, Tile> tiles = new Dictionary<int, Tile>();


    public static void AddTile(Tile t)
    {
        tiles.Add(t.GetId(), t);
    }

    public static Tile GetTile(int id)
    {
        return tiles[id];
    }



}
