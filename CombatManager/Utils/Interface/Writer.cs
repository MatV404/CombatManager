using CombatManager.Entity;

namespace CombatManager.Json;

public interface IWriter<T>
{
    public Task<bool> WriteAsync(T data, string path);
}