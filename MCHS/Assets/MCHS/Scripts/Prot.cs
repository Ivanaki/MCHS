using MCHS.Scripts.BackPack;
using UnityEngine;

namespace MCHS.Scripts
{
    public class Prot : MonoBehaviour
    {
        [SerializeField] private GameObject _protivpgaz;

        private void Start()
        {
            _protivpgaz.SetActive(false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ColliderRedirector colliderRedirector))
            {
                if (colliderRedirector.TryGetComponentBase<Protivpgaz>(out var protivpgaz))
                {
                    GetComponent<SphereCollider>().enabled = false;
                    Destroy(protivpgaz.gameObject);
                    _protivpgaz.SetActive(true);
                }
            }
        }
    }
}