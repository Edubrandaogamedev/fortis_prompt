using System;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] 
    private Renderer _renderer;

    [SerializeField] 
    private Color _color;

    private void Awake()
    {
        SetColor(_color);
    }

    private void SetColor(Color color)
    {
        _renderer.material.color = _color;        
    }
}
