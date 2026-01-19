using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    
    [SerializeField] private float extinguishRate = 1.0f; //amount of fire extinguished power (per second)
    [SerializeField] private float extinguishDisctance = 5f; // distance to fire

    [SerializeField] private Transform raycastOrigin = null;

    [Space, Header("Steam And Pena")]
    [SerializeField] private GameObject steamObject = null;
    [SerializeField] private GameObject penaObject = null;

    private bool IsRaycastingSomething(out RaycastHit hit) => Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, extinguishDisctance, _layerMask);
    

    private bool IsRaycastingFire(out Fire fire) 
    {
        fire = null;
        //bool z = IsRaycastingSomething(out RaycastHit hit);
        //print(hit.collider.gameObject.name);
        return IsRaycastingSomething(out RaycastHit hit1) && hit1.collider.TryGetComponent(out fire);
    }

    private void OnEnable()
    {
        penaObject.SetActive(true);
    }

    private void OnDisable()
    {
        penaObject.SetActive(false);
    }
    
    private void Start()
    {
        if (!steamObject)
            Debug.LogError("Please place a steam particle system on the Extinguisher's steamObject field or rewrite the Extinguisher script.", this);
        penaObject.SetActive(true);
        steamObject.transform.SetParent(null);
        steamObject.SetActive(false);
    }

    private void Update()
    {
        Debug.DrawRay(raycastOrigin.position, raycastOrigin.forward * extinguishDisctance, Color.red);
        if (IsRaycastingFire(out Fire fire))
            ExtinguishFire(fire);
        else if (steamObject.activeSelf)
            steamObject.SetActive(false);
    }

    private void ExtinguishFire(Fire fire) 
    {
        fire.TryExtinguish(extinguishRate * Time.deltaTime);

        steamObject.transform.position = fire.transform.position;
        steamObject.SetActive(fire.GetIntensity() > 0.0f);
    }
}
