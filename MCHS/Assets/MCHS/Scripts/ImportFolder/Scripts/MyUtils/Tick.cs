using R3;
using UnityEngine;
using UnityEngine.UI;

namespace MyUtils
{
    public class Tick : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private GameObject _tickGameObject;
        
        private ReactiveProperty<bool> _isTicking { get; } = new(false);
        public ReadOnlyReactiveProperty<bool> OnTick => _isTicking;
        
        private void Awake()
        {
            if (_button != null) _button.onClick.AddListener(Click);
            
            _tickGameObject.SetActive(false);
        }
        
        public void Click()
        {
            _isTicking.Value = !_isTicking.CurrentValue;
            _tickGameObject.SetActive(_isTicking.CurrentValue);
        }
    }
}