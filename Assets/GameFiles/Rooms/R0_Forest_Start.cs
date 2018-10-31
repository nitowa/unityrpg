using System;
using System.Collections.Generic;

public class R0_Forest_Start : Tile
{
    public R0_Forest_Start(int id, LogController log, GameController game) : base(id, log, game, "You take a look around the forest. On the ground you see some rocks, and a couple of rotten branches.")
    {


    }

    protected override Dictionary<string, Tile> GetIntitialExits()
    {
        return new Dictionary<string, Tile>()
        {
            { "rocky", TileManager.GetTile(-1) }
        };
    }
}
