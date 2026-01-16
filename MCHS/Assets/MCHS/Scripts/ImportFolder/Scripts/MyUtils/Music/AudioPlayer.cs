using UnityEngine;

namespace MyUtils.Music
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        private static AudioPlayer instance;

        public static void Play(AudioClip clip)
        {
            if (instance == null)
            {
                Debug.LogError("No AudioPlayer object attached");
                return;
            }
            instance.PlaySound(clip);
        }
        
        private AudioSource audioSource;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
                Debug.LogError("Duplicate AudioPlayer");
            }
            
            audioSource = GetComponent<AudioSource>();
        }

        private void PlaySound(AudioClip clip)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}