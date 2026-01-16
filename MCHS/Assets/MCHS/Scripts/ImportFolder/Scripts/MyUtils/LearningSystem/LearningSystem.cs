using MyUtils;
using MyUtils.Music;
using UnityEngine;
using R3;

namespace MyUtils.LearningSystem
{
    public class LearningSystem : MonoBehaviour
    {
        [SerializeField] private int _startingNumber = 0;
        [SerializeField] private bool _isTest = false;
        private Subject<bool> _test;
        
        [SerializeField] private Strelka _strelkaPrefab;
        //[SerializeField] private AudioSystem _audioSystem;
        [SerializeField] private TextSystem _textSystem;
        
        [SerializeField] private Data[] _datas;
        
        private Subject<bool> _finish = new();
        private Subject<bool> _start = new();
        
        public Subject<bool> StartConstructor(bool isTraining)
        {
            if(!_isInitialized) Debug.LogError("learningSystem not initialized, first invoke InitTraining()");
            
            if(_isTest)
                _test.OnNext(true);
            else
                _start.OnNext(isTraining);
            
            return _finish;
        }

        private bool _isInitialized = false;
        public void InitTraining()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                Init();
            }
        }
        
        private void Init()
        {
            Subject<bool> test = new();
            
            
            
            for (int i = 0; i < _datas.Length; i++)
            {
                var strelka = Instantiate(_strelkaPrefab);
                strelka.gameObject.name = "Strelka: " + i.ToString();
                
                
                
                var joined = new Subject<bool>();
                //Debug.LogWarning(i);
                //Debug.LogWarning(i-positionsOffset);
                _datas[i].Action.Initialize(strelka);
                
                if(i!=0) _datas[i].Action.StartWork(joined);
                if(i!=0) _datas[i-1].Action.EndWork(joined);

                var index = i;
                if (i != 0) joined.Subscribe(isLearn => { if(isLearn) AudioPlayer.Play(_datas[index].AudioClip); });
                if (i != 0) joined.Subscribe(isLearn => { if(isLearn) _textSystem.ApplyText(_datas[index].Text); });

                if (i == _startingNumber)
                {
                    test = joined;
                }
            }
            
            _start.Subscribe(isLearn => { if(isLearn) AudioPlayer.Play(_datas[0].AudioClip); });
            _start.Subscribe(isLearn => { if(isLearn) _textSystem.ApplyText(_datas[0].Text); });
            
            _datas[0].Action.StartWork(_start);
            _datas[^1].Action.EndWork(_finish);
            //
            //test.OnNext(true);
            _test = test;
            
            _finish.Subscribe(_ => print("Done"));
        }
    }
}