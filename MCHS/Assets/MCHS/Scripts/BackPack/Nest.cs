using System;
using R3;
using UnityEngine;

namespace MCHS.Scripts.BackPack
{
    public abstract class Nest : MonoBehaviour
    {
        [SerializeField] private Material _materialBase;
        [SerializeField] private Material _materialGood;
        [SerializeField] private Material _materialBad;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Transform _itemParent;
        [SerializeField] private NestTrigger _nestTrigger;
        //[SerializeField] private float _minTimeWaitUntilNewSet = 0.2f;
        
        public abstract ItemType ItemType { get; }
        
        private Item _currentItem = null;
        
        //private bool _highlightedInThisFrameFixedUpdate = false;

        private CompositeDisposable _compositeDisposable;

        private void OnEnable()
        {
            _compositeDisposable = new CompositeDisposable();
            
            _compositeDisposable.Add(_nestTrigger.onTriggerEnter.Subscribe(item =>
            {
                var result = TrySetItem(item);

                if (result == false)
                {
                    TryHighlight(item);
                    
                    item.CanPut(this);
                }
            }));
            
            _compositeDisposable.Add(_nestTrigger.onTriggerExit.Subscribe(item =>
            {
                TryUnHighlight(item);
                
                item.CantPut(this);
            }));
        }

        private void Start()
        {
            UnHighlight();
        }
        
        public bool TrySetItem(Item item)
        {
            if (item.ItemType == ItemType && _currentItem == null && item.IsAttachedToHand.CurrentValue == false)
            {
                item.Rigidbody.isKinematic = true;
                item.transform.SetParent(_itemParent);
                item.transform.localPosition = Vector3.zero;
                item.transform.localRotation = Quaternion.identity;
                
                //найдем нужный размер
                item.transform.localScale *= item.SizeOfMaxSizeWhileInNest;
                
                var lossyScale = transform.lossyScale;
                var itemLocalScale = item.transform.localScale;
                item.transform.localScale = new Vector3(itemLocalScale.x * lossyScale.x,
                    itemLocalScale.y * lossyScale.y, itemLocalScale.z * lossyScale.z);
                //
                

                item.OnAttachedToHand.Take(1).Subscribe(_ =>
                {
                    _currentItem.Rigidbody.isKinematic = false;
                    
                    //Scale
                    _currentItem.transform.localScale /= _currentItem.SizeOfMaxSizeWhileInNest;
                    
                    var lossyScaleNew = transform.lossyScale;
                    var itemLocalScaleNew = _currentItem.transform.localScale;
                    _currentItem.transform.localScale = new Vector3(itemLocalScaleNew.x / lossyScaleNew.x,
                        itemLocalScaleNew.y / lossyScaleNew.y, itemLocalScaleNew.z / lossyScaleNew.z);
                    //
                    
                    if (_currentHighlitghtedItem == _currentItem) UnHighlight();
                    
                    _currentItem = null;
                });

                if (_currentHighlitghtedItem == item) UnHighlight();
                
                
                _currentItem = item;
                
                return true;
            }
            
            return false;
        }

        private Item _currentHighlitghtedItem = null;
        
        public void TryHighlight(Item item)
        {
            if (item.IsAttachedToHand.CurrentValue && item != _currentItem)
            {
                if (_currentHighlitghtedItem != null)
                {
                    Debug.LogError("_currentHighlitghtedItem != null");
                    _currentHighlitghtedItem = null;
                }
                
                if (item.ItemType == ItemType && _currentItem == null)
                {
                    _meshRenderer.material =  _materialGood;
                }
                else
                {
                    _meshRenderer.material =  _materialBad;
                }

                /*item.IsAttachedToHand.Take(1).Subscribe(value =>
                {
                    if (value == false)
                    {
                        if (_currentHighlitghtedItem == item)
                        {
                            TryUnHighlight(item);
                        }
                    }
                });*/
                
                _currentHighlitghtedItem = item;
            }
        }

        public bool TryUnHighlight(Item item)
        {
            if (item == _currentHighlitghtedItem)
            {
                _meshRenderer.material = _materialBase;
                _currentHighlitghtedItem = null;
                return true;
            }
            return false;
        }

        private void UnHighlight()
        {
            _meshRenderer.material = _materialBase;
        }

        private void OnDisable()
        {
            _compositeDisposable?.Dispose();
        }
    }
}