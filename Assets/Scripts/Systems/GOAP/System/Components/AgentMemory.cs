using System.Collections.Generic;

public class AgentMemory
{
    private Dictionary<string, object> memoryDictionary = new Dictionary<string, object>();

    public T GetValue<T>(string key)
    {
        if (memoryDictionary.ContainsKey(key))
        {
            return (T)memoryDictionary[key];
        }
        return default(T);
    }

    public void SetValue<T>(string key, T value)
    {
        if (memoryDictionary.ContainsKey(key))
        {
            memoryDictionary[key] = value;
        }
        else
        {
            memoryDictionary.Add(key, value);
        }
    }
}