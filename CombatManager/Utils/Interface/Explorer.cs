using CombatManager.Utils;

namespace CombatManager.Json;

public interface IExplorer
{
    public IEnumerable<DirectoryItem> ExploreDirectory(string dirName);
}