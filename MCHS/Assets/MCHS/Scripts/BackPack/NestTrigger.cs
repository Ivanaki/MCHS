using R3;
using UnityEngine;

namespace MCHS.Scripts.BackPack
{
    public class NestTrigger : MonoBehaviour
    {
        public Observable<Item> onTriggerEnter => _onTriggerEnter;
        public Observable<Item> onTriggerExit => _onTriggerExit;
        
        private readonly Subject<Item> _onTriggerEnter = new();
        private readonly Subject<Item> _onTriggerExit = new();
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ColliderRedirector colliderRedirector))
            {
                if (colliderRedirector.TryGetComponentBase<Item>(out var item))
                {
                    _onTriggerEnter.OnNext(item);
                }
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ColliderRedirector colliderRedirector))
            {
                if (colliderRedirector.TryGetComponentBase<Item>(out var item))
                {
                    _onTriggerExit.OnNext(item);
                }
            }
        }
    }
}