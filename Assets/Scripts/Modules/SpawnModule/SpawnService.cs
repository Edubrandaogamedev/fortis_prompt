using UnitModule;
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
            return _poolManager.Spawn<UnitController>(unitKey, location, rotation, parent).Initialize();
        }

        public UnitController SpawnUnitOnRandomCircle(string unitKey, Vector3 center, float radius, Quaternion rotation = default, Transform parent = null)
        {
            Vector3 randomPoint= Random.insideUnitSphere * radius + center;
            Vector3 spawnLocation = new Vector3(randomPoint.x, center.y, randomPoint.z);
            return SpawnUnit(unitKey, spawnLocation, rotation, parent).Initialize();
        }
    }
}
