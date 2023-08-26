using UnityEngine;
using UnityEngine.Pool;

namespace UtilityModule.Pooling
{
    public class PoolableItem : MonoBehaviour
    {
        [SerializeField]
        private PoolItemData _data; 
        
        private IObjectPool<PoolableItem> _pool;
        
        private PoolManager _poolManager;

        public string Key => _data.Key;
        public int PoolCount => _data.PoolCount;
        public bool IsReleased { get; private set; }
        
        public void Initialize(PoolManager poolManager, IObjectPool<PoolableItem> originContext)
        {
            _pool = originContext;
            _poolManager = poolManager;
            _poolManager.OnCleanup += OnCleanup;
            _poolManager.OnRelease += OnRelease;
        }

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
            IsReleased = !active;
        }

        public void OnRelease()
        {
            if (IsReleased)
            {
                return;
            }
            _pool.Release(this);
        }

        private void OnDestroy()
        {
            if (_poolManager == null)
            {
                return;
            }
            _poolManager.OnCleanup -= OnCleanup;
            _poolManager.OnRelease -= OnRelease;
        }

        private void OnCleanup()
        {
            OnRelease();
            Destroy(gameObject);
        }
    }
}