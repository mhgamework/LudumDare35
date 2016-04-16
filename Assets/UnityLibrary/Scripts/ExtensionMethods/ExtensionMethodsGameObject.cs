using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtensionMethods
{
    public static class ExtensionMethodsGameObject
    {
        // -- PUBLIC

        // .. EXTENSION METHODS

        public static bool FindComponent<T>(this GameObject game_object, out T find_component)
        {
            find_component = game_object.GetComponent<T>();
            var ref_result = find_component as Component;

            bool result = ref_result != null;
            return result;
        }

        public static T AddComponent<T>(this GameObject game_object, T toAdd) where T : Component
        {
            return game_object.AddComponent<T>().GetCopyOf(toAdd) as T;
        }


        public static T GetInterface<T>(this GameObject game_object) where T : class
        {
            if (!typeof(T).IsInterface)
            {
                Debug.LogError(typeof(T).ToString() + ": is not an actual interface!");
                return null;
            }

            return game_object.GetComponents<Component>().OfType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> GetInterfaces<T>(this GameObject game_object) where T : class
        {
            if (!typeof(T).IsInterface)
            {
                Debug.LogError(typeof(T).ToString() + ": is not an actual interface!");
                return Enumerable.Empty<T>();
            }

            return game_object.GetComponents<Component>().OfType<T>();
        }

        public static IEnumerable<T> GetInterfacesInChildren<T>(this GameObject game_object) where T : class
        {
            if (!typeof(T).IsInterface)
            {
                Debug.LogError(typeof(T).ToString() + ": is not an actual interface!");
                return Enumerable.Empty<T>();
            }

            return game_object.GetComponentsInChildren<Component>().OfType<T>();
        }
    }
}
