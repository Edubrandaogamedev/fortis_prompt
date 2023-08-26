using System;
using UnityEngine;
using UnityEngine.AI;
using UtilityModule.Debug;
using UtilityModule.Service;
using Random = UnityEngine.Random;

namespace MovementModule
{
    public class MovementService : IService
    {
        public bool GetRandomPointOnNavMesh(Vector3 center, float range,float maxDistance, int retries, out Vector3 result, Vector2 minimumDistance = default)
        {
            int currentRetry = 0;
            Vector3 randomPos = Random.insideUnitSphere * range + center + (Vector3)minimumDistance;
            NavMeshHit hit;
            while (!NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas) && currentRetry < retries)
            {
                randomPos = Random.insideUnitSphere * maxDistance + center;
                currentRetry++;
                
            }
            if (currentRetry >= retries)
            {
                ServiceLocator.Instance.Get<DebugService>().ShowLog("Failed to find a valid point on Navmesh",DebugLogType.Warning);
                result = center;
                return false;
            }
            result = hit.position;
            return true;
        }

        public bool SetDestinationOnNavMesh(NavMeshAgent agent, Vector3 targetPoint, bool isToCheckPath = true)
        {
            if (!isToCheckPath)
            {
                agent.SetDestination(targetPoint);
                return true;
            }
            
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetPoint, path);
            if (agent.CalculatePath(targetPoint, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                agent.SetDestination(targetPoint);
                return true;
            }
            ServiceLocator.Instance.Get<DebugService>().ShowLog("Failed to find a valid path to the target point.",DebugLogType.Warning);
            return false;
        }

        public bool IsAgentStuckedByTime(NavMeshAgent agent, float lastStartMoveTime, float timeToWait)
        {
            if (agent.velocity.sqrMagnitude == 0 && lastStartMoveTime + timeToWait >= Time.time)
            {
                return true;
            }

            return false;
        }
    }
}