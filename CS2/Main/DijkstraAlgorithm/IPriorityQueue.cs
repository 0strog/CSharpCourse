using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPriorityQueue<TKey>
{
    bool TryGetValue(TKey key, out double value);
    void Add(TKey key, double value);
    void Delete(TKey key);
    void Update(TKey key, double newValue);
    Tuple<TKey, double> ExtractMinKey();
}

public static class PriorityKeyExtention
{
    public static bool UpdateOrAdd<TKey>(this IPriorityQueue<TKey> queue, TKey node, double newValue)
    {
        double oldPrice;
        var nodeInQueue = queue.TryGetValue(node, out oldPrice);
        if (!nodeInQueue || oldPrice > newValue)
        {
            queue.Update(node, newValue);
            return true;
        }
        return false;
    }
}
