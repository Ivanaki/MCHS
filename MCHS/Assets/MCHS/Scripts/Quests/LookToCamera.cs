using System;
using MySteamVR;
using UnityEngine;

namespace MCHS.Scripts.Quests
{
    public class LookToCamera : MonoBehaviour
    {
        private Camera _camera;
        
        private void Start()
        {
            _camera = MyPlayer.instance.GetCamera();
        }

        private void Update()
        {
            if(_camera == null) return;
            
            transform.LookAt(_camera.transform.position);
        }
    }
}