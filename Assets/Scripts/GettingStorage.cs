using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent (typeof(BoxCollider))]
public class GettingStorage : Storage
{
    private const string ResourceTag = "Resource";

    public ResourceType[] RequiringResources = new ResourceType[] {ResourceType.Any};
    public int[] AmountOfRequiringResources = new int[] {5};
    
    [SerializeField] private float _gettingTime = 0.15f;

    private float _lastGetTime;


    private void CheckIsRightLengths() //this method made for bug avoiding
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

    // public void SetNewRequiringResources(ResourceType resourceType, int amount)
    // {
    //     CheckIsRightLengths();

    //     int requiringResourceIndex = GetRequiringResourceIndex(resourceType);
    //     if(requiringResourceIndex >= 0 && AmountOfRequiringResources[requiringResourceIndex] > 0)
    //     {
    //         AmountOfRequiringResources[requiringResourceIndex] += amount;
    //         return;
    //     }

    //     System.Array.Resize<ResourceType>(ref RequiringResources, RequiringResources.Length + 1);
    //     RequiringResources[RequiringResources.Length - 1] = resourceType;
        
    //     System.Array.Resize<int>(ref AmountOfRequiringResources, AmountOfRequiringResources.Length + 1);
    //     AmountOfRequiringResources[AmountOfRequiringResources.Length - 1] = amount;
    // }

    private void DecreaseResourceAmount(ResourceType resourceType)
    {
        int requiringResourceIndex = GetRequiringResourceIndex(resourceType);
        
        AmountOfRequiringResources[requiringResourceIndex]--;
    }

    public override bool AddNewItem(Resource resource)
    {
        ResourceType isNeedThisResource = IsNeedThisResource(resource.resourceType);
        if(isNeedThisResource == ResourceType.Empty)
            throw new System.Exception($"GettingStorage don't need {resource.resourceType}");
        bool isSuccess = base.AddNewItem(resource);
        DecreaseResourceAmount(isNeedThisResource);

        return isSuccess;
    }

    protected override void Start()
    {
        base.Start();
        
        _lastGetTime = Time.time - _gettingTime;

        CheckIsRightLengths();

        TryAddNearWildResources((Resource resource) => IsNeedThisResource(resource.resourceType) != ResourceType.Empty);
    }

    

    private void OnTriggerStay(Collider other)
    {

        if(Time.time - _lastGetTime < _gettingTime)
            return;

        if(other?.tag != "Player")
            return;
        
        PlayerStorage playerStorage = other.GetComponentInChildren<PlayerStorage>();
        if(playerStorage == null)
            return;
        
        if(IsFull())
            return;

        if(playerStorage.IsHasResourceType(ResourceType.Any) < 0)
            return;

        for(int i = 0; i < RequiringResources.Length; i++)
        {
            if(AmountOfRequiringResources[i] > 0)
            {
                int number = playerStorage.IsHasResourceType(RequiringResources[i]);
                if(number >= 0)
                {
                    playerStorage.RemoveItem(this as IResourceHolder, number);
                }
            }
        }

        _lastGetTime = Time.time;
    }
}
