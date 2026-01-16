using TMPro;
using UnityEngine;

namespace MyUtils
{
    public class TextSystem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        
        public void ApplyText(string text)
        {
            _text.text = text;
        }
    }
}