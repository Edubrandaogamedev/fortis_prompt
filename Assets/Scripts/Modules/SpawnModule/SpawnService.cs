using System;
using System.Collections.Generic;
using CharacterModule;
using Modules.UtilityModule.Debug;
using SpawnModule;
using UnityEngine;
using UtilitiesModule.Service;

public class SpawnService : MonoBehaviour,IService
{
    [SerializeField] 
    private SpawnableUnitData _data;


    private void Awake()
    {
        ServiceLocator.Instance.BindService(this);
    }

    public UnitController SpawnUnit(string unitKey, Vector3 location, Quaternion rotation = default, Transform parent = null)
    {
        UnitController unit = _data.UnitPrefabs.Find(unit => unit.Key == unitKey);
        if (unit == null)
        {
            ServiceLocator.Instance.Get<DebugService>().ShowLog("{0} not found, check if the prefab has the correct key or if the unit is on data",DebugLogType.Warning,unitKey);
        }
        return parent == null ? Instantiate(unit, location, rotation) : Instantiate(unit, location, rotation, parent);
    }
}
