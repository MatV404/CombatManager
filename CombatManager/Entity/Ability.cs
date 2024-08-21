namespace CombatManager.Entity;

public class Ability
{
    public string Name { get; set; }
    public string Description { get; set; }
    public AbilityType Type { get; set; }

    public Ability(string name, string description, AbilityType type)
    {
        Name = name;
        Description = description;
        Type = type;
    }
}