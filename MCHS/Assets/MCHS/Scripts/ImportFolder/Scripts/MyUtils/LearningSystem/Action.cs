using R3;
using UnityEngine;

namespace MyUtils.LearningSystem
{
    public class Action : MonoBehaviour
    {
        private const string PIVOT = "PIVOT";
        private const string STRELKA = "STRELKA";

        protected Subject<bool> End { private set; get; }
        protected bool IsActive { private set; get; } = false;

        private bool _isTraining = true;
        
        private Transform _pivot, _strelkaPos;
        private Strelka _strelka;
        
        public virtual void Initialize(Strelka strelka)
        {
            //print(strelka.gameObject.name);
            
            _pivot = transform.Find(PIVOT);
            _strelkaPos = transform.Find(STRELKA);
            
            strelka.transform.SetParent(transform);
            strelka.SetPosition(_pivot, _strelkaPos);
            strelka.Deactivate();
            _strelka = strelka;
        }
        
        public virtual void StartWork(Subject<bool> start)
        {
            start.Subscribe(isTraining =>
            {
                //print(gameObject.name);
                //print(_strelka.gameObject.name);
                
                
                _isTraining = isTraining;
                if (_isTraining)
                {
                    _strelka.Activate();
                }
                
                IsActive = true;
            });
        }

        public virtual void EndWork(Subject<bool> end)
        {
            End = end;
            End.Subscribe(isTraining =>
            {
                _strelka.Deactivate();
                
                IsActive = false;
            });
        }
        
        
        public void ActionCompleted()
        {
            if(IsActive)
                End.OnNext(_isTraining);
        }
        
        private void Reset()
        {
            if (transform.Find(PIVOT) == null)
            {
                var posPivot = new GameObject(PIVOT);
                posPivot.transform.SetParent(transform);
                posPivot.transform.localPosition = Vector3.zero;
                posPivot.transform.localRotation = Quaternion.identity;
                
                var pos = new GameObject(STRELKA);
                pos.transform.SetParent(transform);
                pos.transform.localPosition = Vector3.zero;
                pos.transform.localRotation = Quaternion.identity;
            }
        }
    }
}