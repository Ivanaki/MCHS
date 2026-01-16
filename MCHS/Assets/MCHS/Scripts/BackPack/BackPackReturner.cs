using R3;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace MCHS.Scripts.BackPack
{
    [RequireComponent(typeof(InteractableHoverEvents))]
    public class BackPackReturner : MonoBehaviour
    {
        [SerializeField] private float _lerpCoofPos = 10f;
        [SerializeField] private float _lerpCoofRot = 10f;
        [SerializeField] private float _minLerpDistance = 0.1f;
        [SerializeField] private float _minLerpAngele = 2f;

        [SerializeField] private GameObject _modelToDisableWhileOn;
        [SerializeField] private GameObject _nests;
        
        private InteractableHoverEvents _interactableHoverEvents;
        private CompositeDisposable _disposables;
        private Transform _parent;
        private ReactiveProperty<bool> _isLerpingPos = new(false);
        private ReactiveProperty<bool> _isLerpingRot = new(false);

        private void Awake()
        {
            _interactableHoverEvents = GetComponent<InteractableHoverEvents>();
            _parent = transform.parent;
            _modelToDisableWhileOn.SetActive(false);
            _nests.SetActive(false);
        }

        private void OnEnable()
        {
            _disposables = new CompositeDisposable();
            
            _disposables.Add(_interactableHoverEvents.onDetachedFromHand.AsObservable().Subscribe(_ =>
            {
                transform.SetParent(_parent);
                _isLerpingPos.Value = true;
                _isLerpingRot.Value = true;
            }));
            
            _disposables.Add(_interactableHoverEvents.onAttachedToHand.AsObservable().Subscribe(_ =>
            {
                _isLerpingPos.Value = false;
                _isLerpingRot.Value = false;
                _modelToDisableWhileOn.SetActive(true);
                _nests.SetActive(true);
            }));
        }

        private void Update()
        {
            if (_isLerpingPos.Value)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero,
                    Time.deltaTime * _lerpCoofPos);

                if (Vector3.Distance(transform.localPosition, Vector3.zero) < _minLerpDistance)
                {
                    transform.localPosition = Vector3.zero;
                    _isLerpingPos.Value = false;
                    _modelToDisableWhileOn.SetActive(false);
                    _nests.SetActive(false);
                }
            }

            if (_isLerpingRot.Value)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, 
                    Time.deltaTime * _lerpCoofRot);
                
                if (Quaternion.Angle(transform.localRotation, Quaternion.identity) < _minLerpAngele)
                {
                    transform.localRotation = Quaternion.identity;
                    _isLerpingRot.Value = false;
                }
            }
        }
        
        private void OnDisable()
        {
            _disposables.Dispose();
        }
    }
}