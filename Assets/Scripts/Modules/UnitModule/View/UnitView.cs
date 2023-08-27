using System;
using UnitModule;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    private readonly int RUN_CONDITION = Animator.StringToHash("isRunning");

    [SerializeField] 
    private UnitController _unitController;
    [SerializeField] 
    private Animator _animator;

    private int _currentPlayingAnimation; 
    private void Update()
    {
        if (_unitController.IsMoving && !_animator.GetBool(RUN_CONDITION))
        {
            _animator.SetBool(RUN_CONDITION,_unitController.IsMoving);
        }
        else if (!_unitController.IsMoving && _animator.GetBool(RUN_CONDITION))
        {
            _animator.SetBool(RUN_CONDITION,_unitController.IsMoving);
        }
    }
}
