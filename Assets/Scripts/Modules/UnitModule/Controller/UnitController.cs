using System;
using System.Collections;
using MovementModule;
using UnityEngine;
using UnityEngine.AI;
using UtilityModule.Service;

namespace UnitModule
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField]
        private UnitData _data;
        [SerializeField] 
        private NavMeshMovementSettings _movingSettings;

        private UnitStats _stats = new();
        private NavMeshAgent _agent;
        private MovementService _movementService;
        private bool _isInitialized;
        private float _lastStartedTime;

        public string Key => _data.Key;
        public bool IsMoving => _agent.velocity.sqrMagnitude > 0;
        public event Action<UnitController> OnDie;
        public event Action<UnitController> OnUnitSpawn;

        public UnitController Initialize()
        {
            _stats = new UnitStats();
            _isInitialized = true;
            _agent.enabled = true;
            return this;
        }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            _movementService = ServiceLocator.Instance.Get<MovementService>();
        }
        
        private void Update()
        {
            if (!_isInitialized)
            {
                return;
            }
            TrySetDestination();
            
            if (_movementService.IsAgentStuckedByTime(_agent, _lastStartedTime, _movingSettings.StuckTimeChecker))
            {
                _agent.ResetPath();
                TrySetDestination();
            }
            
        }

        private void TrySetDestination()
        {
            if (_agent.pathPending || (_agent.remainingDistance > _agent.stoppingDistance))
            {
                return;
            }

            bool pointFound = _movementService.GetRandomPointOnNavMesh(transform.position, _movingSettings.MovementRange,
                _movingSettings.MaxSearchDistance, _movingSettings.SearchRetriesCount, out var position, _movingSettings.MinimumDistance);
            if (!pointFound)
            {
                return;
            }
            if (!_movementService.SetDestinationOnNavMesh(_agent, position))
            {
                return;
            }
            _lastStartedTime = Time.time;
        }

        private void OnTriggerEnter(Collider other)
        {
            UnitController collidedUnit = other.gameObject.GetComponent<UnitController>();
            if (collidedUnit == null)
            {
                return;
            }

            if (IsDamageCollision(collidedUnit))
            {
                HandleDamageCollision(collidedUnit);    
            }
            else
            {
                if (_stats.Has(Stats.Invincible) || collidedUnit._stats.Has(Stats.Invincible))
                {
                    return;
                }
                HandleCreationCollision(collidedUnit);
            }
        }

        private bool IsDamageCollision(UnitController collidedUnit)
        {
            return collidedUnit.Key != Key;
        }
        private void HandleDamageCollision(UnitController collidedUnit)
        {
            _isInitialized = false;
            _agent.enabled = false;
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