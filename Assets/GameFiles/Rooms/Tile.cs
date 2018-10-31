using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Tile {

    protected LogController log;
    protected GameController game;
    protected string searchText;
    protected Dictionary<string, Tile> exits = null;

    private Dictionary<string, Item> takeableItems = new Dictionary<string, Item>();
    private int id;
    private Dictionary<string, MethodInfo> actions = new Dictionary<string, MethodInfo>();

    public Tile(int Id, LogController log, GameController game, string searchText)
    {
        this.id = Id;
        this.log = log;
        this.game = game;
        this.searchText = searchText;
        SetDefaultActions();
        TileManager.AddTile(this);
    }

    public void enter()
    {
        if(exits == null)
           this.exits = GetIntitialExits();
        this.game.setCurrentTile(this);
    }

    private void SetDefaultActions()
    {
        AddAction("test");
        AddAction("takeable");
        
        AddAction("look");
        AddAction("search");
        AddAction("move");

        AddAction("use");
        AddAction("drop");
        AddAction("take");
        AddAction("combine");
        AddAction("equip");
        AddAction("unequip");

        AddAction("help");
    }

    public void test(string[] args)
    {

        log.Println("== Testing print types ==");

        log.Print("[Print]");
        log.Println("[Println]");
        log.SlowPrint("[SlowPrint]");
        log.SlowPrintln("[SlowPrintln]");
        log.SlowerPrint("[SlowerPrint]");
        log.SlowerPrintln("[SlowerPrintln]");
        log.DelayPrint("[DelayPrint 250ms]", 250);
        log.DelayPrintln("[DelayPrintln 250ms]", 250);

        string headline = "== Testing colored print types ==";
        int step = 0xFFFFFF / headline.Length;
        for (int i = 0; i < headline.Length; i++)
            log.PrintColored("" + headline[i], i * step);
        log.Println("");

        log.PrintColored("[PrintColored]", 0xFF00FF);
        log.PrintlnColored("[PrintlnColored]", 0xFFFF00);
        log.SlowPrintColored("[SlowPrintColored]", 0xFF00FF);
        log.SlowPrintlnColored("[SlowPrintlnColored]", 0xFFFF00);
        log.SlowerPrintColored("[SlowerPrintColored]", 0xFF00FF);
        log.SlowerPrintlnColored("[SlowerPrintlnColored]", 0xFFFF00);
        log.DelayPrintColored("[DelayPrintColored 250ms]", 0xFF00FF, 250);
        log.DelayPrintlnColored("[DelayPrintlnColored 250ms]", 0xFFFF00, 250);
    }

    protected void AddAction(string name)
    {
        actions.Add(name, this.GetType().GetMethod(name));
    }

    public void InvokeAction(string name, string[] args)
    {
        try
        {
            actions[name].Invoke(this, new object[] { args });
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
    }

    protected void addTakeable(Item item)
    {
        if (takeableItems.ContainsKey(item.ToString().ToLower()))
        {
            addTakeable(item, 1);
        }
        else
            takeableItems.Add(item.ToString().ToLower(), item);
    }

    private void addTakeable(Item item, int index)
    {
        if (takeableItems.ContainsKey(item.ToString().ToLower() + " " + index))
        {
            addTakeable(item, index + 1);
        }
        else
            takeableItems.Add(item.ToString().ToLower() + " " + index, item);
    }

    protected void removeTakeable(string name)
    {
        takeableItems.Remove(name);
    }

    public virtual void look(string[] args)
    {
        string combined = string.Join(" ", args);

        switch (combined)
        {
            case "inventory":
                game.GetPlayer().GetInventory().listItems();
                break;
            case "stats":
                game.GetPlayer().reportStats();
                break;
            case "exits":
                log.PrintlnColored(getExitTexts(), UIColors.DIRECTIONS);
                break;
            case "":
                log.SlowPrintln(searchText);
                break;
            default:
                /* check if it was an item */
                Item i = game.GetPlayer().GetInventory().getItemByIndexOrName(combined);
                if (i != null)
                {
                    log.ItemPrintln(i);
                    log.Println(i.getDescription());
                }
                break;
        }
    }

    private string getExitTexts()
    {
        string directions = "";
        foreach (string s in exits.Keys)
            directions += "[" + s + "] ";

        return "Available exits: " + directions;
    }

    public void combine(string[] args)
    {
        string joined = string.Join(" ", args);
        if (!joined.Contains("with"))
        {
            log.Println("A combination must contain the word \"with\".");
            log.Println("e.g. \"combine [item 1] with [item 2]\"");
            return;
        }
        string[] items = joined.Split(" with ".ToCharArray(), 2, System.StringSplitOptions.RemoveEmptyEntries);
        game.GetPlayer().GetInventory().combine(items[0], items[1]);
    }


    public virtual void use(string[] args)
    {

    }

    public virtual void drop(string[] args)
    {
        if (args.Length == 0)
            return;

        string what = string.Join(" ", args);

        Item item = game.GetPlayer().GetInventory().getItemByIndexOrName(what);

        if (item != null)
        {
            log.Print("You dropped ");
            //JukeBox.playMP3(JukeBox.DROP);
            log.ItemPrintln(item);
            addTakeable(item);
            game.GetPlayer().GetInventory().silentRemove(item);
        }
    }


    public virtual void search(string[] args)
    {
        log.SlowPrintln("You find nothing.");
    }

    public virtual void move(string[] args)
    {
        string where = args[0];
        Tile next = exits[where];
        Debug.Log(this.GetType().Name + " -[" + where + "]-> " + next.GetType().Name);

        if (exits.ContainsKey(where))
        {
            //JukeBox.playMP3(JukeBox.WALK);
            next.enter();
        }
    }

    public virtual void help(string[] args)
    {
        log.Println("\n+-----------------------+"
                  + "\n|        Commands       |"
                  + "\n+-----------------------+"
                  + "\n|"
                  + "\n+------   General"
                  + "\n|"
                  + "\n|  look"
                  + "\n|  look    [ exits | stats ]"
                  + "\n|  move    [ direction ]"
                  + "\n|  search  [ target ]"
                  + "\n|  take    [ target ]"
                  + "\n|  jump    [ target ]"
                  + "\n|  duck    [ target ]"
                  + "\n|  takeable"
                  + "\n|"
                  + "\n+------   Inventory"
                  + "\n|"
                  + "\n|  look    [ inventory | item | id ]"
                  + "\n|  unequip [ item | slot ]"
                  + "\n|  equip   [ item | id ]"
                  + "\n|  drop    [ item | id ]"
                  + "\n|  combine [ item | id ] with [ item | id ]");
    }

    public virtual void take(string[] args)
    {
        if (args.Length == 0)
            return;

        string what = args[0];

        if (!takeableItems.ContainsKey(args[0]))
        {
            log.SlowPrintln("Could not find any " + what);
            return;
        }
        Item item = takeableItems[what];

        game.GetPlayer().GetInventory().Add(item, 
            () => { removeTakeable(what); },
            () => { log.SlowPrintln("Your inventory is full"); }
        );
    }


    public virtual void equip(string[] args)
    {
        if (args.Length == 0)
            return;

        string what = args[0];

        Item item = game.GetPlayer().GetInventory().getItemByIndexOrName(what);

        if (item is EquippableItem) {
            game.GetPlayer().GetInventory().equip((EquippableItem)item);
            game.GetJukebox().playSound(JukeboxController.EQUIP);
        }
        else if (item == null)
        {
            log.SlowPrintln("Unknown item");
        }
        else
        {
            log.SlowPrintln("Could not equip " + item);
        }
    }

    public virtual void unequip(string[] args)
    {
        if (args.Length == 0)
            return;

        game.GetPlayer().GetInventory().unequip(string.Join(" ", args));
    }

    public void takeable(string[] args)
    {
        log.SlowPrintln("Takeable items: ");
        

        foreach (KeyValuePair<string, Item> e in takeableItems)
        {
            log.PrintlnColored(e.Key, e.Value.getColor());
        }
    }

    public int GetId()
    {
        return this.id;
    }

    protected abstract Dictionary<string, Tile> GetIntitialExits();

}
