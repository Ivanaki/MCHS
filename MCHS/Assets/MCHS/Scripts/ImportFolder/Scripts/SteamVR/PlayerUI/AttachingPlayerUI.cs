using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

namespace MySteamVR.PlayerUI
{
    public class AttachingPlayerUI : MonoBehaviour
    {
        private void Start()
        {
            MyPlayer.instance.AttachToPlayer(transform);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);
        }
    }
}