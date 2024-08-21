using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CombatManager.Entity;
using CombatManager.Json;
using CombatManager.Utils.Enum;
using CombatManager.Utils.Paths;
using CombatManager.Utils.Validation;

namespace CombatManager;

public partial class CreatureCreation : Page
{
    private readonly IWriter<Creature> _writer;
    private readonly List<Ability> _abilities;
    public CreatureCreation(IWriter<Creature> writer)
    {
        _writer = writer;
        _abilities = [];
        InitializeComponent();
    }

    private void NumOnly_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var input = sender as TextBox;
        
        e.Handled = !InputValidator.ValidateNumOnlyInput(e.Text,input!.Text);
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
        var creature = new Creature
        {
            Name = Name.Text,
            Type = Type.Text,
            Initiative = 0,
            IsGenerated = Generated.IsChecked ?? false,
            Stats = CollectStatData()
        };

        foreach (var ability in _abilities)
        {
            switch (ability.Type)
            {
                case AbilityType.ATTACK:
                    creature.Attacks.Add(ability);
                    break;
                case AbilityType.FEATURE:
                    creature.Features.Add(ability);
                    break;
                case AbilityType.OTHER:
                default:
                    creature.Other.Add(ability);
                    break;
            }
        }

        return creature;
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
    
    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        var index = (int)button.Tag;
        
        _abilities.RemoveAt(index);
        AbilitiesPanel.Children.Clear();
        foreach (var ability in _abilities)
        {
            CreateAbilityEntry(ability);
        }
    }

    private void CreateAbilityEntry(Ability ability)
    {
        var panel = new StackPanel
        {
            Name = ability.Name,
            Orientation = Orientation.Horizontal
        };

        var text = new TextBlock
        {
            Text = $"{ability.Name}. {ability.Description}"
        };

        object tag = _abilities.Count - 1;
        
        var button = new Button
        {
            Content = "Delete",
            Tag = tag
        };
        button.Click += DeleteButton_OnClick;

        panel.Children.Add(text);
        panel.Children.Add(button);
        AbilitiesPanel.Children.Add(panel);
    }

    private void AddAbility(object? sender, Ability ability)
    {
        _abilities.Add(ability);
        CreateAbilityEntry(ability);
    }
    
    private void AddAbilityButton_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new AbilityCreation();
        window.AbilityAdded += AddAbility;
        window.ShowDialog();
    }
}