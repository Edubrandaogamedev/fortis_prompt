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
        }
    }
}

