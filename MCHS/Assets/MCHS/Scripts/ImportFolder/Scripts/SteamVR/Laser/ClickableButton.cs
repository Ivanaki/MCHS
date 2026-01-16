using UnityEngine;
using UnityEngine.UI;

namespace MySteamVR.Laser
{
    [RequireComponent(typeof(Button), typeof(Image))]
    public class ClickableButton : MonoBehaviour, IPointerIn, IPointerOut
    {
        [SerializeField] private Color _enterColor = new Color(1, 1, 1, 0.466667f);
        [SerializeField] private Color _exitColor = new Color(0, 0, 0, 0);

        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            _image.color = _exitColor;
        }

        public void OnPointerIn()
        {
            if (_image) _image.color = _enterColor;
        }

        public void OnPointerOut()
        {
            if (_image) _image.color = _exitColor;
        }

        private void OnDisable()
        {
            OnPointerOut();
        }
    }
}