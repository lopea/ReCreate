using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lopea.Tools
{
    public class GameObjectPool
    {
        GameObject _prefab;
        int _max;

        /// <summary>
        /// stores all inactive gameobjects
        /// </summary>
        Queue<GameObject> inactiveGO;

        /// <summary>
        /// stores all active gameObjects
        /// </summary>
        List<GameObject> currentGO;

        /// <summary>
        /// GameObject reference to create more instances of.
        /// </summary>
        public GameObject prefab => _prefab;

        /// <summary>
        /// The maximum instances of the prefab given to spawn
        ///</summary>
        public int maxObjects => _max;

        /// <summary>
        /// Current number of active gameobjects
        /// </summary>
        public int ActiveCount => currentGO.Count;


        public GameObject this[int index] => currentGO[index];
        public GameObjectPool(GameObject prefab, int max, Transform parent = null)
        {
            if(max >= 0)
            {
                throw new System.Exception("Error when creating GameObjectPool: invalid maximum value.");
            }
            
            //initialize lists
            inactiveGO = new Queue<GameObject>();
            currentGO = new List<GameObject>();

            //set variables
            _max = max;
            _prefab = prefab;

            //create new instances of the prefab
            for(int i = 0; i < max; i++)
            {
                var instance = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, parent);
                instance.SetActive(false);

                //set to the inactive list
                inactiveGO.Enqueue(instance);
            }
        }
        
        public GameObject GetNextGameObject()
        {
            var next = inactiveGO.Dequeue();
            if(!next.activeSelf)
            { 
                next.SetActive(true);
                currentGO.Add(next);
            }
            inactiveGO.Enqueue(next);
            return next;
        }

        public void PopulateActiveList(int count)
        {
            for(int i = 0; i < count; i ++)
            {
                GetNextGameObject();
            }
        }
        
        public void Deactivate(GameObject obj)
        {
            obj.SetActive(false);
            currentGO.Remove(obj);
            inactiveGO.Enqueue(obj);
        }

    }
}