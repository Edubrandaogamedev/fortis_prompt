using UnityEngine;

namespace MovementModule
{
    [CreateAssetMenu(menuName = "Movement/Settings/NavMesh", fileName = "NewNavMeshMovementSettings")]
    public class NavMeshMovementSettings : ScriptableObject
    {
        [SerializeField] 
        private float _maxSearchDistance;

        [SerializeField] 
        private float _movementRange;

        [SerializeField] 
        private Vector2 _minimumDistance;

        [SerializeField] 
        private int _searchRetriesCount;

        public float MaxSearchDistance => _maxSearchDistance;
        public int SearchRetriesCount => _searchRetriesCount;

        public float MovementRange => _movementRange;

        public Vector2 MinimumDistance => _minimumDistance;
    }
}