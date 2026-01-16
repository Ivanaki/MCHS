using R3;
using UnityEngine;

namespace MCHS.Scripts
{
    public abstract class TriggerEnter<T> : MonoBehaviour
    {
        public Observable<T> OnCollisionEnter => _onCollisionEnter;
        private Subject<T> _onCollisionEnter = new();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out T item))
            {
                _onCollisionEnter.OnNext(item);
            }
        }
    }
}