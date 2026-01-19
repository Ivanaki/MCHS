using System;
using UnityEngine;

namespace MCHS.Scripts
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<EnemyAnimator>().StartEnemy();
        }
    }
}