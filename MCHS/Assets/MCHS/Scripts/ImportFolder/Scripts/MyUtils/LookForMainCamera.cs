using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyUtils
{
    public class LookForMainCamera : MonoBehaviour
    {
        private Transform _camera;

        private void Start()
        {
            _camera = Camera.main.transform;
        }

        private void Update()
        {
            transform.LookAt(_camera);
        }
    }
}