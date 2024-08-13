using CombatManager.Entity;

namespace CombatManager.Json;

public interface IReader<T>
{
    public Task<T?> ReadAsync(string path);
}