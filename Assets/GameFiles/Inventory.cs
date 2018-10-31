using System;
using System.Collections;
using System.Collections.Generic;

public class Inventory
{
    private LogController log;
    private int size;
    private Weapon weapon;
    private HelmArmor helmet;
    private ChestArmor chest;
    private LegArmor legs;
    private BootsArmor boots;
    private GlovesArmor gloves;
    private CloakArmor cloak;
    private ArrayList bag;

	public Inventory(int size, LogController log)
	{
        this.size = size;
        this.log = log;
        this.bag = new ArrayList();
	}

    public void Add(Item item)
    {
        Add(item, () => { }, () => { });
    }


    public void Add(Item item, Action onBagFull)
    {
        Add(item, () => { }, onBagFull);
    }

    public void Add(Item item, Action success, Action onBagFull)
    {
        if (bag.Count == size)
        {
            onBagFull();
            return;
        }

        log.Print("You received ");
        log.ItemPrintln(item);
        bag.Add(item);
        success();
    }

    public int getTotalArmor()
    {
        int armorsum = 0;
        if (helmet != null)
            armorsum += helmet.getArmorValue();
        if (chest != null)
            armorsum += chest.getArmorValue();
        if (legs != null)
            armorsum += legs.getArmorValue();
        if (boots != null)
            armorsum += boots.getArmorValue();
        if (cloak != null)
            armorsum += cloak.getArmorValue();
        if (gloves != null)
            armorsum += gloves.getArmorValue();

        return armorsum;
    }

    public int getAttack()
    {
        if (weapon == null)
            return 0;
        return weapon.getAttack();
    }

    public void combine(String identifier1, String identifier2)
    {
        Item i1 = getItemByIndexOrName(identifier1);
        Item i2 = getItemByIndexOrName(identifier2);

        Item combined = ItemCombinator.combine(i1, i2);

        if(i1 == null || i2 == null || combined == null)
        {
            log.Println("Nothing happened.");
            return;
        }

        silentRemove(i1);
        silentRemove(i2);
        Add(combined);
    }

    public Item getItemByIndexOrName(String identifier)
    {
        try
        {
            int id = int.Parse(identifier);
            return getItem(id);
        }
        catch
        {
            return getItem(identifier);
        }
    }

    private Item getItem(int id)
    {
        if(id > size)
        {
            return null;
        }
        
        return (Item) bag[id-1];
    }

    private Item getItem(String name)
    {
        foreach(Item i in bag)
        {
            if (i.getName().ToLower().Equals(name.ToLower()))
                return i;
        }
        return null;
    }

    public void listItems()
    {
        log.Println("\n+------ EQUIPMENT ------+");
        if (weapon != null)
        {
            log.Print("|  Weapon: ");
            log.ItemPrintln(weapon);
        }
        if (helmet != null)
        {
            log.Print("|  Helmet: ");
            log.ItemPrintln(helmet);
        }
        if (cloak != null)
        {
            log.Print("|  Cloak: ");
            log.ItemPrintln(cloak);
        }
        if (chest != null)
        {
            log.Print("|  Chest: ");
            log.ItemPrintln(chest);
        }
        if (gloves != null)
        {
            log.Print("|  Gloves: ");
            log.ItemPrintln(gloves);
        }
        if (legs != null)
        {
            log.Print("|  Legs: ");
            log.ItemPrintln(legs);
        }
        if (boots != null)
        {
            log.Print("|  Boots: ");
            log.ItemPrintln(boots);
        }
        log.Println("+--------  BAG  --------+");

        if (bag.Count == 0)
        {
            log.Println("|  (empty)");
        }
        else
        {
            for (int i = 0; i < bag.Count; i++)
            {
                log.Print("|  " + (i + 1) + ") ");
                log.ItemPrintln((Item)bag[i]);
            }
        }
        log.Println("+-----------------------+");
    }

    public void equip(EquippableItem item)
    {
        if (!bag.Contains(item))
            return;

        EquippableItem tmp = null;

        if(item is HelmArmor)
        {
            if (helmet != null)
                tmp = helmet;
            helmet = (HelmArmor) item;
        }

        if (item is CloakArmor)
        {
            if (cloak != null)
                tmp = cloak;
            cloak = (CloakArmor)item;
        }

        if (item is ChestArmor)
        {
            if (chest != null)
                tmp = chest;
            chest = (ChestArmor)item;
        }

        if (item is GlovesArmor)
        {
            if (gloves != null)
                tmp = gloves;
            gloves = (GlovesArmor)item;
        }

        if (item is LegArmor)
        {
            if (legs != null)
                tmp = legs;
            legs = (LegArmor)item;
        }

        if (item is BootsArmor)
        {
            if (boots != null)
                tmp = boots;
            boots = (BootsArmor)item;
        }

        if (item is Weapon)
        {
            if (weapon != null)
                tmp = weapon;
            weapon = (Weapon)item;
        }

        bag.Remove(item);
        if (tmp != null)
            bag.Add(tmp);

        log.Print("Equipped ");
        log.ItemPrintln(item);
    }

    public void unequip(String slot)
    {
        if (bag.Count == size)
        {
            log.Println("The inventory is full. Cannot unequip.");
            return;
        }

        EquippableItem item = null;

        switch (slot)
        {

            case "boots":
                item = this.boots;
                this.boots = null;
                break;

            case "legs":
                item = this.legs;
                this.legs = null;
                break;

            case "gloves":
                item = this.gloves;
                this.gloves = null;
                break;


            case "chest":
                item = this.chest;
                this.chest = null;
                break;

            case "cloak":
                item = this.cloak;
                this.cloak = null;
                break;

            case "helm":
                item = this.helmet;
                this.helmet = null;
                break;

            case "weapon":
                item = this.weapon;
                this.weapon = null;
                break;
            default:
                //check if maybe the names match
                if (this.weapon != null && this.weapon.ToString().ToLower().Equals(slot.ToLower()))
                {
                    item = this.weapon;
                    this.weapon = null;
                }
                else if (this.helmet != null && this.helmet.ToString().ToLower().Equals(slot.ToLower()))
                {
                    item = this.helmet;
                    this.helmet = null;
                }
                else if (this.cloak != null && this.cloak.ToString().ToLower().Equals(slot.ToLower()))
                {
                    item = this.cloak;
                    this.cloak = null;
                }
                else if (this.legs != null && this.legs.ToString().ToLower().Equals(slot.ToLower()))
                {
                    item = this.legs;
                    this.legs = null;
                }
                else if (this.boots != null && this.boots.ToString().ToLower().Equals(slot.ToLower()))
                {
                    item = this.boots;
                    this.boots = null;
                }
                else if (this.gloves != null && this.gloves.ToString().ToLower().Equals(slot.ToLower()))
                {
                    item = this.gloves;
                    this.gloves = null;
                }
                else if (this.chest != null && this.chest.ToString().ToLower().Equals(slot.ToLower()))
                {
                    item = this.chest;
                    this.chest = null;
                }
                else
                {
                    return;
                }
                break;
        }

        Add(item, ()=> { }, () => { /*bag cannot be full here*/ });
    }

    public void silentRemove(Item item)
    {
        bag.Remove(item);
    }

    public Weapon getWeapon()
    {
        return weapon;
    }

    public HelmArmor getHelmet()
    {
        return helmet;
    }

    public ChestArmor getChest()
    {
        return chest;
    }

    public LegArmor getLegs()
    {
        return legs;
    }

    public CloakArmor getCloak()
    {
        return cloak;
    }

    public GlovesArmor getGloves()
    {
        return gloves;
    }

    public BootsArmor getBoots()
    {
        return boots;
    }

}
