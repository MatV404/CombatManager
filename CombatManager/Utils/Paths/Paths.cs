using System.IO;
using System.Text;

namespace CombatManager.Utils.Paths;

public class Paths
{
    public const string CreatureDirName = "entity";
    public const string CombatDirName = "combat";

    public static string GetStoragePath(string dirName)
    {
        var dir = Directory.GetCurrentDirectory();
        try
        {
            var result = Directory.CreateDirectory(Path.Join(dir, "save", dirName));
            return result.FullName;
        }
        catch (Exception)
        {
            return dir;
        }
    }

    public static string GetFullPath(string name, string path)
    {
        var fileName = SanitizeFileName(name) + ".json";
        return Path.Join(path, fileName);
    }

    private static string SanitizeFileName(string input)
    {
        StringBuilder builder = new();

        foreach (var c in input.Where(
                     char.IsLetterOrDigit))
        {
            builder.Append(c);
        }

        return builder.ToString();
    }
}