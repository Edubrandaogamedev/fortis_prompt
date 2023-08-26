using System;
using System.Collections.Generic;
using UnitModule;
using Unity.VisualScripting;
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
        [SerializeField] 
        private float _maxPopulation;
        
        private SpawnService _spawnService;

        private readonly HashSet<UnitController> _spawnedUnits = new();
        private bool _canSpawn;
        private float _currentTime;

        public string Key => _unitKey;
        public HashSet<UnitController> SpawnedUnits => _spawnedUnits;
        public event Action OnUnitCountChange;
        
        

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
            if (_currentTime >= _interval && _spawnedUnits.Count < _maxPopulation)
            {
                UnitController spawnedUnit = _spawnService.SpawnUnit(_unitKey, transform.position);
                spawnedUnit.transform.position = transform.position;
                SetupUnit(spawnedUnit);
                OnUnitCountChange?.Invoke();
                _currentTime = 0;
            }
        }
        
        private void BornUnit(UnitController unit)
        {
            UnitController spawnedUnit = _spawnService.SpawnUnit(_unitKey, unit.transform.position);
            SetupUnit(spawnedUnit);
            OnUnitCountChange?.Invoke();
        }
        
        private void OnUnitDie(UnitController unit)
        {
            _spawnedUnits.Remove(unit);
            OnUnitCountChange?.Invoke();
        }
        
        private void SetupUnit(UnitController unit)
        {
            _spawnedUnits.Add(unit);
            unit.OnUnitSpawn += BornUnit;
            unit.OnDie += OnUnitDie;
            unit.SetInvincibilityStat();
        }


        public void SetInterval(float newInterval)
        {
            _interval = newInterval;
        }

        public void SetMaxPopulation(int maxPopulation)
        {
            _maxPopulation = maxPopulation;
        }
    }
}
