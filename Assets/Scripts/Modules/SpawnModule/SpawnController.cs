using System;
using SpawnModule;
using UnityEngine;
using UtilitiesModule.Service;

public class SpawnController : MonoBehaviour
{
    [SerializeField] 
    private string _characterKey;
    private SpawnService _spawnService;
    
    public void Start()
    {
        _spawnService = ServiceLocator.Instance.Get<SpawnService>();
        _spawnService.SpawnUnit(_characterKey, transform.position);
    }
}
