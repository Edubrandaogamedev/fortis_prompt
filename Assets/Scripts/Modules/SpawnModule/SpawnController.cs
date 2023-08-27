using System;
using System.Collections.Generic;
using MovementModule;
using UnitModule;
using Unity.VisualScripting;
using UnityEngine;
using UtilityModule.Service;

namespace SpawnModule
{
    public class SpawnController : MonoBehaviour
    {
        private const float SPAWN_RADIUS = 10f;
        [SerializeField] 
        private string _unitKey;
        [SerializeField] 
        private float _interval;
        [SerializeField] 
        private float _maxPopulation;

        private SpawnService _spawnService;
        private MovementService _movementService;

        private readonly HashSet<UnitController> _spawnedUnits = new();
        private bool _canSpawn;
        private float _currentTime;

        public string Key => _unitKey;
        public HashSet<UnitController> SpawnedUnits => _spawnedUnits;
        public event Action OnUnitCountChange;
        
        

        private void Start()
        {
            _spawnService = ServiceLocator.Instance.Get<SpawnService>();
            _movementService = ServiceLocator.Instance.Get<MovementService>();
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
                _movementService.GetRandomPointOnNavMesh(transform.position, 
                    SPAWN_RADIUS, SPAWN_RADIUS, 5,
                    out Vector3 spawnPosition);
                UnitController spawnedUnit = _spawnService.SpawnUnit(_unitKey, spawnPosition);
                SetupUnit(spawnedUnit);
                OnUnitCountChange?.Invoke();
                _currentTime = 0;
            }
        }
        
        private void BornUnit(UnitController unit)
        {
            if (_spawnedUnits.Count < _maxPopulation)
            {
                _movementService.GetRandomPointOnNavMesh(unit.transform.position, 
                    SPAWN_RADIUS/3f, SPAWN_RADIUS, 5,
                    out Vector3 bornPosition);
                UnitController spawnedUnit = _spawnService.SpawnUnit(_unitKey, bornPosition);
                SetupUnit(spawnedUnit);
                OnUnitCountChange?.Invoke();
                
            }
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
