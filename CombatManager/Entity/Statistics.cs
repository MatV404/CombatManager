namespace CombatManager.Entity;

public class Statistics
{
    public int StrengthModifier { get; set; } = 0;
    public int DexterityModifier { get; set; } = 0;
    public int ConstitutionModifier { get; set; } = 0;
    public int IntelligenceModifier { get; set; } = 0;
    public int WisdomModifier { get; set; } = 0;
    public int CharismaModifier { get; set; } = 0;

    public int InitiativeModifier { get; set; } = 0;
    public int PerceptionModifier { get; set; } = 0;

    public int Armor { get; set; } = 10;
    public int CurrentMaxHealth { get; set; } = 0;
    public int CurrentHealth { get; set; } = 0;

    public int MinGeneratedHealth { get; set; } = 0;
    public int MaxGeneratedHealth { get; set; } = 0;
}