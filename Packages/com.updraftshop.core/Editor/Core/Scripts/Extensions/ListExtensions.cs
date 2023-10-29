using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UpdraftShop
{
    public static class ListExtensions
    {
        public static void Replace<T>(this IList<T> self, int index1, int index2)
        {
            if (self.Count <= index1 || self.Count <= index2)
                return;
        
            (self[index1], self[index2]) = (self[index2], self[index1]);
        }
    }    
}