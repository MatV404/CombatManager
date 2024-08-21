using System.Windows;
using CombatManager.Entity;

namespace CombatManager;

public partial class AbilityCreation : Window
{
    public EventHandler<Ability>? AbilityAdded;
    
    public AbilityCreation()
    {
        InitializeComponent();
    }

    private void AddAbilityButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(AbilityName.Text)
            || string.IsNullOrEmpty(Description.Text))
        {
            MessageBox.Show("Fill out the Ability Name and Description.");
            return;
        }

        var ability = new Ability(AbilityName.Text, Description.Text, ExtractAbilityType());
        AbilityAdded?.Invoke(this, ability);
        Close();
    }

    private AbilityType ExtractAbilityType()
    {
        return Type.Text switch
        {
            "Attack" => AbilityType.ATTACK,
            "Feature" => AbilityType.FEATURE,
            _ => AbilityType.OTHER
        };
    }
}