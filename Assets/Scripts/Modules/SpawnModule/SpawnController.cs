using System.Collections.Generic;
using UnitModule;
using UnityEngine;
using UtilityModule.Service;

namespace SpawnModule
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField] 
        private string _unitKey;
        [SerializeField] 
        private float _interval;
        
        private SpawnService _spawnService;

        private HashSet<UnitController> _spawnedUnits = new();
        private bool _canSpawn;
        private float _currentTime;

        public string Key => _unitKey;

        private void Start()
        {
            _spawnService = ServiceLocator.Instance.Get<SpawnService>();
            _canSpawn = true;
        }

        private void Update()
        {
            if (!_canSpawn)
            {
                return;
            }
            _currentTime += Time.deltaTime;
            TrySpawnUnit();
        }

        private void TrySpawnUnit()
        {
            if (_currentTime >= _interval)
            {
                UnitController spawnedUnit = _spawnService.SpawnUnit(_unitKey, transform.position);
                _spawnedUnits.Add(spawnedUnit);
                spawnedUnit.OnUnitSpawn += BornUnit;
                spawnedUnit.SetInvincibilityStat();
                _currentTime = 0;
            }
        }
        
        private void BornUnit(UnitController unit)
        {
            UnitController spawnedUnit = _spawnService.SpawnUnit(_unitKey, unit.transform.position);
            _spawnedUnits.Add(spawnedUnit);
            spawnedUnit.OnUnitSpawn += BornUnit;
            spawnedUnit.SetInvincibilityStat();
        }

        public void SetInterval(float newInterval)
        {
            _interval = newInterval;
        }
    }
}
