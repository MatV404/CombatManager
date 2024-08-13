using System.IO;
using System.Text.Json;
using CombatManager.Entity;
using CombatManager.Utils.Exceptions;

namespace CombatManager.Json;

public class JsonReader<T> : IReader<T>
{
    public async Task<T?> ReadAsync(string path)
    {
        try
        {
            await using var file = File.OpenRead(path);
            return await JsonSerializer.DeserializeAsync<T>(file);
        }
        catch (Exception e)
        {
            throw new ReaderException(e.Message);
        }
    }
}