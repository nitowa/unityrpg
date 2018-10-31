using System;
using System.Collections.Generic;
using UnityEngine;

public class RockyRoom : Tile
{

    private bool rocksCrashed = false;
    private bool rocksSmashed = false;

    public RockyRoom(int id, LogController log, GameController game) : base(id, log, game, "A mundane room with a bunch of rocks")
    {
        AddAction("smash");

        addTakeable(new Comb1());
        addTakeable(new Comb2());
    }

    public override void search(string[] args)
    {
        switch (args[0])
        {
            case "rock":
            case "rocks":
                if (!rocksCrashed)
                {
                    log.SlowerPrintln("The rocks comes crashing down, blocking the oblivion exit");
                    exits.Remove("oblivion");
                    rocksCrashed = true;
                    searchText = "The exit is blocked by rocks. You trapped yourself.";
                }
                else
                {
                    if (!rocksSmashed)
                    {
                        log.DelayPrintln("R o c k s.", 400);
                    }
                }
                break;
            case "debris":
                if (rocksCrashed)
                {
                    log.SlowPrintln("You behold the consequences of your actions. Good job.");
                }
                break;
            default:
                base.search(args);
                break;
        }
    }

    public void smash(string[] args)
    {
        switch (args[0])
        {
            case "rocks":
                if (rocksCrashed && !rocksSmashed)
                {
                    log.SlowPrintln("You smash the rocks with your bare fists. Wow!");
  
                    searchText = "The fearsome Nujalik blocks your path.";

                    log.Println("[COMBAT NOT YET IMPLEMENTED]");


                    log.SlowPrintln("The path is clear.");
                    exits.Add("oblivion", TileManager.GetTile(0));
                    rocksSmashed = true;
                }
                break;
            case "debris":
                if (rocksCrashed && rocksSmashed)
                {
                    log.SlowPrintln("It is already smashed, what are you doing with your life?");
                }
                break;
            default:
                log.SlowPrintln("Nothing happened.");
                break;
        }
    }

    protected override Dictionary<string, Tile> GetIntitialExits()
    {
        return new Dictionary<string, Tile>(){
            { "oblivion", TileManager.GetTile(0) }
        }; 
    }
}
