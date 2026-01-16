using UnityEngine;

namespace MyUtils
{
    public class SceneLiveTime : MonoBehaviour
    {
        private float _sceneStartLive;

        public float GetSceneTimeLive() => Time.time - _sceneStartLive;

        private void Start()
        {
            _sceneStartLive = Time.time;
        }
    }
}