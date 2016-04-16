using ExtensionMethods;
using UnityEngine;

namespace Miscellaneous.ObjectPooling
{
    /// <summary>
    /// An object that supports object pooling.
    /// </summary>
    public class PooledObject
    {
        // .. ATTRIBUTES

        public GameObject GameObject { get { return theGameObject; } }

        private GameObject theGameObject = null;
        private ObjectPoolComponent pool = null;

        // .. INITIALIZATION

        public PooledObject(ObjectPoolComponent pool, GameObject game_object)
        {
            this.pool = pool;
            theGameObject = game_object;
        }

        // .. OPERATIONS

        /// <summary>
        /// Release the object so it can be re-used.
        /// </summary>
        public void Release()
        {
            pool.ReleasePooledObject(this);
            GameObject.GetComponent<Transform>().SetParent(pool.GetComponent<Transform>(), true);
        }

        public T GetComponent<T>()
        {
            return theGameObject.GetComponent<T>();
        }

        public bool FindComponent<T>(out T find_component)
        {
            return theGameObject.FindComponent<T>(out find_component);
        }
    };
}
