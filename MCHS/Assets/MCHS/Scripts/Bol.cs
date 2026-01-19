using MCHS.Scripts.BackPack;
using R3;
using UnityEngine;

namespace MCHS.Scripts
{
    public class Bol : MonoBehaviour
    {
        public ReactiveProperty<bool> Heal = new(false);

        private void Awake()
        {
            GetComponent<SphereCollider>().enabled = false;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ColliderRedirector redirector))
            {
                if (redirector.TryGetComponentBase(out Bandage bandage))
                {
                    Heal.Value = true;
                    GetComponent<MeshRenderer>().enabled = false;
                    GetComponent<SphereCollider>().enabled = false;
                }
            }
        }

        public void StartBol()
        {
            GetComponent<SphereCollider>().enabled = true;
        }
    }
}
