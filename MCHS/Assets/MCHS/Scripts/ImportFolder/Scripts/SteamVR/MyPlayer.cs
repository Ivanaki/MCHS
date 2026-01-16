using UnityEngine;
using Valve.VR.InteractionSystem;

namespace MySteamVR
{
    [RequireComponent(typeof(Player))]
    public class MyPlayer : MonoBehaviour
    {
        [SerializeField] private Camera _playerCamera;
        
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

        public Camera GetCamera() => _playerCamera;
    }
}