using System;
using System.Collections.Generic;
using CharacterModule;
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
        return parent == null ? Instantiate(unit, location, rotation) : Instantiate(unit, location, rotation, parent);
    }
}
