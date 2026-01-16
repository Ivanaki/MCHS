using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyUtils
{
    [RequireComponent(typeof(Image))]
    public class Navedenie : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Color _enterColor = new Color(1, 1, 1, 0.03529412f);
        [SerializeField] private Color _exitColor = new Color(0, 0, 0, 0);

        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
            _image.color = _exitColor;
        }

        public void OnPointerEnter(PointerEventData eventData = null)
        {
            if (_image) _image.color = _enterColor;
        }

        public void OnPointerExit(PointerEventData eventData = null)
        {
            if (_image) _image.color = _exitColor;
        }

        private void OnDisable()
        {
            OnPointerExit();
        }
    }
}