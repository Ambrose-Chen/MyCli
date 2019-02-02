using System;

namespace MyCli
{
    class KeyValue<TKey, TValue>
    {
        public KeyValue(TKey Key, TValue Value)
        {
            this.Key = Key;
            this.Value = Value;
        }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }
}