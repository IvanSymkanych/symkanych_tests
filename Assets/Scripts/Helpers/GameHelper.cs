using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Helpers
{
    public static class GameHelper
    {
        public static float GetClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        public static float GetPercent(float value, float maxValue) =>
            Mathf.Clamp(value / maxValue, 0f, 1f);

        public static bool ShouldRicochetTrigger(float healthPercentage)
        {
            var invertedPercent = (1 - healthPercentage) * 100;
            var chance = Mathf.Max(invertedPercent, 0f);
            var randomValue = Random.Range(0, 100);
            return randomValue <= chance;
        }
        
        public static Vector3 FindNearestPosition(List<Vector3> positionsList, Vector3 currentPosition)
        {
            var minDistance = Mathf.Infinity;
            var nearestPosition = Vector3.zero;
            
            foreach (var position in positionsList)
            {
                var distance = Vector3.Distance(currentPosition, position);
                
                if (!(distance < minDistance)) 
                    continue;
                minDistance = distance;
                nearestPosition = position;
            }
            
            return nearestPosition;
        }

        public static bool ReachedDestinationOrGaveUp(this NavMeshAgent navMeshAgent)
        {
            if (navMeshAgent.pathPending)
                return false;

            if (!(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance))
                return false;

            return !navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f;
        }
    }
}