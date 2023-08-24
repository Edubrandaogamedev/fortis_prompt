using CharacterModule;
using UnityEngine;
using UtilityModule.Pooling;
using UtilityModule.Service;

namespace SpawnModule
{
    public class SpawnService : MonoBehaviour,IService
    {
        [SerializeField] 
        private PoolManager _poolManager;


        private void Awake()
        {
            ServiceLocator.Instance.BindService(this);
            _poolManager.Initialize();
        }

        public UnitController SpawnUnit(string unitKey, Vector3 location, Quaternion rotation = default, Transform parent = null)
        {
            return _poolManager.Spawn<UnitController>(unitKey, parent);
        }
    }
}
