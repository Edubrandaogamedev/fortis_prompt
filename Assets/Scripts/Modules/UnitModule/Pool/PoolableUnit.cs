using System;
using UtilityModule.Pooling;

namespace UnitModule
{
    public class PoolableUnit : PoolableItem
    {
        private UnitController _unitController;

        private void Awake()
        {
            _unitController = GetComponent<UnitController>();
            _unitController.OnDie += OnDie;
        }

        private void OnDie(UnitController unit)
        {
            OnRelease();
        }
    }
}