using R3;
using UnityEngine;

namespace MyUtils.Triggers
{
    [RequireComponent(typeof(Collider))]
    public class Trigger : MonoBehaviour
    {
        [SerializeField] private bool _suicideTrigger = false;
        [SerializeField] private string _tag;
        
        private readonly Subject<Unit> onTriggerEntered = new();
        public Observable<Unit> OnTriggerEntered => onTriggerEntered;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_tag))
            {
                onTriggerEntered.OnNext(Unit.Default);
                
                if (_suicideTrigger) Destroy(gameObject);
            }
        }
    }
}