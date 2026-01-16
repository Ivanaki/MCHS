using MyUtils;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

namespace MySteamVR.Linear
{
    public class MyLinearDrive : LinearDrive, IStartSettings
    {
        [SerializeField] private Transform _basePosition;
       
        [SerializeField] private float _centerValue;
        
        
        public UnityEvent _attach;
        public UnityEvent _detech;
        
        
        [SerializeField] private UnityEvent _pressed;
        [SerializeField] private UnityEvent _unPressed;
        [SerializeField] private UnityEvent _attachedUpdate;

        [SerializeField] private bool _isWithModes;
        [SerializeField] private float[] _modesValues;
        
        [SerializeField] private IntEvent _pressedWithMode = new IntEvent();


        protected override void Start()
        {
            base.Start();
            
            if (_isWithModes)
            {
                if (_modesValues.Length < 2)
                {
                    Debug.LogError("modes need be more 1");
                }
            }
            
            SetStartSettings();
            
            interactable.onDetachedFromHand += Detech;
            interactable.onAttachedToHand += Attach;
        }

        private void Detech(Hand hand)
        {
            _detech.Invoke();
        }

        private void Attach(Hand hand)
        {
            _attach.Invoke();
        }
        
        public void SetStartSettings()
        {
            SetPosition(_basePosition);
        }

        protected override void OnDetachedFromHand(Hand hand)
        {
            base.OnDetachedFromHand(hand);
            
            if (!_isWithModes)
            {
                if (linearMapping.value >= _centerValue)
                {
                    _pressed.Invoke();
                }
                else
                {
                    _unPressed.Invoke();
                }
            }
            else
            {
                var mode = 0;

                for (int i = 0; i < _modesValues.Length - 1; i++)
                {
                    if (linearMapping.value >= _modesValues[i] && linearMapping.value < _modesValues[i + 1])
                    {
                        mode = i + 1;
                        break;
                    }
                }

                /*if (mode == _modeUnPressed)
                {
                    _unPressed.Invoke();
                }
                if (mode == _modePressed)
                {
                    _pressed.Invoke();
                }*/
                
                _pressedWithMode.Invoke(mode);
            }
        }

        protected override void HandAttachedUpdate(Hand hand)
        {
            base.HandAttachedUpdate(hand);
            
            _attachedUpdate.Invoke();
        }

        public void SetPosition(Transform pos)
        {
            mappingChangeRate = 0f;
            initialMappingOffset = 0f;
            linearMapping.value = Mathf.Clamp01(CalculateLinearMapping( pos ) );
            transform.position = Vector3.Lerp( startPosition.position, endPosition.position, linearMapping.value );
        }
    }
}