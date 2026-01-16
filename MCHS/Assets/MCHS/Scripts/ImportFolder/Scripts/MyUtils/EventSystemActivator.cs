using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyUtils
{
    public class EventSystemCreator
    {
        public EventSystemCreator()
        {
            var es = new GameObject("GameplayEventSystem").AddComponent<EventSystem>().AddComponent<StandaloneInputModule>();
        }
    }
}