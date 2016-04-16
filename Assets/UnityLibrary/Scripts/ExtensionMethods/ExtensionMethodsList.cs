using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class ExtensionMethodsList
    {
        // -- PUBLIC

        // .. FUNCTIONS

        public static bool AddIfNotContains< T >(
            this List< T > list,
            T obj
            )
        {
            if ( !list.Contains ( obj ) )
            {
                list.Add( obj );
                return true;
            }

            return false;
        }
    }
}
