using System;
using System.Collections;
using System.Collections.Generic;
using MCHS.Scripts.BackPack;
using MCHS.Scripts.Destructible;
using R3;
using UnityEngine;
using Random = UnityEngine.Random;

//Made by Rajendra Abhinaya, 2023

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private float _cooldown = 0.5f;
    
    [SerializeField] private Material _0;
    [SerializeField] private Material _1;
    [SerializeField] private Material _2;

    [SerializeField] private MeshRenderer _meshRenderer;
    
    
    [SerializeField] private Despawn _prefabBroken;

    [SerializeField, Tooltip("Force required to break the object")]
    private float forceRequired;
    
    [SerializeField, Tooltip("Time in seconds before debris will despawn when using the Timed despawn mode")]
    private float despawnTime;
    [SerializeField] private int _health;

    [Header("Audio")]
    [SerializeField] private List<AudioClip> _audioClipsPunch = new List<AudioClip>();
    
    [SerializeField, Tooltip("List of audio clips that will be played when the object breaks. Audio clips are selected randomly from the list")]
    private List<AudioClip> audioClipsBreak = new List<AudioClip>();

    [SerializeField, Tooltip("Volume of the audio clip when played"), Range(0f, 1f)]
    private float volume;

    [SerializeField, Tooltip("Amount of variation in the volume of each audio clip played"), Range(0f, 0.2f)]
    private float volumeVariation;

    [SerializeField, Tooltip("Amount of variation in the pitch volume of each audio clip played"), Range(0f, 0.5f)]
    private float pitchVariation;

    private ReactiveProperty<int> _healthProperty;
    
    private Despawn debris;
    private new Rigidbody rigidbody;
    
    private CompositeDisposable _disposables;
    
    private AudioSource _audioSource = null;
    

    private void Awake()
    {
        _healthProperty =  new ReactiveProperty<int>(_health);
        TryGetComponent(out _audioSource);
    }

    private void OnEnable()
    {
        _disposables = new CompositeDisposable();
        
        _disposables.Add(_healthProperty.Subscribe(value =>
        {
            if (_meshRenderer != null)
            {
                switch (value)
                {
                    case 1:
                        _meshRenderer.material = _2;
                        break;
                    case 2:
                        _meshRenderer.material = _1;
                        break;
                    case 3:
                        _meshRenderer.material = _0;
                        break;
                }
            }

            if (_audioClipsPunch.Count > 0 && _audioSource != null && value is 2 or 1)
            {
                _audioSource.PlayOneShot(_audioClipsPunch[Random.Range(0, _audioClipsPunch.Count)]);
            }
        }));
    }

    private void OnDisable()
    {
        _disposables?.Dispose();
    }

    public void Break(int force)
    {
        _healthProperty.Value -= force;
        if (_healthProperty.Value > 0) return;
        
        float velocityMagnitude = rigidbody.velocity.magnitude;

        debris.SetVariables(audioClipsBreak[Random.Range(0, audioClipsBreak.Count)], volume, volumeVariation, pitchVariation);

        
        //Activates the debris object and sets its position and rotation to match the object's
        debris.transform.position = transform.position;
        debris.transform.rotation = transform.rotation;
        debris.transform.localScale = transform.lossyScale;
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

    private float _time = 0f;
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.relativeVelocity.magnitude > forceRequired)
        {
            if (Time.time - _time >= _cooldown)
            {
                if (collision.gameObject.TryGetComponent(out Axe axe1))
                {
                    _time = Time.time;
                    Break(axe1.Force);
                }
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
