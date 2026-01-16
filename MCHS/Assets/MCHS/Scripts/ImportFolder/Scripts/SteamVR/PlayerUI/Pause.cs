using UnityEngine;
using Valve.VR;

namespace MySteamVR.PlayerUI
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] private GameObject _menu;
        private SteamVR_Action_Boolean _pause = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Pause");

        private bool flag = false;
        
        private void Start()
        {
            //_menu.SetActive(flag);
        }
        
        private void Update()
        {
            if (_pause.changed)
            {
                print("Changing pause state");
                flag = !flag;
                _menu.SetActive(flag);
            }
        }

        public void Continue()
        {
            flag = false;
            _menu.SetActive(flag);
        }
    }
}