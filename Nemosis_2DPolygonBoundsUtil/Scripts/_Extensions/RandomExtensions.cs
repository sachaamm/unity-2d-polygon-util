using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class RandomExtensions
    {
        public static T RandomItem<T>(this List<T> t)
        {
            return t[t.RandomIndex()];
        }

        public static int RandomIndex<T>(this List<T> t)
        {
            return t.Count.RandomInteger();
        }
        
        public static int RandomInteger(this int i)
        {
            return Random.Range(0, i);
        }
    }
}