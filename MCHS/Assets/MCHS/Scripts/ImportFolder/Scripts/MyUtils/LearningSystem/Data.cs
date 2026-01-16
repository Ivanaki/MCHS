using System;
using UnityEngine;

namespace MyUtils.LearningSystem
{
    [Serializable]
    public class Data
    {
        public Action Action;
        public AudioClip AudioClip;
        [TextArea] public string Text;
    }
}