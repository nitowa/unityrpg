using System;

public class Player : Unit
{
    private Inventory inventory;

    public Player(string name, int maxHP, int baseAttack, int baseDamageRange, LogController log, Inventory inv) : base(name, maxHP, baseAttack, baseDamageRange, log)
    {
        this.inventory = inv;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }

    public override void takeDamage(int value, string attackTarget)
    {
        throw new NotImplementedException();
    }
}
