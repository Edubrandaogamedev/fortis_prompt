using System;
using UnityEngine;
using UtilitiesModule.Service;

namespace Modules.UtilityModule.Debug
{
    [Flags]
    public enum DebugLogType
    {
        Nothing = 0x1,
        Normal = 0x2,
        Warning = 0x4,
        Error = 0x8,
    }
    
    public class DebugService : MonoBehaviour,IService
    {
        [SerializeField] 
        private DebugSettings _settings;

        public void Awake()
        {
            ServiceLocator.Instance.BindService(this);
        }

        public void ShowLog(string message, DebugLogType type, params object[] parameters)
        {
            if (!_settings.AllowLogs || !_settings.LogsToShow.HasFlag(type))
            {
                return;
            }
            string formattedMessage = string.Format(message, parameters);
            switch (type)
            {
                case DebugLogType.Warning:
                    UnityEngine.Debug.LogWarning(formattedMessage);
                    break;
                case DebugLogType.Error:
                    UnityEngine.Debug.LogError(formattedMessage);
                    break;
                default:
                    UnityEngine.Debug.Log(formattedMessage);
                    break;
            }
        }
    }
}