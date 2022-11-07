using System.Collections.Generic;

namespace _Extensions
{
    public static class ListExtensions
    {
        public static bool IsValidIndex<T>(this List<T> l, int i)
        {
            return l != null && l.Count > i && i >= 0;
        }
        
        public static bool IsValidIndex<T>(this HashSet<T> h, int i)
        {
            return h != null && h.Count > i && i >= 0;
        }
    }
}