using System;
using System.Collections;
using System.Collections.Generic;
using SpawnModule;
using TMPro;
using UnityEngine;

public class UnitHUD : MonoBehaviour
{
    [SerializeField] 
    private SpawnController _spawnController;
    [SerializeField] 
    private TextMeshProUGUI _countText;
    private void Awake()
    {
        _spawnController.OnUnitCountChange += UpdateUnitCount;
    }

    private void UpdateUnitCount()
    {
        _countText.text = _spawnController.SpawnedUnits.Count.ToString();
    }
}
