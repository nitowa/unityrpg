using System;

public abstract class Item
{
    protected string name;
    protected string description;
    private int color;

    public Item(string name, string description, int color)
	{
        this.color = color;
        this.description = description;
        this.name = name;
	}

    public string getDescription()
    {
        return description;
    }

    public string getName()
    {
        return name;
    }

    public int getColor()
    {
        return color;
    }
}
