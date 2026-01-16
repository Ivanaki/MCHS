using UnityEngine;
using UnityEngine.Events;

namespace MyUtils.Triggers
{
    public class TouchToVoid : MonoBehaviour
    {
        [SerializeField] private string _tagName;

        [SerializeField] private UnityEvent _pressed;
        [SerializeField] private UnityEvent _unPressed;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(_tagName))
            {
                _pressed.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(_tagName))
            {
                _unPressed.Invoke();
            }
        }
    }
}