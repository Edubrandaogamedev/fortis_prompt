using System;
using UnityEngine;

namespace Modules.UtilityModule.Debug
{
    [CreateAssetMenu(menuName = "Debug/Settings", fileName = "NewDebugSettings")]
    public class DebugSettings : ScriptableObject
    {
        [SerializeField] 
        private bool _allowLogs;

        [SerializeField] 
        private DebugLogType _logsToShow;

        public bool AllowLogs => _allowLogs;

        public DebugLogType LogsToShow => _logsToShow;
    }
}