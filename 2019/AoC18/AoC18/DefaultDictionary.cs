using System.Collections.Generic;

namespace AoC18
{
    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TValue : new()
    {
        public new TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out var val))
                {
                    return val;
                }

                val = new TValue();
                Add(key, val);
                return val;
            }
            set => base[key] = value;
        }
    }
}