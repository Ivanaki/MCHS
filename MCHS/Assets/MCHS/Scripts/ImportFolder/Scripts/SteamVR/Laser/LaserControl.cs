using UnityEngine;

namespace MySteamVR.Laser
{
    [RequireComponent(typeof(HandLaser))]
    public class LaserControl : MonoBehaviour
    {
        public static LaserControl instance;

        private GameObject _laser;
        private HandLaser _handLaser;

        [SerializeField] private LayerMask _laserActiveLayers;
        [SerializeField] private float _laserDistance = 30f;

        private void Start()
        {
            _handLaser = GetComponent<HandLaser>();
            
            SearchLaser();
            
            if (instance == null)
            {
                instance = this;
                ActivateLaser();
            }
            else
            {
                DeactivateLaser();
                this.enabled = false;
            }
        }

        private void Update()
        {
            Ray raycast = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(raycast, out hit, _laserDistance, _laserActiveLayers))
            {
                ActivateLaser();
            }
            else
            {
                DeactivateLaser();
            }
        }

        private void SearchLaser()
        {
            foreach (Transform child in transform)
            {
                if (child.name == "New Game Object")
                {
                    _laser = child.gameObject;
                    break;
                }
            }
        }

        public void ActivateLaser()
        {
            _laser.SetActive(true);
            _handLaser.enabled = true;
        }

        public void DeactivateLaser()
        {
            _laser.SetActive(false);
            _handLaser.enabled = false;
        }
    }
}