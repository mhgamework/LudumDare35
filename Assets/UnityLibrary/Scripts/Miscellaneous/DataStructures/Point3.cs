using System;
using UnityEngine;

namespace Miscellaneous.DataStructures
{
    /// <summary>
    /// A three-valued vector with integer components.
    /// </summary>
    [Serializable]
    public class Point3
    {
        // .. ATTRIBUTES

        public int x { get { return components[0]; } }
        public int y { get { return components[1]; } }
        public int z { get { return components[2]; } }

        [SerializeField]
        private int[] components;


        // .. INITIALIZATION

        public Point3()
        {
            components = new int[3];
            components[0] = 0;
            components[1] = 0;
            components[2] = 0;
        }

        public Point3(int x, int y, int z)
        {
            components = new int[3];
            components[0] = x;
            components[1] = y;
            components[2] = z;
        }

        // .. OPERATIONS

        public int this[int i]
        {
            get { return components[i]; }
        }

    }
}
