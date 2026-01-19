using System;
using UnityEngine;

namespace MCHS.Scripts
{
    public class ActiveWhenStart : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;

        private void Start()
        {
            _gameObject.SetActive(true);
        }
    }
}