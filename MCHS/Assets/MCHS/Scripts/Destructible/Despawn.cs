using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Made by Rajendra Abhinaya, 2023

public class Despawn : MonoBehaviour
{
    private int despawnPercentage;
    private float despawnTime;
    private float distanceFromPlayer;
    private GameObject player;
    private AudioClip clip;
    private float volume;
    private float variation;
    private float volumeVariation;
    private float pitchVariation;
    private AudioSource audioSource;

    //Used to receive the variables' values from the parent object
    public void SetVariables(AudioClip clip, float volume, float volumeVariation, float pitchVariation){
        this.clip = clip;
        this.volume = volume;
        this.volumeVariation = volumeVariation;
        this.pitchVariation = pitchVariation;
    }

    void Start(){
        //Plays a random audio clip from the list of audio clips set in the object
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1f + Random.Range(-pitchVariation/2, pitchVariation/2);
        audioSource.PlayOneShot(clip, volume + Random.Range(-volumeVariation, volumeVariation));
    }

    //Starts the selected despawn mode's coroutine function
    

    //Despawns the debris based on the despawn percentage
    public IEnumerator DespawnDebris(float timeDispawn)
    {
        yield return new WaitForSeconds(timeDispawn * 10f);
        
        for(var i = transform.childCount-1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
            yield return new WaitForSeconds(timeDispawn);
        }
    }

    //Checks the distance between the debris and the player every 0.5 seconds after a 5 second delay
    public IEnumerator CheckDistance(){
        yield return new WaitForSeconds(5f);
        while(true){
            Vector3 distance = transform.position - player.transform.position;
            if(distance.magnitude > distanceFromPlayer)
            {
                
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    //Despawns the debris after a set amount of time
    public IEnumerator DespawnCoroutine()
    {
        yield return new WaitForSeconds(despawnTime * 2);
    }
}
