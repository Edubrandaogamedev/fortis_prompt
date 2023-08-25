using Modules.GameFlowModule;
using MovementModule;
using UnityEngine;
using UtilityModule.Service;

namespace BootstrapModule
{
    public class GameBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            ServiceLocator.Initialize();
            ServiceLocator.Instance.BindService(new MovementService());
            ServiceLocator.Instance.BindService(new PauseService());
        }
    }
}

