using UnityEngine;
using UtilityModule.Service;

namespace Modules.GameFlowModule
{
    public class PauseService : IService
    {
        private float _lastTimeScale;
        
        public void PauseGameByTimeScale()
        {
            _lastTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        public void UnPauseGameByTimeScale()
        {
            Time.timeScale = _lastTimeScale;
        }
    }
}