using System.Collections;
using MySteamVR.Laser;
using UnityEngine;

namespace MyUtils
{
    [RequireComponent(typeof(BoxCollider), typeof(RectTransform)), DefaultExecutionOrder(1)]
    public class BoxColliderButton : MonoBehaviour
    {
        [SerializeField] private float _buttonWight = 1f;
        
        private RectTransform _rectTransform;
        private BoxCollider _boxCollider;
        
        private void Awake()
        { 
            _boxCollider = GetComponent<BoxCollider>(); 
            _rectTransform = GetComponent<RectTransform>();
        }
        
        private void Start()
        {
            UpdateSize();
        }

        private void OnValidate()
        {
            UpdateSize();
        }

        public void UpdateSize()
        {
            Awake();
            _boxCollider.size = new Vector3(_rectTransform.rect.width, _rectTransform.rect.height, _buttonWight);
        }

        public void LateUpdateSize()
        {
            StartCoroutine(LateUpdateSizeCoroutine());
        }

        private IEnumerator LateUpdateSizeCoroutine()
        {
            yield return null;
            yield return null;
            yield return null;
            UpdateSize();
        }
    }
    
#if UNITY_EDITOR
    //-------------------------------------------------------------------------
    [UnityEditor.CustomEditor( typeof( BoxColliderButton ) )]
    public class BoxColliderButtonEditor : UnityEditor.Editor
    {
        //-------------------------------------------------
        // Custom Inspector GUI allows us to click from within the UI
        //-------------------------------------------------
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            BoxColliderButton uiElement = (BoxColliderButton)target;
            if ( GUILayout.Button( "Update Size" ) )
            {
                uiElement.UpdateSize();
            }
        }
    }
#endif
}