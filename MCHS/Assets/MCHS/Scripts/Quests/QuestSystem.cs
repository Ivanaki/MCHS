using System;
using R3;
using UnityEngine;

namespace MCHS.Scripts.Quests
{
    public class QuestSystem : MonoBehaviour
    {
        public static QuestSystem instance;

        private string[] Quests = new[]
        {
            "Возьмите предметы из машины", //0
            "", //1
            "", //2
            "", //3
            "", //4
            "", //5
            "", //6
            "", //7
            "", //8
            "", //9
            "", //10
            "", //11
        };
        
        private bool CheckQuestComplete(int index)
        {
            switch (index)
            {
                case 0:
                    
                    
                    
                    return true;
                default:
                    return true;
            }
            
            return false;
        }
        
        
        
        public ReadOnlyReactiveProperty<string> CurrentQuest => _currentQuestSubject;
        
        private readonly ReactiveProperty<string> _currentQuestSubject = new("None");
        private int _currentQuestIndex = 0;
        private bool _isEnd = false;

        private void Start()
        {
            _currentQuestSubject.Value = Quests[0];
        }


        public bool TryGoToNextQuest()
        {
            if(_isEnd) return true;

            var result = CheckQuestComplete(_currentQuestIndex);
            if (result)
            {
                _currentQuestIndex++;

                try
                {
                    var value = Quests[_currentQuestIndex];
                    _currentQuestSubject.Value = value;
                }
                catch
                {
                    _currentQuestSubject.Value = "Поздравляем, вы прошли подготовку пожарного";
                    _isEnd = true;
                }
                
            }
            
            return result || _isEnd;
        }
        
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                transform.SetParent(null);
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}