using System;

public abstract class Unit
{
    protected string name;
    protected int maxHP;
    protected int currHP;
    protected LogController log;
    protected int baseAttack;
    protected int baseDamageRange;

    public Unit(string name, int maxHP, int baseAttack, int baseDamageRange, LogController log)
    {
        this.name = name;
        this.maxHP = maxHP;
        this.currHP = maxHP;
        this.baseAttack = baseAttack;
        this.baseDamageRange = baseDamageRange;
        this.log = log;
    }

    public abstract void takeDamage(int value, string attackTarget);

    public void restoreHP(int amount)
    {
        currHP += amount;
        if (currHP > maxHP)
            currHP = maxHP;
    }

    public void reportStats()
    {

        int colorPick = (int)(((double)currHP) / ((double)maxHP) * 10) - 1;

        log.PrintlnColored("HP: " + currHP + "/" + maxHP, UIColors.HEALTH_GRADIENT[colorPick]);
        if (this is Player)
        {
            Player _this = ((Player)this);
            log.Println("Weapon: " + _this.GetInventory().getAttack() + " Attack");
            log.Println("Armor: " +  _this.GetInventory().getTotalArmor()+" Defense");
        }
            
    }
}
