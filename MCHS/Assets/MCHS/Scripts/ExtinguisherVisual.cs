using System;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace MCHS.Scripts
{
    [RequireComponent(typeof(Interactable))]
    [SelectionBase]
    public class ExtinguisherVisual : MonoBehaviour
    {
        [SerializeField] private Transform _offTransform;
        [SerializeField] private Transform _onTransform;
        [SerializeField] private Transform _rotatedTransform;

        [SerializeField] private Extinguisher _extinguisher;
        [SerializeField] private float _minGrip = 0.8f;
        
        private Interactable _interactable;
        
        private SteamVR_Action_Single _gripSqueeze = SteamVR_Input.GetAction<SteamVR_Action_Single>("Squeeze");

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
        }
        
        private void FixedUpdate()
        {
            if (_interactable.attachedToHand)
            {
                var grip = _gripSqueeze.GetAxis(_interactable.attachedToHand.handType);
                _rotatedTransform.localRotation = Quaternion.Lerp(_offTransform.localRotation, _onTransform.localRotation, grip);
                
                _extinguisher.enabled = grip >= _minGrip;
            }
            else
            {
                _extinguisher.enabled = false;
                _rotatedTransform.localRotation = _offTransform.localRotation;
            }
        }
    }
}