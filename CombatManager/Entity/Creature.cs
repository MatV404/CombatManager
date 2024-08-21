using System.Resources;
using System.Windows.Controls.Primitives;

namespace CombatManager.Entity;

public class Creature
{
    public string Name { get; set; }
    public string Type { get; set; }
    public int Initiative { get; set; }
    public Statistics Stats { get; set; }
    public List<Ability> Attacks { get; set; } = [];
    public List<Ability> Features { get; set; } = [];
    public List<Ability> Other { get; set; } = [];

    // Attribute that determines whether the stats for a given creature have already been generated or whether 
    // stuff such as health needs to be rolled for.
    public bool IsGenerated { get; set; } = false;

    public void Initialize(Random generator)
    {
        Initiative = generator.Next(1, 21) + Stats.InitiativeModifier;

        if (IsGenerated)
        {
            return;
        }

        Stats.CurrentMaxHealth = generator.Next(Stats.MinGeneratedHealth, Stats.MaxGeneratedHealth + 1);
        Stats.CurrentHealth = Stats.CurrentMaxHealth;
    }

    public void ChangeHealth(int changeValue)
    {
        Stats.CurrentHealth += changeValue;
        if (Stats.CurrentHealth <= 0)
        {
            Stats.CurrentHealth = 0;
        }

        if (Stats.CurrentHealth > Stats.CurrentMaxHealth)
        {
            Stats.CurrentHealth = Stats.CurrentMaxHealth;
        }
    }

    public void Reset()
    {
        Stats.CurrentHealth = Stats.CurrentMaxHealth;
        if (IsGenerated)
        {
            return;
        }
        Initiative = 0;
    }
}