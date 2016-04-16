using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Miscellaneous.ObjectPooling
{
    /// <summary>
    /// A helper class to manage pooled objects.
    /// </summary>
    public class ObjectPoolComponent : MonoBehaviour
    {
        // .. ATTRIBUTES

        [SerializeField]
        private GameObject PrefabToClone = null;
        [SerializeField]
        private int Size = 20;
        [SerializeField]
        private bool ItHasWildGrowth = false;

        private List<PooledObject> unusedPoolTable = new List<PooledObject>(20);
        private List<PooledObject> usedPoolTable = new List<PooledObject>(20);


        // .. INITIALIZATION

        public void Start()
        {
            for (int index = unusedPoolTable.Count; index < Size; index++)
            {
                CreateNewObject();
            }
        }

        // .. OPERATIONS

        /// <summary>
        /// Get a pooled object
        /// </summary>
        /// <returns></returns>
        public PooledObject FetchPooledObject()
        {
            int
                result_index;

            for (int index = unusedPoolTable.Count + usedPoolTable.Count; index < Size; index++)
            {
                CreateNewObject();
            }

            result_index = -1;

            for (int index = 0; index < unusedPoolTable.Count; index++)
            {
                if (unusedPoolTable[index] != null)
                {
                    result_index = index;
                    break;
                }
            }

            if (result_index == -1 && ItHasWildGrowth)
            {
                result_index = CreateNewObject();
            }

            if (result_index != -1)
            {
                usedPoolTable[result_index] = unusedPoolTable[result_index];
                unusedPoolTable[result_index] = null;

                return usedPoolTable[result_index];
            }

            return null;
        }

        /// <summary>
        /// Releases a pooled object (in order for it to be re-used)
        /// </summary>
        /// <param name="pool_object"></param>
        public void ReleasePooledObject(PooledObject pool_object)
        {
            int
                result_index;

            result_index = usedPoolTable.IndexOf(pool_object);

            unusedPoolTable[result_index] = usedPoolTable[result_index];
            usedPoolTable[result_index] = null;
        }

        /// <summary>
        /// Releases the pooled object containing given gameobject, if any exists.
        /// </summary>
        /// <param name="game_object"></param>
        public void ReleasPooledObjectFromGameObject(GameObject game_object)
        {
            var pooled_object = usedPoolTable.FirstOrDefault(e => e != null && e.GameObject == game_object);
            if (pooled_object != null)
                ReleasePooledObject(pooled_object);
        }

        public bool HasPooledObjects()
        {
            return unusedPoolTable.Count > 0;
        }


        private int CreateNewObject()
        {
            GameObject
                new_game_object;

            new_game_object = (GameObject)GameObject.Instantiate(PrefabToClone);

            new_game_object.SetActive(false);

            new_game_object.GetComponent<Transform>().SetParent(GetComponent<Transform>(), false);

            unusedPoolTable.Add(new PooledObject(this, new_game_object));
            usedPoolTable.Add(null);

            return unusedPoolTable.Count - 1;
        }


    }
}