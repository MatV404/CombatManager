using System.Windows;
using System.Windows.Controls;
using CombatManager.Entity;

namespace CombatManager;

public partial class CreatureInfo : Page
{
    public CreatureInfo(CreatureInstance instance)
    {
        InitializeComponent();
        CreatureName.Text = instance.Label;
        HP.Text = instance.Data.Stats.CurrentHealth.ToString();
        MaxHP.Text = instance.Data.Stats.CurrentMaxHealth.ToString();
        Initiative.Text = instance.Data.Initiative.ToString();
        Armor.Text = instance.Data.Stats.Armor.ToString();
        Type.Text = instance.Data.Type;
        Perception.Text = instance.Data.Stats.PerceptionModifier.ToString();
        Strength.Text = instance.Data.Stats.StrengthModifier.ToString();
        Dexterity.Text = instance.Data.Stats.DexterityModifier.ToString();
        Constitution.Text = instance.Data.Stats.ConstitutionModifier.ToString();
        Intelligence.Text = instance.Data.Stats.IntelligenceModifier.ToString();
        Wisdom.Text = instance.Data.Stats.WisdomModifier.ToString();
        Charisma.Text = instance.Data.Stats.CharismaModifier.ToString();
        FillAbilities(instance.Data.Attacks, AttackPanel);
        FillAbilities(instance.Data.Features, FeaturePanel);
        FillAbilities(instance.Data.Other, OtherPanel);
    }

    private void FillAbilities(IEnumerable<Ability> abilities, StackPanel panel)
    {
        foreach (var ability in abilities)
        {
            var stack = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Width = 500
            };

            var name = new TextBlock
            {
                FontSize = 13,
                FontWeight = FontWeights.Bold,
                Text = $"{ability.Name}.",
                TextWrapping = TextWrapping.Wrap,
                Width = 100,
                Margin = new Thickness(0, 0, 5, 0)
            };

            var description = new TextBlock
            {
                FontSize = 12,
                Text = ability.Description,
                Width = 350,
                TextWrapping = TextWrapping.Wrap
            };

            stack.Children.Add(name);
            stack.Children.Add(description);
            panel.Children.Add(stack);
        }
    }
}