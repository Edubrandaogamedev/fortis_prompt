using System;
using System.Collections;
using MovementModule;
using UnityEngine;
using UnityEngine.AI;
using UtilityModule.Pooling;
using UtilityModule.Service;

namespace CharacterModule
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField]
        private UnitData _data;
        [SerializeField] 
        private NavMeshMovementSettings _movingSettings;

        private UnitStats _stats;
        private NavMeshAgent _agent;
        private MovementService _movementService;

        public string Key => _data.Key;
        public event Action<UnitController> OnDie;
        public event Action<UnitController> OnUnitSpawn;

        public UnitController Initialize()
        {
            _stats = new UnitStats();
            return this;
        }
        private void Start()
        {
            _movementService = ServiceLocator.Instance.Get<MovementService>();
            _agent = GetComponent<NavMeshAgent>();
            
        }

        
        private void Update()
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                TryMove();
            }
        }

        private bool TryMove()
        {
            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (_movementService.GetRandomPointOnNavMesh(transform.position, _movingSettings.MovementRange, _movingSettings.MaxSearchDistance,
                        _movingSettings.SearchRetriesCount, out var position,_movingSettings.MinimumDistance))
                {
                    return _movementService.SetDestinationOnNavMesh(_agent,position);
                }
            }
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            UnitController collidedUnit = other.gameObject.GetComponent<UnitController>();
            if (collidedUnit == null)
            {
                return;
            }

            if (_stats.Has(Stats.Invincible) || collidedUnit._stats.Has(Stats.Invincible))
            {
                return;
            }

            if (IsDamageCollision(collidedUnit))
            {
                HandleDamageCollision(collidedUnit);
            }
            else
            {
                HandleCreationCollision(collidedUnit);
            }
        }

        private bool IsDamageCollision(UnitController collidedUnit)
        {
            return collidedUnit.Key != Key;
        }
        private void HandleDamageCollision(UnitController collidedUnit)
        {
            collidedUnit.OnDie?.Invoke(this);
            OnDie?.Invoke(this);
        }

        private void HandleCreationCollision(UnitController collidedUnit)
        {
            OnUnitSpawn?.Invoke(this);
            collidedUnit.SetInvincibilityStat();
            SetInvincibilityStat();
        }
        
        private IEnumerator InvincibilityTime()
        {
            _stats.Add(Stats.Invincible);
            yield return new WaitForSeconds(_data.InvincibilityTime);
            _stats.Remove(Stats.Invincible);
        }
        
        public void SetInvincibilityStat()
        {
            StartCoroutine(InvincibilityTime());
        }
        
    }
}