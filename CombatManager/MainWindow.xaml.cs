using System.Windows;
using CombatManager.Combat;
using CombatManager.Entity;
using CombatManager.Json;

namespace CombatManager;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void CreateCreatureButton_OnClick(object sender, RoutedEventArgs e)
    {
        var creationPage = new CreatureCreation(new JsonWriter<Creature>());

        NavigationFrame.Content = creationPage;
    }

    private void StartCombatButton_OnClick(object sender, RoutedEventArgs e)
    {
        var managementPage = new CombatManagement(
            new JsonReader<IEnumerable<Creature>>(),
            new JsonReader<Creature>(),
            new InitiativeTracker());

        NavigationFrame.Content = managementPage;
    }

    private void CreateCombatButton_OnClick(object sender, RoutedEventArgs e)
    {
        var creationPage = new CombatCreation(
            new JsonReader<Creature>(),
            new JsonReader<IEnumerable<Creature>>(),
            new JsonWriter<IEnumerable<Creature>>());

        NavigationFrame.Content = creationPage;
    }
}