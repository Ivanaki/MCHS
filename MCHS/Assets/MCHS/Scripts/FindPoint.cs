using System;
using MySteamVR;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MCHS.Scripts
{
    public class FindPoint : MonoBehaviour
    {
        //public static FindPoint instance;
        
        private Camera _camera;


        private void Start()
        {
            _camera = MyPlayer.instance.GetCamera();
        }

        private void FixedUpdate()
        {
            transform.position = _camera.transform.position - new Vector3(0, _camera.transform.localPosition.y, 0);
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        /*private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }*/
    }
}