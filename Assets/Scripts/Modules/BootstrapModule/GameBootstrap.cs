using Modules.UtilityModule.Debug;
using UnityEngine;
using UnityEngine.SceneManagement;
using UtilitiesModule.Service;

namespace BootstrapModule
{
    public class GameBootstrap
    {
        private const string TARGET_LOAD_SCENE = "Game";
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            ServiceLocator.Initialize();
            //Change to a loading system
            //SceneManager.LoadSceneAsync(TARGET_LOAD_SCENE);
        }
    }
}

