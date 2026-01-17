using System;
using System.Collections;
using MySteamVR;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace MCHS.Scripts.Quests
{
    public class LeftHandQuestsView : MonoBehaviour
    {
        private SteamVR_Action_Boolean _toggleQuests = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("default", "ToggleQuests");
        
        private CompositeDisposable _disposables;

        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private UIElement _uiElement;
        [SerializeField] private Canvas _toggleObject;

        private Transform _hand;

        private bool _toggle = true;
        
        private void OnEnable()
        {
            _disposables = new CompositeDisposable();
            
            _disposables.Add(QuestSystem.instance.CurrentQuest.Subscribe(text =>
            {
                _tmpText.text = text;
            }));
            
            
            _disposables.Add(_uiElement.onHandClick.AsObservable().Subscribe(_ =>
            {
                var result = QuestSystem.instance.TryGoToNextQuest();
                if (result == false)
                {
                    StartCoroutine(ShowError());
                }
            }));

            _toggleQuests.onChange += ButtonPressed;
        }

        private void ButtonPressed(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
        {
            Toggle();
        }

        private IEnumerator ShowError()
        {
            _tmpText.text = "Сначала выполни квест";
            _tmpText.color = Color.red;
            
            yield return new WaitForSeconds(2f);
            
            _tmpText.color = Color.white;
            _tmpText.text = QuestSystem.instance.CurrentQuest.CurrentValue;
        }
        
        private void Start()
        {
            _toggleObject.gameObject.SetActive(_toggle);
            _hand = MyPlayer.instance.GetObjectTransform(SteamVR_Input_Sources.LeftHand);
        }

        private void Toggle()
        {
            _toggle = !_toggle;
            _toggleObject.gameObject.SetActive(_toggle);
        }
        
        private void Update()
        {
            transform.position = _hand.position;
        }
        
        private void OnDisable()
        {
            _disposables?.Dispose();
            _toggleQuests.onChange -= ButtonPressed;
        }
    }
}