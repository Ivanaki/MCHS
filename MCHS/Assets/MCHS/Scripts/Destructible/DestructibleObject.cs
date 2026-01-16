using System.Collections;
using System.Collections.Generic;
using MCHS.Scripts.BackPack;
using MCHS.Scripts.Destructible;
using UnityEngine;

//Made by Rajendra Abhinaya, 2023

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private Despawn _prefabBroken;

    [SerializeField, Tooltip("Force required to break the object")]
    private float forceRequired;
    
    [SerializeField, Tooltip("Time in seconds before debris will despawn when using the Timed despawn mode")]
    private float despawnTime;
    [SerializeField] private int _health = 1;

    [Header("Audio")]
    [SerializeField, Tooltip("List of audio clips that will be played when the object breaks. Audio clips are selected randomly from the list")]
    private List<AudioClip> audioClips = new List<AudioClip>();

    [SerializeField, Tooltip("Volume of the audio clip when played"), Range(0f, 1f)]
    private float volume;

    [SerializeField, Tooltip("Amount of variation in the volume of each audio clip played"), Range(0f, 0.2f)]
    private float volumeVariation;

    [SerializeField, Tooltip("Amount of variation in the pitch volume of each audio clip played"), Range(0f, 0.5f)]
    private float pitchVariation;

   
    
    private Despawn debris;
    private new Rigidbody rigidbody;

    public void Break(int force)
    {
        _health -= force;
        if (_health > 0) return;
        
        float velocityMagnitude = rigidbody.velocity.magnitude;

        debris.SetVariables(audioClips[Random.Range(0, audioClips.Count)], volume, volumeVariation, pitchVariation);

        
        //Activates the debris object and sets its position and rotation to match the object's
        debris.transform.position = transform.position;
        debris.transform.rotation = transform.rotation;
        debris.transform.localScale = transform.localScale;
        debris.gameObject.SetActive(true);

        //Applies force to the debris based on the velocity of the object
        for(int i = 0; i < debris.transform.childCount; i++){
            Rigidbody debrisRigidbody = debris.transform.GetChild(i).GetComponent<Rigidbody>();
            Vector3 randomise = new Vector3(Random.Range(0f, velocityMagnitude), Random.Range(0f, velocityMagnitude), Random.Range(0f, velocityMagnitude)) / 2;
            debrisRigidbody.velocity = rigidbody.velocity + randomise;
        }

        
        
        //Activates the despawning mechanism of the debris
        debris.StartCoroutine(debris.DespawnDebris(despawnTime));

        //Destroys the game object
        Destroy(gameObject);
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        debris = Instantiate(_prefabBroken, transform.position, Quaternion.identity);
        
        debris.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > forceRequired)
        {
            if (collision.gameObject.TryGetComponent(out Axe axe1))
            {
                Break(axe1.Force);
            }
            /*else
            {
                if (collision.gameObject.TryGetComponent(out ColliderRedirector colliderRedirector))
                {
                    if (colliderRedirector.TryGetComponentBase(out Axe axe))
                    {
                        Break(axe.Force);
                    }
                }
            }*/
        }
    }
}
