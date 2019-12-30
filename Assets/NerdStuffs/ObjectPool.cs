using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPool : MonoBehaviour
    {
        public GameObject prefab;

        HashSet<GameObject> used;
        HashSet<GameObject> available;

        protected void Awake()
        {
            used = new HashSet<GameObject>();
            available = new HashSet<GameObject>();
        }

        public int ActiveObjects { get { return used.Count; } }

        public GameObject Create()
        {
            foreach (var old in available)
            {
                used.Add(old);
                available.Remove(old);
                old.SetActive(true);
                return old;
            }

            var fab = Instantiate(prefab);
            fab.transform.SetParent(transform);
            used.Add(fab);
            return fab;
        }

        public void Dispose(GameObject o)
        {
            if (used.Contains(o))
            {
                used.Remove(o);
                o.SetActive(false);
                available.Add(o);
            }
            else
                Debug.LogError(o + " is not a part of this object pool! Only use Destroy to dispose objects yielded from Create!");
        }
    }
}
