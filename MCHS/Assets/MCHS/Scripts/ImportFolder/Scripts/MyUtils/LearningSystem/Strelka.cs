using UnityEngine;

namespace MyUtils.LearningSystem
{
    public class Strelka : MonoBehaviour
    {
        [SerializeField] private GameObject _strelkaAnim;
        
        public void SetPosition(Transform pivot, Transform strelka)
        {
            transform.position = strelka.position;
            transform.LookAt(pivot);
        }

        public void Activate()
        {
            _strelkaAnim.SetActive(true);
        }

        public void Deactivate()
        {
            _strelkaAnim.SetActive(false);
        }
    }
}