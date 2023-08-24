using System;
using UnityEngine;

namespace UtilityModule.Pooling
{
    [Serializable]
    public class PoolItemData
    {
        public GameObject Prefab;
        public string Key;
        public int PoolCount = PoolManager.DEFAULT_POOL_COUNT;
    }
}