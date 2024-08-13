using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CombatManager.Entity;
using CombatManager.Json;
using CombatManager.Utils.Enum;
using CombatManager.Utils.Paths;

namespace CombatManager;

public partial class CombatCreation : Page
{
    private List<Creature> _creatures;
    private IReader<Creature> _creatureReader;
    private IReader<IEnumerable<Creature>> _combatReader;
    private IWriter<IEnumerable<Creature>> _combatWriter;

    public CombatCreation(IReader<Creature> creatureReader,
                          IReader<IEnumerable<Creature>> combatReader,
                          IWriter<IEnumerable<Creature>> combatWriter)
    {
        _creatureReader = creatureReader;
        _combatReader = combatReader;
        _combatWriter = combatWriter;
        _creatures = [];
        InitializeComponent();
    }

    private void AddCreatureButton_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ExplorerWindow(new JsonExplorer(), ExplorerMode.READ, Paths.CreatureDirName);
        window.TargetChosen += AddCreature;
        window.ShowDialog();
    }

    private async void AddCreature(object? sender, string path)
    {
        var result = await _creatureReader.ReadAsync(path);
        if (result is null)
        {
            MessageBox.Show("Unable to read creature from selected file.");
            return;
        }
        _creatures.Add(result);
        RefreshCreatures();
    }

    // ToDo: Create an event instead that reacts only by touching the affected creature entry.
    private void RefreshCreatures()
    {
        var idx = 0;
        CreaturesPanel.Children.Clear();
        foreach (var creature in _creatures)
        {
            CreateCreatureEntry(idx, creature);
            idx++;
        }
    }

    private void RemoveCreature(object sender, RoutedEventArgs e)
    {
        var btn = sender as Button;
        if (btn?.Tag is null)
        {
            return;
        }
        var idx = (int)btn.Tag;

        _creatures.RemoveAt(idx);
        RefreshCreatures();
    }

    private void CreateCreatureEntry(int idx, Creature creature)
    {
        object idxTag = idx;
        var border = new Border
        {
            Tag = idxTag,
            BorderBrush = Brushes.Black,
            BorderThickness = new Thickness(1.0),
            Margin = new Thickness(5.0),
            HorizontalAlignment = HorizontalAlignment.Center
        };

        var panel = new StackPanel
        {
            FlowDirection = FlowDirection.LeftToRight,
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(5),
        };
        border.Child = panel;

        var text = new TextBlock
        {
            Text = creature.Name,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Left,
            Margin = new Thickness(5, 0, 5, 0)
        };
        panel.Children.Add(text);

        var filler = new TextBlock
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Width = 150
        };
        panel.Children.Add(filler);

        var button = new Button
        {
            Tag = idxTag,
            Content = "Delete",
            Width = 80,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        button.Click += RemoveCreature;
        panel.Children.Add(button);

        CreaturesPanel.Children.Add(border);
    }

    private async void SaveCombat(object? sender, string path)
    {
        var combatName = CombatName.Text;
        if (string.IsNullOrEmpty(combatName))
        {
            MessageBox.Show("No combat name provided!");
            return;
        }

        var fullPath = Paths.GetFullPath(combatName, path);
        var succeeded = await _combatWriter.WriteAsync(_creatures, fullPath);
        MessageBox.Show(succeeded ? "Combat saved!" : "Failed to save combat.");
    }

    private void SaveCombatButton_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ExplorerWindow(new JsonExplorer(), ExplorerMode.WRITE, Paths.CombatDirName);
        window.TargetChosen += SaveCombat;
        window.ShowDialog();
    }

    private async void LoadCombat(object? sender, string path)
    {
        var result = await _combatReader.ReadAsync(path);
        if (result is null)
        {
            MessageBox.Show("Unable to read combat from selected file.");
            return;
        }
        _creatures.Clear();
        _creatures.AddRange(result);
        RefreshCreatures();
    }

    private void LoadCombatButton_OnClick(object sender, RoutedEventArgs e)
    {
        var window = new ExplorerWindow(new JsonExplorer(), ExplorerMode.READ, Paths.CombatDirName);
        window.TargetChosen += LoadCombat;
        window.ShowDialog();
    }
}