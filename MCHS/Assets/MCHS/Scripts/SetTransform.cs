using UnityEngine;

namespace MCHS.Scripts
{
    public class SetTransform : MonoBehaviour
    {
        [SerializeField] private Vector3 _vector3;
        [SerializeField] private Quaternion _quaternion;

        public void Set()
        {
            transform.position = _vector3;
            transform.rotation = _quaternion;
            //GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}