using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent (typeof(BoxCollider))]
public class FiniteGettingStorage : GettingStorage
{

    public ResourceType[] RequiringResources = new ResourceType[] {ResourceType.Any};
    public int[] AmountOfRequiringResources = new int[] {5};

    private int _fullAmountOfRequiringResources;


    protected override void CheckRequiringResources() //this method made for bug avoiding
    {
        if(RequiringResources.Length != AmountOfRequiringResources.Length)
            throw new System.Exception($"RequiringResources.Length != AmountOfRequiringResources.Length ({RequiringResources.Length}, {AmountOfRequiringResources.Length})");
        
        int fullAmountOfRequiringResources = 0;
        for (int i = 0; i < AmountOfRequiringResources.Length; i++)
        {
            fullAmountOfRequiringResources+=AmountOfRequiringResources[i];
        }
        _fullAmountOfRequiringResources = fullAmountOfRequiringResources;
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

    public override ResourceType IsNeedThisResource(ResourceType resourceType) 
    {
        if(GetRequiringResourceAmount(resourceType) > 0)
            return resourceType;
        if(GetRequiringResourceAmount(ResourceType.Any) > 0)
            return ResourceType.Any;

        return ResourceType.Empty;
    }

    public override bool IsFull()
    {
        return _fullAmountOfRequiringResources <= 0;
    }

    private void DecreaseResourceAmount(ResourceType resourceType, int value = 1)
    {
        int requiringResourceIndex = GetRequiringResourceIndex(resourceType);
        
        AmountOfRequiringResources[requiringResourceIndex] -= value;
        _fullAmountOfRequiringResources -= value;
    }

    private void IncreaseResourceAmount(ResourceType resourceType, int value = 1)
    {
        int requiringResourceIndex = GetRequiringResourceIndex(resourceType);
        
        if(requiringResourceIndex == -1)
        {
            Array.Resize<ResourceType>(ref RequiringResources, RequiringResources.Length + 1);
            Array.Resize<int>(ref AmountOfRequiringResources, AmountOfRequiringResources.Length + 1);
            RequiringResources[RequiringResources.Length - 1] = resourceType;
            AmountOfRequiringResources[AmountOfRequiringResources.Length - 1] = value;
            return;
        }

        AmountOfRequiringResources[requiringResourceIndex]++;
        _fullAmountOfRequiringResources++;
    }

    public override bool AddNewItem(Resource resource, bool isInvokeEvents = true)
    {

        bool isSuccess = base.AddNewItem(resource, false);
        if(isSuccess)
        {
            DecreaseResourceAmount(IsNeedThisResource(resource.resourceType));
            if(isInvokeEvents)
            {
                storageEventManager.OnAddNewItem.Invoke();
                if(IsFull())
                    storageEventManager.OnFull.Invoke();
            }
        }

        return isSuccess;
    }

    protected override void Start()
    {
        base.Start();
    }

}
