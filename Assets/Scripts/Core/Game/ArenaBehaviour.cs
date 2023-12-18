using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Core.Game
{
    public class ArenaBehaviour : MonoBehaviour
    {
        [SerializeField] private LayerMask enemyLayerMask;
        [SerializeField] private LayerMask playerLayerMask;

        private NavMeshTriangulation _triangulation;

        public Vector3 GetRandomPositionInArenaWithoutEnemy() =>
            GetRandomPositionInArenaWithoutLayerMask(playerLayerMask);
        
        public Vector3 GetRandomPositionInArenaWithoutPlayer() =>
            GetRandomPositionInArenaWithoutLayerMask(playerLayerMask);
        
        public Vector3 GetRandomPositionInArena()
        {
            var index = Random.Range(0, _triangulation.vertices.Length);
            return NavMesh.SamplePosition(_triangulation.vertices[index], out var navMeshHit, 2f, -1)
                ? navMeshHit.position
                : Vector3.zero;
        }
        
        private void Awake() => 
            _triangulation = NavMesh.CalculateTriangulation();
        
        private Vector3 GetRandomPositionInArenaWithoutLayerMask(LayerMask layerMask)
        {
            const float radius = 1f;
            const float maxAttempts = 3;

            var colliders = new Collider[1];
            Vector3 position;
            int countCollider;
            var attempts = 0;
            do
            {
                attempts++;
                position = GetRandomPositionInArena();
                countCollider = Physics.OverlapSphereNonAlloc(position, radius, colliders, layerMask);
            } while (attempts <= maxAttempts && countCollider > 0);

            return position;
        }
    }
}