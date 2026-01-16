using UnityEngine;

namespace MCHS.Scripts.BackPack
{
    public class ColliderRedirector : MonoBehaviour
    {
        [SerializeField] private GameObject _mainObject;
        
        /// <summary>
        /// если мы знаем, что этот объект может быть найден другим скриптом с помощью коллайдера
        /// </summary>
        /// <param name="mainScript"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool TryGetComponentBase<T>(out T mainScript)
        {
            return _mainObject.TryGetComponent(out mainScript);
        }
    }
}