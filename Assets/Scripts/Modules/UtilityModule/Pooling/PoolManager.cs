using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;
using UtilityModule.Debug;
using UtilityModule.Service;

namespace UtilityModule.Pooling
{
    public class PoolManager : MonoBehaviour
    {
	    public static int DEFAULT_POOL_COUNT = 1;
        
        [SerializeField] 
        private List<PoolableItem> _poolableItems = new();
        
        [SerializeField] 
        private bool _collectionChecks = true;
        
        [SerializeField] 
        private int _maxPoolSize = 10;

        private Dictionary<string, IObjectPool<PoolableItem>> _pools;
        private readonly Dictionary<string, GameObject> _poolsSubContext = new();
        private readonly Dictionary<PoolableItem, List<Component>> _cachedPooledComponent = new();

        public bool IsInitialized { get; private set; }

        public event Action OnCleanup;
        public event Action OnRelease;

        public void Initialize()
        {	
	        if (IsInitialized)
	        {
		        return;
	        }
	        _pools = DictionaryPool<string, IObjectPool<PoolableItem>>.Get();
	        foreach (PoolableItem poolableItem in _poolableItems)
	        {
		        CreateObjectPool(poolableItem);
		        InitializeObjectPool(poolableItem);
	        }
	        OnRelease?.Invoke();
	        IsInitialized = true;
        }
	
        public void TerminatePool(bool garbageCollect = true)
        {
	        if (!IsInitialized)
	        {
		        return;
	        }
	        
	        OnCleanup?.Invoke();
	        DictionaryPool<string, IObjectPool<PoolableItem>>.Release(_pools);
	        ClearCachedInformation();
	        IsInitialized = false;
	        OnCleanup = null;

	        if (garbageCollect)
	        {
		        GC.Collect();
	        }
        }

        public void ReleaseAllPolledObjects()
        {
	        if (!IsInitialized)
	        {
		        return;
	        }

	        ClearCachedInformation();
	        OnRelease?.Invoke();
        }
        
        public T Spawn<T>(string key, Vector3 location = default, Quaternion rotation = default, Transform parent = null) where T : Component
        {
	        if (!_pools.TryGetValue(key, out IObjectPool<PoolableItem> objectPool))
	        {
		        DebugService debugService = ServiceLocator.Instance.Get<DebugService>();
		        if (!IsInitialized)
		        {
			        debugService.ShowLog("Initialize Pool Manager first",DebugLogType.Error);
		        }
		        else
		        {
			        debugService.ShowLog("Tried to spawn {0} but the pool was not created. Try to check if the {0} prefab is registered on pool manager object", DebugLogType.Error,key);
		        }
		        
		        return null;
	        }
	        
	        PoolableItem pooledItem = objectPool.Get();
	        pooledItem.transform.position = location;
	        pooledItem.transform.rotation = rotation;
	        if (parent)
	        {
		        pooledItem.transform.SetParent(parent);
	        }
	        
	        return GetCachedComponent<T>(pooledItem);
        }
        
        private void CreateObjectPool(PoolableItem item)
        {
	        if (_pools.ContainsKey(item.Key))
	        {
		        ServiceLocator.Instance.Get<DebugService>().ShowLog("Trying to create the {0} object but the pool is already created",DebugLogType.Warning,item.Key);
		        return;
	        }
	        CreatePoolSubsection(item.Key);
	        _pools.Add(item.Key, new ObjectPool<PoolableItem>(() => CreatePoolableItem(item), OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, 
			        _collectionChecks, item.PoolCount, _maxPoolSize
		        )
	        );
        }
        
        private void InitializeObjectPool(PoolableItem item)
        {
	        IObjectPool<PoolableItem> objectPool = _pools[item.Key];
	        for(var i = 0; i < item.PoolCount; ++i)
	        {
		        objectPool.Get();
	        }
        }

        private void CreatePoolSubsection(string key)
        {
	        const string subContextPath = "_poolContext"; 
	        GameObject poolSubcontext = new GameObject();
	        poolSubcontext.name = key + subContextPath;
	        poolSubcontext.transform.parent = transform;
	        _poolsSubContext[key] = poolSubcontext; 
        }
        
        private PoolableItem CreatePoolableItem(PoolableItem item)
        {
	        PoolableItem itemInstance = Instantiate(item, Vector3.zero, Quaternion.identity, _poolsSubContext[item.Key].transform);
	        itemInstance.name = item.name;
	        itemInstance.Initialize(this,_pools[item.Key]);
	        return itemInstance;
        }

        private void OnTakeFromPool(PoolableItem itemToTake)
        {
	        itemToTake.SetActive(true);
        }

        private void OnReturnedToPool(PoolableItem itemToReturn)
        {
	        itemToReturn.transform.SetParent(_poolsSubContext[itemToReturn.Key].transform);
	        itemToReturn.SetActive(false);
        }

        private void OnDestroyPoolObject(PoolableItem itemToDestroy)
        {
	        _cachedPooledComponent.Remove(itemToDestroy);
	        Destroy(itemToDestroy.gameObject);
	        
        }
        
        private T GetCachedComponent<T>(PoolableItem item) where T : Component
        {
	        if (!_cachedPooledComponent.ContainsKey(item))
	        {
		        _cachedPooledComponent[item] = new List<Component>();
	        }
	        
	        T foundComponent = _cachedPooledComponent[item].Find(targetComponent => targetComponent is T) as T;
	        
	        if (foundComponent != null)
	        {
		        return foundComponent;
	        }
    
	        T component = item.GetComponent<T>();
		    _cachedPooledComponent[item].Add(component);
		    return component;
        }
        
        private void ClearCachedInformation(bool garbageCollect = true)
        {
	        _cachedPooledComponent.Clear();
	        foreach (GameObject subContextObject in _poolsSubContext.Values)
	        {
		        Destroy(subContextObject);
	        }
	        _poolsSubContext.Clear();
	        if (garbageCollect)
	        {
		        GC.Collect();
	        }
        }
    }
}