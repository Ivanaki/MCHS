using MySteamVR;
using UnityEngine;

namespace MCHS.Scripts
{
    public class ScaleCameraDistance : MonoBehaviour
    {
        [SerializeField] private float _baseScaleCameraDistance = 5f;
        
        private Camera _camera;
        private Vector3 _baseScale;
        
        private void Start()
        {
            _camera = MyPlayer.instance.GetCamera();
            _baseScale = transform.localScale;
        }

        private void Update()
        {
            if(_camera == null) return;
            
            var dist = Vector3.Distance(transform.position, _camera.transform.position);
            var coof = _baseScaleCameraDistance * dist;
            
            transform.localScale = _baseScale * coof;
        }
    }
}