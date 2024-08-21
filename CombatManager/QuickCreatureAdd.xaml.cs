using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CombatManager.Entity;
using CombatManager.Utils.Validation;

namespace CombatManager;

public partial class QuickCreatureAdd : Window
{
    public event EventHandler<Creature>? QuickCreatureAdded; 
    
    public QuickCreatureAdd()
    {
        InitializeComponent();
    }
    
    private void NumOnly_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var input = sender as TextBox;
        
        e.Handled = !InputValidator.ValidateNumOnlyInput(e.Text,input!.Text);
    }

    private void AddCreatureButton_OnClick(object sender, RoutedEventArgs e)
    {
        Creature creature = new()
        {
            Name = Name.Text,
            Initiative = int.TryParse(Initiative.Text.Trim(), out var result)
                ? result
                : 0,
            Stats = new Statistics(),
            IsGenerated = true
        };

        QuickCreatureAdded?.Invoke(this, creature);
    }
}