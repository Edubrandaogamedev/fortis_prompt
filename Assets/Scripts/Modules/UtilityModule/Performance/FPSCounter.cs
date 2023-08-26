using System.Collections;
using UnityEngine;

namespace UtilityModule.Performance
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] 
        private float _frequency;
        private int fps;
        private bool _isInitialized;
        
        
        private WaitForSecondsRealtime waitForFrequency;

        public float Frequency => _frequency;

        public bool IsInitialized => _isInitialized;

        public void Initialize()
        {
            waitForFrequency = new WaitForSecondsRealtime(_frequency);
            _isInitialized = true;
            StartCoroutine(CalculateFPS());
        }

        public void Stop()
        {
            _isInitialized = false;
        }
        
        public int GetFPS()
        {
            return fps;
        }
        
        private IEnumerator CalculateFPS()
        {
            while(_isInitialized)
            {
                // Capture frame-per-second
                var lastFrameCount = Time.frameCount;
                var lastTime = Time.realtimeSinceStartup;
                yield return waitForFrequency;
                var timeSpan = Time.realtimeSinceStartup - lastTime;
                var frameCount = Time.frameCount - lastFrameCount;
                fps = (int)(frameCount / timeSpan);
            }
        }
    }
}