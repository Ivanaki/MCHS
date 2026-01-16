using MySteamVR;
using UnityEngine;

namespace MCHS.Scripts.BackPack
{
    public class BackPackAttacher : MonoBehaviour
    {
        public static BackPackAttacher Instance;

        [SerializeField] private Transform _moveableObject;
        //[SerializeField] private Vector3 _offset = new Vector3(0f, -1f, 0f);
        
        private Camera _cameraVr;
        
        private void Start()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            MyPlayer.instance.AttachToPlayer(transform);
            
            _cameraVr = MyPlayer.instance.GetCamera();
        }

        private void FixedUpdate()
        {
            _moveableObject.localPosition = _cameraVr.transform.localPosition/* + _offset*/;
            _moveableObject.localEulerAngles = new Vector3(_moveableObject.localEulerAngles.x,
                _cameraVr.transform.localEulerAngles.y, _moveableObject.localEulerAngles.z);
        }
    }
}