using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CombatManager.Combat;
using CombatManager.Entity;
using CombatManager.Json;
using CombatManager.Utils.Enum;
using CombatManager.Utils.Paths;

namespace CombatManager;

public partial class CombatManagement : Page
{
    private IReader<IEnumerable<Creature>> _combatReader;
    private IReader<Creature> _creatureReader;
    private Random? _generator;
    private InitiativeTracker _initiative;

    private List<CreatureInstance> _creatures;
    public CombatManagement(IReader<IEnumerable<Creature>> combatReader, 
                            IReader<Creature> creatureReader, InitiativeTracker tracker)
    {
        _combatReader = combatReader;
        _creatureReader = creatureReader;
        _creatures = [];
        _initiative = tracker;
        InitializeComponent();
    }

    private void CloseButton_OnClick(object sender, RoutedEventArgs e)
    {
        NavigationService?.Navigate(null);
    }

    private void RemoveCreature(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            MessageBox.Show("Unable to remove creature.");
            return;
        }

        var tag = button.Tag;
        if (tag is not int index)
        {
            MessageBox.Show("Unable to remove creature.");
            return;
        }
        _creatures.RemoveAt(index);
        RefreshCreatures();
    }

    private void PreviewCreature(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
        {
            MessageBox.Show("Unable to preview creature.");
            return;
        }

        var tag = button.Tag;
        if (tag is not int index)
        {
            MessageBox.Show("Unable to preview creature.");
            return;
        }

        var creature = _creatures[index];
        var creatureInfoPage = new CreatureInfo(creature);
        CreatureInfoFrame.Content = creatureInfoPage;
    }
    
    // ToDo: Create an event instead that reacts only by touching the affected creature entry.
    private void RefreshCreatures()
    {
        var idx = 0;
        ActiveCreaturePanel.Children.Clear();
        foreach (var creature in _creatures)
        {
            creature.Id = idx;
            CreateCreatureEntry(creature);
            idx++;
        }
        UpdateTurnMarkers();
    }
    
    private void CreateCreatureEntry(CreatureInstance creature)
    {
        object idxTag = creature.Id;
        var border = new Border
        {
            Tag = idxTag,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1.0),
            Margin = new Thickness(5.0),
            Width = 160,
            HorizontalAlignment = HorizontalAlignment.Left
        };

        var panel = new StackPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(5),
        };
        border.Child = panel;

        var button = new Button
        {
            Tag = idxTag,
            Content = "X",
            Width = 20,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        button.Click += RemoveCreature;
        panel.Children.Add(button);

        var previewButton = new Button
        {
            Tag = idxTag,
            Content = "\ud83d\udc41\ufe0f",
            Width = 20,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        previewButton.Click += PreviewCreature;
        panel.Children.Add(previewButton);
        
        var text = new TextBlock
        {
            Text = $"{creature.Label}",
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            TextWrapping = TextWrapping.NoWrap,
            Margin = new Thickness(5, 0, 5, 0)
        };
        panel.Children.Add(text);

        ActiveCreaturePanel.Children.Add(border);
    }
    
    // ToDo: A lot of code duplication with CombatCreation. Consider deduplication.
    private async void LoadCombat(object? sender, string path)
    {
        _generator = new Random(path.Length);
        var result = await _combatReader.ReadAsync(path);
        if (result is null)
        {
            MessageBox.Show("Unable to read combat from selected file.");
            return;
        }
        _creatures.Clear();
        var idx = 0;
        _creatures.AddRange(result.Select(c =>
        {
            var instance = new CreatureInstance(
                $"{idx}. {c.Name}", idx, c);
            idx++;
            return instance;
        }));
        RefreshCreatures();
    }
    
    private async void LoadCombatButton_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ExplorerWindow(new JsonExplorer(), ExplorerMode.READ, Paths.CombatDirName);
        window.TargetChosen += LoadCombat;
        window.ShowDialog();
    }

    private void StartCombatButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (_generator is null)
        {
            //ToDo: Open new window with a start value for the generator.
            MessageBox.Show("Generator is null!");
            return;
        }
        foreach (var item in _creatures.Where(item => !item.Data.IsGenerated))
        {
            item.Data.Initialize(_generator);
        }
        
        _initiative.Initialize();
        _creatures = _creatures.OrderByDescending(i => i.Data.Initiative).ToList();
        RefreshCreatures();
    }

    private void UpdateTurnMarkers()
    {
        if (!_initiative.IsActive)
        {
            return;
        }
        var previousCreature = _initiative.PeekPrevValue(_creatures.Count);
        var nextCreature = _initiative.PeekNextValue(_creatures.Count);
        PreviousTurn.Text = _creatures[previousCreature].Label;
        CurrentTurn.Text = _creatures[_initiative.Current].Label;
        NextTurn.Text = _creatures[nextCreature].Label;
    }
    
    private void NextTurnButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_initiative.IsActive)
        {
            MessageBox.Show("Combat has not started yet.");
            return;
        }
        _initiative.Increment(_creatures.Count);
        UpdateTurnMarkers();
    }

    private void PreviousTurnButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (!_initiative.IsActive)
        {
            MessageBox.Show("Combat has not started yet.");
            return;
        }
        _initiative.Decrement(_creatures.Count);
        UpdateTurnMarkers();
    }

    private void AddQuickCreature(object? sender, Creature creature)
    {
        var creatureInstance = new CreatureInstance(
            $"{_creatures.Count}. {creature.Name}",
            _creatures.Count, creature);
        _creatures.Add(creatureInstance);
        _creatures = _creatures.OrderByDescending(i => i.Data.Initiative).ToList();
        RefreshCreatures();
    }
    
    private void AddCreatureButton_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new QuickCreatureAdd();
        window.QuickCreatureAdded += AddQuickCreature;
        window.ShowDialog();
    }

    private async void LoadCreature(object? sender, string path)
    {
        Creature? creature;
        try
        {
            creature = await _creatureReader.ReadAsync(path);
        }
        catch (Exception)
        {
            creature = null;
        }

        if (creature is null)
        {
            MessageBox.Show("Unable to load chosen creature.");
            return;
        }

        if (!creature.IsGenerated && _initiative.IsActive)
        {
            if (_generator is null)
            {
                MessageBox.Show("Unable to determine initiative for creature. Try restarting combat and loading again.");
                return;
            }
            creature.Initialize(_generator);
        }
        var instance = new CreatureInstance($"{_creatures.Count}. {creature.Name}", 
            _creatures.Count, creature);
        _creatures.Add(instance);
        _creatures = _creatures.OrderByDescending(i => i.Data.Initiative).ToList();
        RefreshCreatures();
    }

    private void LoadCreatureButton_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ExplorerWindow(new JsonExplorer(), ExplorerMode.READ, Paths.CreatureDirName);
        window.TargetChosen += LoadCreature;
        window.ShowDialog();
    }

    private void ResetCombatButton_OnClick(object sender, RoutedEventArgs e)
    {
        foreach (var creature in _creatures)
        {
            creature.Data.Reset();
        }
    }
}