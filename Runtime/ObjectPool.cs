using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Broccollie.Core
{
    public class ObjectPool
    {
        public event Action<GameObject> OnAddToPool = null;
        public event Action<GameObject> OnTakeFromPool = null;
        public event Action<GameObject> OnReturnToPool = null;

        private static Dictionary<string, List<GameObject>> s_pool = new Dictionary<string, List<GameObject>>();
        private static Dictionary<string, (GameObject, Transform)> s_prefabs = new Dictionary<string, (GameObject, Transform)>();

        private int _defaultCapacity = 0;
        private int _maxCapacity = 0;

        public ObjectPool(int defaultCapacity = 10, int maxCapacity = 1000)
        {
            _defaultCapacity = defaultCapacity > maxCapacity ? maxCapacity : defaultCapacity;
            _maxCapacity = maxCapacity;
        }

        #region Public Features
        public void AddPool(GameObject poolObject, Transform holder, string name)
        {
            List<GameObject> newPool = new List<GameObject>();
            for (int i = 0; i < _defaultCapacity; i++)
            {
                GameObject go = GameObject.Instantiate(poolObject, holder);
                go.SetActive(false);
                newPool.Add(go);
                OnAddToPool?.Invoke(go);
            }

            if (s_pool.TryGetValue(name, out List<GameObject> pool))
                pool.AddRange(newPool);
            else
                s_pool.Add(name, newPool);
            
            if (s_prefabs.TryGetValue(name, out (GameObject, Transform) prefix)) return;
            s_prefabs.Add(name, (poolObject, holder));
        }

        public GameObject TakeFromPool(string name)
        {
            if (s_pool.TryGetValue(name, out List<GameObject> pool))
            {
                GameObject result = null;
                foreach (GameObject go in pool)
                {
                    if (!go.activeInHierarchy)
                    {
                        result = go;
                        break;
                    }
                }

                if (result == null && s_prefabs.TryGetValue(name, out (GameObject, Transform) prefix))
                {
                    if (pool.Count >= _maxCapacity) return null;

                    GameObject newGo = GameObject.Instantiate(prefix.Item1, prefix.Item2);
                    newGo.SetActive(false);
                    pool.Add(newGo);
                    OnAddToPool?.Invoke(newGo);
                    result = newGo;
                }

                OnTakeFromPool?.Invoke(result);
                return result;
            }
            else
                return null;
        }

        public void ReturnToPool(string name, GameObject gameObject)
        {
            if (s_pool.TryGetValue(name, out List<GameObject> pool))
            {
                gameObject.SetActive(false);

                if (s_prefabs.TryGetValue(name, out (GameObject, Transform) prefix))
                {
                    if (gameObject.transform.parent != prefix.Item2)
                        gameObject.transform.parent = prefix.Item2;
                }
                
                OnReturnToPool?.Invoke(gameObject);
            }
        }

        #endregion
    }
}
