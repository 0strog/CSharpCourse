using System;
using System.Collections.Generic;
using System.Linq;

class DictionaryPriorityQueue<TKey> : IPriorityQueue<TKey>
{
    Dictionary<TKey, double> dictionary = new Dictionary<TKey, double>();

    public void Add(TKey key, double value)
    {
        dictionary.Add(key, value);
    }

    public void Delete(TKey key)
    {
        dictionary.Remove(key);
    }

    public Tuple<TKey, double> ExtractMinKey()
    {
        if (dictionary.Count == 0) return null;
        var min = dictionary.Min(z => z.Value);
        var key = dictionary.Where(z => z.Value == min).FirstOrDefault().Key;
        dictionary.Remove(key);
        return Tuple.Create(key, min);
    }

    public bool TryGetValue(TKey key, out double value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public void Update(TKey key, double newValue)
    {
        dictionary[key] = newValue;
    }
}
