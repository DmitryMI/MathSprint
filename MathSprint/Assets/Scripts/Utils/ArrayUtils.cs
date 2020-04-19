using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = System.Random;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Set of utility functions for arrays
    /// </summary>
    public static class ArrayUtils
    {
        private static Random _random = new Random();
        public static T GetRandomItem<T>(IEnumerable<T> enumerable)
        {
            T[] array = enumerable.ToArray();
            int length = array.Length;
            int index = _random.Next(0, length);
            return array[index];
        }
    }
}
