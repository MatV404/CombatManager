namespace CombatManager.Entity;

public class CreatureInstance
{
    public string Label { get; set; }
    public int Id { get; set; }
    public Creature Data { get; private set; }

    public CreatureInstance(string label, int id, Creature data)
    {
        Label = label;
        Id = id;
        Data = data;
    }
}