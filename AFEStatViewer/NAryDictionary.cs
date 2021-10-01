using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFEStatViewer
{
    // https://softwareengineering.stackexchange.com/questions/319264/dictionary-of-dictionaries-design-in-c
    public class NAryDictionary<TKey, TValue> :
    Dictionary<TKey, TValue>
    {
    }

    public class NAryDictionary<TKey1, TKey2, TValue> :
    Dictionary<TKey1, NAryDictionary<TKey2, TValue>>
    {
    }

    public class NAryDictionary<TKey1, TKey2, TKey3, TValue> :
    Dictionary<TKey1, NAryDictionary<TKey2, TKey3, TValue>>
    {
    }

    public static class NAryDictionaryExtensions
    {
        public static NAryDictionary<TKey2, TValue> New<TKey1, TKey2, TValue>(this NAryDictionary<TKey1, TKey2, TValue> dictionary)
        {
            return new NAryDictionary<TKey2, TValue>();
        }

        public static NAryDictionary<TKey2, TKey3, TValue> New<TKey1, TKey2, TKey3, TValue>(this NAryDictionary<TKey1, TKey2, TKey3, TValue> dictionary)
        {
            return new NAryDictionary<TKey2, TKey3, TValue>();
        }
    }
}
