using System;
using R3;
using UnityEngine;

namespace MCHS.Scripts.Quests
{
    public class QuestSystem : MonoBehaviour
    {
        public static QuestSystem instance;

        private string[] Quests = 
        {
            "Взять оборудование в пожарной машине", //0
            "Найти вход в дом", //1
            "Найти первого пострадавшего (Не забыть отметить все опасные места)", //2 Отмечать опасные места лазером
            "Определить состояние пострадавшего", //3
            "Обезопасить зону возле пострадавшего", //4
            "Выдать пострадавшему противогаз", //5
            "Оказать базовую помощь пострадавшему", //5
            "Вывести пострадавшего из горящего здания", //6
            "Вывести остальных пострадавших", //7
            "Передать скорой информацию" //8
        };
        
        private bool CheckQuestComplete(int index)
        {
            /*switch (index)
            {
                case 0:
                    
                    
                    
                    return true;
                default:
                    return true;
            }
            
            return false;*/
            return true;
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
                    _currentQuestSubject.Value = "Спасательная операция закончена, вернитесь к пожарной машине";
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