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

    private int GetRequiringResourceIndex(ResourceType resourceType)
    {
        for(int i = 0; i < RequiringResources.Length; i++)
        {
            if(RequiringResources[i] == resourceType)
                return i;       
        }

        return -1;
    }

    public int GetRequiringResourceAmount(ResourceType resourceType)
    {
        int requiringResourceIndex = GetRequiringResourceIndex(resourceType);

        if(requiringResourceIndex < 0)
            return 0;

        return AmountOfRequiringResources[requiringResourceIndex];
    }

    /// <summary>
    /// Returns resourceType when storage need it
    /// Returns ResourceType.Any when storage need ResourseType.Any
    /// Returns ResourceType.Empty when storage don't need resourseType
    /// </summary> 

    public ResourceType IsNeedThisResource(ResourceType resourceType) 
    {
        if(GetRequiringResourceAmount(resourceType) > 0)
            return resourceType;
        if(GetRequiringResourceAmount(ResourceType.Any) > 0)
            return ResourceType.Any;

        return ResourceType.Empty;
    }

    public void SetNewRequiringResources(ResourceType[] resourceType, int[] amount)
    {
        RequiringResources = resourceType;
        AmountOfRequiringResources = amount;

        CheckIsRightLengths();
    }

    public void SetNewRequiringResources(ResourceType resourceType, int amount)
    {
        CheckIsRightLengths();

        int requiringResourceIndex = GetRequiringResourceIndex(resourceType);
        if(AmountOfRequiringResources[requiringResourceIndex] > 0)
        {
            AmountOfRequiringResources[requiringResourceIndex] += amount;
            return;
        }

        System.Array.Resize<ResourceType>(ref RequiringResources, RequiringResources.Length);
        RequiringResources[RequiringResources.Length - 1] = resourceType;
        
        System.Array.Resize<int>(ref AmountOfRequiringResources, AmountOfRequiringResources.Length);
        AmountOfRequiringResources[AmountOfRequiringResources.Length - 1] = amount;
    }

    private void DecreaseResourceAmount(ResourceType resourceType)
    {
        int requiringResourceIndex = GetRequiringResourceIndex(resourceType);
        
        AmountOfRequiringResources[requiringResourceIndex]--;
    }

    public override void AddNewItem(Resource resource)
    {
        ResourceType isNeedThisResource = IsNeedThisResource(resource.resourceType);
        if(isNeedThisResource == ResourceType.Empty)
            throw new System.Exception($"GettingStorage don't need {resource.resourceType}");
        base.AddNewItem(resource);

        DecreaseResourceAmount(isNeedThisResource);
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
                Resource resource = collider.GetComponent<Resource>();
                if(IsNeedThisResource(resource.resourceType) != ResourceType.Empty)
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
        
        if(IsNeedThisResource(ResourceType.Any) == ResourceType.Empty)
            return;

        if(!playerStorage.IsHasResourceType(ResourceType.Any))
            return;
        
        playerStorage.RemoveItem(this as IResourceHolder, ResourceType.Any);

        _lastGiveTime = Time.time;
    }
}
