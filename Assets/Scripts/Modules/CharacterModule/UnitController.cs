using System;
using MovementModule;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UtilityModule.Service;

namespace CharacterModule
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] 
        private NavMeshAgent _agent;
        [SerializeField] 
        private NavMeshMovementSettings _movingSettings;
        [SerializeField]
        private string Key;

        private MovementService _movementService;
        private void Start()
        {
            _movementService = ServiceLocator.Instance.Get<MovementService>();
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
    }
}