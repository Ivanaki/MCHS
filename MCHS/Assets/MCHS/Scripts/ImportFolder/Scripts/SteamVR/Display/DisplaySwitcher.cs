using R3;
using UnityEngine;

namespace MySteamVR.Display
{
    public class DisplaySwitcher : MonoBehaviour
    {
        [SerializeField] private FingerButton _leftButton, _rightButton;

        [SerializeField] private Camera[] _cameras;
        private int _cameraIndex = 0;
        
        private CompositeDisposable _compositeDisposable = new();

        private void Start()
        {
            foreach (var camera in _cameras)
            {
                camera.gameObject.SetActive(false);
            }

            _cameras[_cameraIndex].gameObject.SetActive(true);

            var disp1 = _leftButton.onButtonPressed.Subscribe(_ => SwitchCamera(true));
            var disp2 = _rightButton.onButtonPressed.Subscribe(_ => SwitchCamera(false));
            _compositeDisposable.Add(disp1);
            _compositeDisposable.Add(disp2);
        }

        private void SwitchCamera(bool isRight)
        {
            _cameras[_cameraIndex].gameObject.SetActive(false);
            _cameraIndex = ((_cameraIndex + (isRight? 1 : -1))+ (_cameras.Length)) % (_cameras.Length);
            print("Camera index: " + _cameraIndex);
            _cameras[_cameraIndex].gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _compositeDisposable.Dispose();
        }
    }
}