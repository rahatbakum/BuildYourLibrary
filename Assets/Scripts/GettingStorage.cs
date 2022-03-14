using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider))]
public class GettingStorage : Storage
{
    public ResourceType[] RequiringResources = new ResourceType[] {ResourceType.Any};
    public int[] AmountOfRequiringResources = new int[] {5};
    
    public float GivingTimeInterval = 0.15f;

    private float _startTime;
    private float _lastGiveTime;


    private void CheckIsRightLengths ()
    {
         if(RequiringResources.Length != AmountOfRequiringResources.Length)
            throw new System.Exception($"RequiringResources.Length != AmountOfRequiringResources.Length ({RequiringResources.Length}, {AmountOfRequiringResources.Length})");
    }

    public int GetRequiringResourceAmount(ResourceType resourceType)
    {
        for(int i = 0; i < RequiringResources.Length; i++)
        {
            if(RequiringResources[i] == resourceType)
                return AmountOfRequiringResources[i];       
        }

        return 0;
    }

    public void SetNewRequiringResources (ResourceType[] resourceType, int[] amount)
    {
        RequiringResources = resourceType;
        AmountOfRequiringResources = amount;

        CheckIsRightLengths();
    }

    public void SetNewRequiringResources (ResourceType resourceType, int amount)
    {
        System.Array.Resize<ResourceType>(ref RequiringResources, RequiringResources.Length);
        RequiringResources[RequiringResources.Length - 1] = resourceType;
        
        System.Array.Resize<int>(ref AmountOfRequiringResources, AmountOfRequiringResources.Length);
        AmountOfRequiringResources[AmountOfRequiringResources.Length - 1] = amount;

        CheckIsRightLengths();
    }

    protected override void Start()
    {
        base.Start();
        
        _startTime = Time.time;
        _lastGiveTime = _startTime;

        CheckIsRightLengths();

        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Collider[] colliders = Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size * 0.5f, transform.rotation);
        foreach(var collider in colliders)
        {
            if(collider.tag == "Resource"){
                AddNewItem(collider.GetComponent<Resource>());
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        if(Time.time - _lastGiveTime < GivingTimeInterval)
            return;

        if(other?.tag != "Player")
            return;
        
        PlayerStorage playerStorage = other.GetComponentInChildren<PlayerStorage>();
        if(playerStorage == null)
            return;
        
        if(IsFull())
            return;
        
        if(!playerStorage.IsHasResourceType(ResourceType.Any))
            return;
        
        playerStorage.RemoveItem(this as IResourceHolder, ResourceType.Any);

        _lastGiveTime = Time.time;
    }
}
