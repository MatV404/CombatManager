using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CombatManager.Entity;
using CombatManager.Json;
using CombatManager.Utils.Enum;
using CombatManager.Utils.Paths;

namespace CombatManager;

public partial class CreatureCreation : Page
{
    private readonly IWriter<Creature> _writer;
    public CreatureCreation(IWriter<Creature> writer)
    {
        _writer = writer;
        InitializeComponent();
    }

    private void NumOnly_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = !int.TryParse(e.Text, out _);
    }

    private async void SaveCreature(object? sender, string path)
    {
        var creature = CollectCreatureData();
        var fullPath = Paths.GetFullPath(creature.Name, path);
        var succeeded = await _writer.WriteAsync(creature, fullPath);
        MessageBox.Show(!succeeded ? "Unable to save creature." : "Creature saved successfully.");
    }

    private void SaveButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Name.Text))
        {
            MessageBox.Show("Name can't be empty!");
            return;
        }

        var window = new ExplorerWindow(new JsonExplorer(), ExplorerMode.WRITE, Paths.CreatureDirName);
        window.TargetChosen += SaveCreature;
        window.ShowDialog();
    }

    private Creature CollectCreatureData()
    {
        return new Creature
        {
            Name = Name.Text,
            Type = Type.Text,
            Initiative = 0,
            IsGenerated = Generated.IsChecked ?? false,
            Stats = CollectStatData()
        };
    }

    private Statistics CollectStatData()
    {
        return new Statistics
        {
            StrengthModifier = GetIntFromTextBox(Strength),
            DexterityModifier = GetIntFromTextBox(Dexterity),
            ConstitutionModifier = GetIntFromTextBox(Constitution),
            IntelligenceModifier = GetIntFromTextBox(Intelligence),
            WisdomModifier = GetIntFromTextBox(Wisdom),
            CharismaModifier = GetIntFromTextBox(Intelligence),
            InitiativeModifier = GetIntFromTextBox(Initiative),
            PerceptionModifier = GetIntFromTextBox(Perception),
            Armor = GetIntFromTextBox(Armor),
            CurrentMaxHealth = GetIntFromTextBox(MaxHealth),
            CurrentHealth = GetIntFromTextBox(MaxHealth),
            MinGeneratedHealth = GetIntFromTextBox(MinHealthRoll),
            MaxGeneratedHealth = GetIntFromTextBox(MaxHealthRoll)
        };
    }

    private static int GetIntFromTextBox(TextBox box)
    {
        return int.TryParse(box.Text.Trim(), out var result)
            ? result
            : 0;
    }
}