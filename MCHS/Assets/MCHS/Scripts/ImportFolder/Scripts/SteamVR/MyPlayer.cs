using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace MySteamVR
{
    [RequireComponent(typeof(Player))]
    public class MyPlayer : MonoBehaviour
    {
        [SerializeField] private Camera _playerCamera;

        [SerializeField] private Transform _rightHand;
        [SerializeField] private Transform _leftHand;
        
        public Player Player { get; private set; }
        public static MyPlayer instance { get; private set; }

        private SkinnedMeshRenderer _rightHandModel = null;
        private SkinnedMeshRenderer _leftHandModel = null;
        
        private void Awake()
        {
            Player = GetComponent<Player>();
            instance = this;
        }

        public SkinnedMeshRenderer GetRightHandModel()
        {
            if (_rightHandModel == null)
            {
                _rightHandModel = Player.rightHand.mainRenderModel.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
            }

            return _rightHandModel;
        } 
        
        public SkinnedMeshRenderer GetLeftHandModel()
        {
            if (_leftHandModel == null)
            {
                _leftHandModel = Player.rightHand.mainRenderModel.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<SkinnedMeshRenderer>();
            }

            return _leftHandModel;
        }


        public void AttachToPlayer(Transform objectToAttach)
        {
            objectToAttach.SetParent(transform);
            objectToAttach.localPosition = Vector3.zero;
            objectToAttach.localRotation = Quaternion.identity;
        }

        public bool TryAttachTo(SteamVR_Input_Sources type, Transform objectToAttach)
        {
            var parentTransform = type switch
            {
                SteamVR_Input_Sources.LeftHand => _leftHand,
                SteamVR_Input_Sources.RightHand => _rightHand,
                _ => null
            };

            if (parentTransform != null)
            {
                objectToAttach.SetParent(_leftHand);
                objectToAttach.localPosition = Vector3.zero;
                objectToAttach.localRotation = Quaternion.identity;
                return true;
            }
            
            Debug.LogError("Cannot find this type: " + type);
            return false;
        }

        public Transform GetObjectTransform(SteamVR_Input_Sources type)
        {
            return type switch
            {
                SteamVR_Input_Sources.LeftHand => _leftHand,
                SteamVR_Input_Sources.RightHand => _rightHand,
                _ => null
            };
        }

        public Camera GetCamera() => _playerCamera;
    }
}