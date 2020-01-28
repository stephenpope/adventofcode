using System;
using System.Collections.Generic;

namespace AoC18
{
    public class MiniDictionary
    {
        private int[] buckets;
        private Entry[] entries;
        private int freeCount;
        private int freeList;
        private int count;

        public static readonly int[] primes =
        {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
        };

        private struct Entry
        {
            public long hashCode; // Lower 31 bits of hash code, -1 if unused
            public long key;      // Key of entry
            public int value;     // Value of entry
            public int next;
        }

        public MiniDictionary(int capacity)
        {
            int size = GetPrime(capacity);
            buckets = new int[size];
            for (int i = 0; i < buckets.Length; i++) buckets[i] = -1;
            entries = new Entry[size];
            freeList = -1;
        }

        private int GetPrime(int min)
        {
            for (int i = 0; i < primes.Length; i++)
            {
                int prime = primes[i];
                if (prime >= min) return prime;
            }

            throw new Exception("Prime!");
        }

        private void Add(long key, int value)
        {
            long hashCode = key.GetHashCode() & 0x7FFFFFFF;
            long targetBucket = hashCode % buckets.Length;

            var index = count;
            count++;

            entries[index].hashCode = hashCode;
            entries[index].next = buckets[targetBucket];
            entries[index].key = key;
            entries[index].value = value;
            buckets[targetBucket] = index;
        }

        public bool ContainsKey(long key)
        {
            return FindEntry(key) >= 0;
        }

        private int FindEntry(long key)
        {
            int hashCode = key.GetHashCode() & 0x7FFFFFFF;

            for (int i = buckets[hashCode % buckets.Length]; i >= 0; i = entries[i].next)
            {
                if (entries[i].hashCode == hashCode && entries[i].key == key) return i;
            }

            return -1;
        }

        public int this[long key]
        {
            get
            {
                var i = FindEntry(key);

                if (i >= 0)
                {
                    return entries[i].value;
                }

                throw new KeyNotFoundException();
            }
            
            set => Add(key, value);
        }
    }
}