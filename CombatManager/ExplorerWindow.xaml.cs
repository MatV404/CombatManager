using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using CombatManager.Json;
using CombatManager.Utils.Enum;
using CombatManager.Utils.Paths;

namespace CombatManager;

public partial class ExplorerWindow : Window
{
    private readonly IExplorer _explorer;
    private readonly ExplorerMode _mode;
    private string _currentDirectory;

    private const string DirIcon = "\ud83d\udcc1";
    private const string FileIcon = "\ud83d\uddb9";

    public event EventHandler<string>? TargetChosen;
    public ExplorerWindow(IExplorer explorer, ExplorerMode mode, string directoryName)
    {
        _explorer = explorer;
        _mode = mode;
        InitializeComponent();
        if (mode == ExplorerMode.READ)
        {
            SaveButton.Visibility = Visibility.Collapsed;
        }
        _currentDirectory = Paths.GetStoragePath(directoryName);
        OpenDirectory(_currentDirectory);
    }

    private void OnTargetChosen(string path)
    {
        TargetChosen?.Invoke(this, path);
    }

    private void ChooseFileToLoad(object sender, RoutedEventArgs e)
    {
        var filePath = RetrievePath(sender);

        if (filePath is null)
        {
            return;
        }
        
        OnTargetChosen(filePath);
        Close();
    }

    private void ChoseSaveDirectory(object sender, RoutedEventArgs e)
    {
        OnTargetChosen(_currentDirectory);
        Close();
    }

    private void LoadDirectory(object sender, RoutedEventArgs e)
    {
        var dirPath = RetrievePath(sender);

        if (dirPath is null)
        {
            return;
        }

        OpenDirectory(dirPath);
    }

    private static string? RetrievePath(object sender)
    {
        if (sender is not Button button)
        {
            MessageBox.Show("Unable to retrieve file name.");
            return null;
        }

        var tag = button.Tag;
        if (tag is string filePath) return filePath;
        MessageBox.Show("Unable to retrieve file name.");
        return null;

    }
    
    private void OpenDirectory(string dirName)
    {
        _currentDirectory = dirName;
        CurrentPath.Text = _currentDirectory;
        var contents = _explorer.ExploreDirectory(dirName);
        ContentsPanel.Children.Clear();
        foreach (var item in contents)
        {
            object tag = item.Name;
            var itemName = Path.GetFileName(item.Name);
            Button button = new()
            {
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                HorizontalAlignment = HorizontalAlignment.Left,
                Cursor = Cursors.Hand,
                Tag = tag
            };

            switch (item.IsDirectory)
            {
                case false when _mode == ExplorerMode.READ:
                    button.Click += ChooseFileToLoad;
                    break;
                case true:
                    button.Click += LoadDirectory;
                    break;
            }

            TextBlock text = new()
            {
                Text = item.IsDirectory
                    ? $"{DirIcon} {itemName}"
                    : $"{FileIcon} {itemName}",
                TextDecorations = TextDecorations.Underline,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 16
            };

            button.Content = text;
            ContentsPanel.Children.Add(button);
        }
    }

    private void PrevDirectoryButton_OnClick(object sender, RoutedEventArgs e)
    {
        DirectoryInfo? results;
        try
        {
            results = Directory.GetParent(_currentDirectory);
        }
        catch (Exception)
        {
            results = null;
        }

        if (results is null)
        {
            MessageBox.Show("No parent directory found.");
            return;
        }
        OpenDirectory(results.FullName);
    }
}