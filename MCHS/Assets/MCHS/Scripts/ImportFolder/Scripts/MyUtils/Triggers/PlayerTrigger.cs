using R3;
using UnityEngine;

namespace MyUtils.Triggers
{
    [RequireComponent(typeof(Collider))]
    public class PlayerTrigger : MonoBehaviour
    {
        private readonly Subject<Unit> _trigger = new();
        public Observable<Unit> Triggered => _trigger;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                _trigger.OnNext(Unit.Default);
                gameObject.SetActive(false);
            }
        }
    }
}