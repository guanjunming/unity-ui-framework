﻿using System.Collections.Generic;
using UnityEngine;

namespace UIFramework.Utils
{
    public static class SimpleObjMgr
    {
        public static SimpleObjectManager Instance;

        static SimpleObjMgr()
        {
            Instance = new SimpleObjectManager();
        }
    }

    public class SimpleObjectManager
    {
        public List<GameObject> InitGameObjectPool(Transform parent, GameObject inst, Vector3 localpos, Vector3 localscale, int poolsize = 10)
        {
            List<GameObject> retval = new List<GameObject>();

            for (int i = 0; i < poolsize; ++i)
            {
                GameObject newinst = Object.Instantiate(inst, Vector3.zero, Quaternion.identity);
                newinst.transform.SetParent(parent, false);
                newinst.transform.localPosition = localpos;
                newinst.transform.localScale = localscale;

                newinst.SetActive(false);
                retval.Add(newinst);
            }
            return retval;
        }

        public Queue<GameObject> InitGameObjectPoolQueue(Transform parent, GameObject inst, Vector3 localpos, Vector3 localscale, int poolsize = 10)
        {
            Queue<GameObject> retval = new Queue<GameObject>();

            for (int i = 0; i < poolsize; ++i)
            {
                GameObject newinst = Object.Instantiate(inst, Vector3.zero, Quaternion.identity);
                newinst.transform.SetParent(parent, false);
                newinst.transform.localPosition = localpos;
                newinst.transform.localScale = localscale;

                newinst.SetActive(false);
                retval.Enqueue(newinst);
            }
            return retval;
        }

        public GameObject GetContainerObject(List<GameObject> container)
        {
            for (int i = 0; i < container.Count; ++i)
            {
                GameObject obj = container[i];
                if (!obj.activeSelf)
                    return obj;
            }
            return null;
        }

        public List<GameObject> GetActiveContainerObjects(List<GameObject> container)
        {
            List<GameObject> retlist = new List<GameObject>();
            for (int i = 0; i < container.Count; ++i)
            {
                GameObject obj = container[i];
                if (obj.activeSelf)
                    retlist.Add(obj);
            }
            return retlist;
        }

        public void ResetContainerObject(List<GameObject> container)
        {
            for (int i = 0; i < container.Count; ++i)
            {
                container[i].SetActive(false);
            }
        }

        public void DestroyContainerObject(List<GameObject> container)
        {
            for (int i = 0; i < container.Count; ++i)
            {
                Object.Destroy(container[i]);
            }
        }

        public void DestroyContainerObject(Queue<GameObject> container)
        {
            while (container.Count != 0)
            {
                Object.Destroy(container.Dequeue());
            }
        }
    }
}