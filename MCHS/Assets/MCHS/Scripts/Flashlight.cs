using System;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace MCHS.Scripts
{
    [RequireComponent(typeof(Interactable))]
    public class Flashlight : MonoBehaviour
    {
        [SerializeField] private GameObject _lightsParent;
        
        private SteamVR_Action_Boolean _leftTurn = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "FlashlightLeft");
        private SteamVR_Action_Boolean _rightTurn = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "FlashlightRight");
        
        private Interactable _interactable;
        private bool _state = false;

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
            
            _lightsParent.SetActive(false);
        }

        private void OnEnable()
        {
            _leftTurn.onChange += TurnLeft;
            _rightTurn.onChange += TurnRight;
        }

        private void OnDisable()
        {
            _leftTurn.onChange -= TurnLeft;
            _rightTurn.onChange -= TurnRight;
        }

        private void TurnRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (_interactable.attachedToHand != null && _interactable.attachedToHand.handType == SteamVR_Input_Sources.RightHand)
            {
                Toggle();
            }
        }

        private void TurnLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            if (_interactable.attachedToHand != null && _interactable.attachedToHand.handType == SteamVR_Input_Sources.LeftHand)
            {
                Toggle();
            }
        }

        private void Toggle()
        {
            _state = !_state;
            _lightsParent.SetActive(_state);
        }
    }
}