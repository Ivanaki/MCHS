using MyUtils;
using UnityEngine;
using UnityEngine.Events;

namespace MySteamVR.Circular
{
    public class MyCircularDrive : CircularDrive, IStartSettings
    {
        [Space]
        [SerializeField] private UnityEvent _grabOff;
        [Space]
        
        [SerializeField] private float _centerValue;
        [SerializeField] private UnityEvent _pressed;
        [SerializeField] private UnityEvent _unPressed;

        [SerializeField] private bool _isWithModes;
        [SerializeField] private float[] _modesValues;
        
        [SerializeField] private IntEvent _pressedWithMode = new IntEvent();

        /*private void Start()
        {
            if (_isWithModes)
            {
                if (_modesValues.Length < 2)
                {
                    Debug.LogError("modes need be more 1");
                }
            }
        }*/
        
        public void SetStartSettings()
        {
            outAngle = Mathf.Clamp( startAngle, minAngle, maxAngle );
            UpdateAll();
        }

        /*
         ///////////////////////////////////////////////
         protected override void GrabOff()
        {
            _grabOff.Invoke();
            
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
                
                _pressedWithMode.Invoke(mode);
            } 
        }*/
        
        /*protected override void HandHoverUpdate(Hand hand)
        {
            base.HandHoverUpdate(hand);
            
            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabbingWithType(grabbedWithType) == false;

            if (grabbedWithType == GrabTypes.None && startingGrabType != GrabTypes.None)
            {
                
            }
            else if (grabbedWithType != GrabTypes.None && isGrabEnding)
            {
                print("endcircular");
            }
        }*/

        /*protected override void UpdateLinearMapping()
        {
            base.UpdateLinearMapping();
            
            if (!_isWithModes)
            {
                if (linearMapping.value > 0.95f)
                {
                    _withOutModesPressed.Invoke();
                }
            }
            else
            {
                for (int i = 0; i < _modesValues.Length -1; i++)
                {
                    
                }
            }
        }*/
        
        public void SetAngle(float angle)
        {
            if (angle <= maxAngle && angle >= minAngle && limited)
            {
                outAngle = Mathf.Clamp( angle, minAngle, maxAngle );
                UpdateAll();
            }
            else
            {
                Debug.LogError("angle <= maxAngle && angle >= minAngle && limited");
            }
        }
    }
}
