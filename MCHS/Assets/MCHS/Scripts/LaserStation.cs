using System;
using UnityEngine;

namespace MCHS.Scripts
{
    public class LaserStation : MonoBehaviour
    {
        [SerializeField] private float _needTime = 2f;
        [SerializeField] private GameObject _objectToSetActive;

        private bool _ended = false;
        
        private bool _active = false;
        private float _time = 0f;

        private void Start()
        {
            _objectToSetActive.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LaserRay") && _ended == false)
            {
                _time = Time.time;
                _active = true;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("LaserRay") && _ended == false)
            {
                _active = false;
            }
        }

        private void FixedUpdate()
        {
            if (_active && _ended == false)
            {
                if (Time.time - _time >= _needTime)
                {
                    _ended = true;
                    _objectToSetActive.SetActive(true);
                }
            }
        }
    }
}