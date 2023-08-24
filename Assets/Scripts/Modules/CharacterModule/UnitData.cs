using UnityEngine;

namespace CharacterModule
{
    [CreateAssetMenu(menuName = "Unit/Data", fileName = "NewUnitData")]
    public class UnitData : ScriptableObject
    {
        [SerializeField] 
        private string _key;
        [SerializeField] 
        private float _invincibilityTime;
        public string Key => _key;
        public float InvincibilityTime => _invincibilityTime;
    }
}