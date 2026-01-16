using R3;
using UnityEngine;

namespace MySteamVR
{
    [RequireComponent(typeof(Collider))]
    public class FingerButton : MonoBehaviour
    {
        private const float DelayTime = 0.4f;
        
        [HideInInspector] public Subject<Unit> onButtonPressed = new();

        private float _time = 0f;
        
        private void OnTriggerEnter(Collider other)
        {
            //print(other.name);
            if (other.CompareTag("Finger") && _time > DelayTime)
            {
                Debug.Log("finger");
                onButtonPressed.OnNext(Unit.Default);
                _time = 0f;
            }
        }

        private void Update()
        {
            _time += Time.deltaTime;
        }
    }
}