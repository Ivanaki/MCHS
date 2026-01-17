using System;
using R3;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace MCHS.Scripts.BackPack
{
    [RequireComponent(typeof(Throwable))]
    [RequireComponent(typeof(InteractableHoverEvents))]
    [RequireComponent(typeof(Rigidbody))]
    public class Item : MonoBehaviour
    {
        //private const float _sphereOverlapRadius = 0.3f;
        //private LayerMask _sphereOverlapsLayerMask;
        
        //[SerializeField] private float _sphereOverlapRadius = 0.3f;
        //[SerializeField] private Transform _sphereOverlapsPosition;
        //[SerializeField] private LayerMask _sphereOverlapsLayerMask;
        public bool IsNested = false;
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField, Range(0f, 1f)] public float SizeOfMaxSizeWhileInNest { get; private set; } = 1f;
        
        public Observable<Unit> OnAttachedToHand {get; private set;}
        
        public Rigidbody Rigidbody {get; private set;}
        
        public ReadOnlyReactiveProperty<bool> IsAttachedToHand => _isAttachedToHand;
        
        private readonly ReactiveProperty<bool> _isAttachedToHand = new(false);
        
        private InteractableHoverEvents _interactableHoverEvents;
        private CompositeDisposable _disposables;

        private Nest _canPutNest = null;
        
        private void Awake()
        {
            //_sphereOverlapsLayerMask = LayerMask.GetMask("Nest");
            Rigidbody = GetComponent<Rigidbody>();
            _interactableHoverEvents = GetComponentInChildren<InteractableHoverEvents>();
            OnAttachedToHand = _interactableHoverEvents.onAttachedToHand.AsObservable();
        }

        private void OnEnable()
        {
            _disposables = new CompositeDisposable();
            
            _disposables.Add(_interactableHoverEvents.onAttachedToHand.AsObservable().Subscribe(_ =>
            {
                _isAttachedToHand.Value = true;
            }));
            _disposables.Add(_interactableHoverEvents.onDetachedFromHand.AsObservable().Subscribe(_ =>
            {
                _isAttachedToHand.Value = false;
            }));
            
            _disposables.Add(_isAttachedToHand.Subscribe(value =>
            {
                if (value == false)
                {
                    //_canPutNest?.TryUnHighlight(this);
                    //_canPutNest = null;
                    
                    TryPutObjectToNest();
                }
            }));
        }

        
        /*private Nest _currentHighlightNest;
        
        //по идее этим должен заниматься скрипт на руке, но чтобы не переписывать скрипт от SteamVr, то пусть будет здесь
        private void FixedUpdate()
        {
            if (_isAttachedToHand.Value)
            {
                var colliders = Physics.OverlapSphere(_sphereOverlapsPosition.position, _sphereOverlapRadius,
                    _sphereOverlapsLayerMask);
                foreach (var collider in colliders)
                {
                    if (collider.TryGetComponent(out ColliderRedirector colliderRedirector))
                    {
                        if (colliderRedirector.TryGetComponentBase(out Nest nest))
                        {
                            
                        }
                    }
                }
            }
        }*/

        private bool TryPutObjectToNest()
        {
            /*var colliders = Physics.OverlapSphere(_sphereOverlapsPosition.position, _sphereOverlapRadius,
                _sphereOverlapsLayerMask);
            
            var minDistance = float.MaxValue;
            Nest minNest = null;
            
            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out ColliderRedirector colliderRedirector))
                {
                    if (colliderRedirector.TryGetComponentBase(out Nest nest))
                    {
                        var dist = Vector3.Distance(transform.position, nest.transform.position);
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            minNest = nest;
                        }
                    }
                }
            }

            if (minNest != null)
            {
                return minNest.TrySetItem(this);
            }*/

            if (_canPutNest != null)
            {
                _canPutNest.TrySetItem(this);
            }
            
            return false;
        }

        public void CanPut(Nest nest)
        {
            _canPutNest = nest;
        }

        public void CantPut(Nest nest)
        {
            if(_canPutNest == nest)
                _canPutNest = null;
        }
        
        private void OnDisable()
        {
            _disposables.Dispose();
        }
    }
}