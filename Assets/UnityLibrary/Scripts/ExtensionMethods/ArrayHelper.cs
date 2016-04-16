using System;

namespace ExtensionMethods
{
    public static class ArrayHelper
    {
        public static void Append<T>(ref T[] array, T[] other_array)
        {
            Array.Resize(ref array, array.Length + other_array.Length);
            other_array.CopyTo(array, array.Length - other_array.Length);
        }

        public static void Add<T>(ref T[] array, T element)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = element;
        }
    }
}
