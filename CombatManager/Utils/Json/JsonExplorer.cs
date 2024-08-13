using System.IO;
using CombatManager.Utils;

namespace CombatManager.Json;

public class JsonExplorer : IExplorer
{
    public IEnumerable<DirectoryItem> ExploreDirectory(string dirName)
    {
        var items = new List<DirectoryItem>();
        var directories = Directory.GetDirectories(dirName);
        items.AddRange(directories.Select(d => new DirectoryItem { Name = d, IsDirectory = true }));
        var files = Directory.GetFiles(dirName);
        items.AddRange(files.Where(file => file.EndsWith(".json")).Select(file => new DirectoryItem { Name = file, IsDirectory = false }));
        return items;
    }
}