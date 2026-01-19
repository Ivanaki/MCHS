using System;
using UnityEngine;

namespace MCHS.Scripts
{
    public class ScaleSin : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _offset;
        [SerializeField] private float _cosLarge = 1f;

        private Vector3 _scale;
        
        private void Awake()
        {
            _scale = transform.localScale;
        }

        private void FixedUpdate()
        {
            transform.localScale = _scale * (Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad * _speed * Time.time) * _cosLarge) + _offset);
        }
    }
}