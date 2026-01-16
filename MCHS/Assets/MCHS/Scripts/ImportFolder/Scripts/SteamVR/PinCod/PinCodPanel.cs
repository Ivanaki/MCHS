using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace MySteamVR.PinCod
{
    public class PinCodPanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textFirst;
        [SerializeField] private TMP_Text _textSecond;

        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _correct;
        [SerializeField] private AudioClip _incorrect;
        [SerializeField] private AudioClip _pic;

        [SerializeField] private UnityEvent _pinCodComplete;
        //[SerializeField] private UnityEvent _pinCodCompleteSecond;

        private int _randomPinCodFirst;
        private int _randomPinCodSecond;

        private List<int> _queue = new List<int>();

        private void Start()
        {
            _randomPinCodFirst = Int32.Parse(Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() +
                                             Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString());
            _textFirst.text = _randomPinCodFirst.ToString();

            _randomPinCodSecond = Int32.Parse(Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString() +
                                              Random.Range(0, 9).ToString() + Random.Range(0, 9).ToString());
            _textSecond.text = _randomPinCodSecond.ToString();
        }

        public void PressNumber(int number)
        {
            _queue.Add(number);

            _source.PlayOneShot(_pic);
        }

        public void PressLater()
        {
            _source.PlayOneShot(_pic);
        }

        /*private void CheckSize()
        {
            if (_queue.Count >= 4)
            {
                Clear();
            }
        }*/

        public void PressEnter()
        {
            string cod = "";
            for (int i = 0; i < _queue.Count; i++)
            {
                cod = cod + _queue[i];
            }

            if (_randomPinCodFirst == Int32.Parse(cod) || _randomPinCodSecond == Int32.Parse(cod))
            {
                _pinCodComplete.Invoke();
                _source.PlayOneShot(_correct);
                Clear();
            }
            else
            {
                // неверный пинкод
                _source.PlayOneShot(_incorrect);
                Clear();
            }
        }

        private void Clear()
        {
            _queue.Clear();
        }

        public void ClearPinCod()
        {
            _source.PlayOneShot(_pic);
            Clear();
        }
    }
}