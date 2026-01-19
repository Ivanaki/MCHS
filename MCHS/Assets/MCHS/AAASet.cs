using UnityEngine;

namespace MCHS
{
    public class AAASet : MonoBehaviour
    {
        [SerializeField] private Transform _downTransform;

        public void Set()
        {
            _downTransform.SetParent(transform);
            _downTransform.localPosition = Vector3.zero;
            _downTransform.localRotation = Quaternion.identity;
        }
    }
}