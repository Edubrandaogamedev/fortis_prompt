using System.Collections.Generic;
using CharacterModule;
using UnityEngine;
using UnityEngine.Serialization;

namespace SpawnModule
{
    [CreateAssetMenu(menuName = "Character/Spawn", fileName = "NewSpawnableCharacterData")]
    public class SpawnableUnitData : ScriptableObject
    {
        [SerializeField] 
        private List<UnitController> _unitPrefabs;

        public List<UnitController> UnitPrefabs => _unitPrefabs;
    }
}