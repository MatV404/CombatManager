using System.IO;
using System.Text;
using System.Text.Json;
using CombatManager.Entity;

namespace CombatManager.Json;

public class JsonWriter<T> : IWriter<T>
{
    public async Task<bool> WriteAsync(T data, string path)
    {
        try
        {
            await using var stream = File.Create(path);
            await JsonSerializer.SerializeAsync(stream, data);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }
}