using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent (typeof(BoxCollider))]
public abstract class GettingStorage : Storage
{
    protected const string ResourceTag = "Resource";

    [SerializeField] private float _gettingTime = 0.15f;

    private float _lastGetTime;


    protected abstract void CheckRequiringResources(); //this method made for bug avoiding

    /// <summary>
    /// Returns resourceType when storage need it.
    /// Returns ResourceType.Any when storage need ResourseType.Any.
    /// Returns ResourceType.Empty when storage don't need resourseType.
    /// </summary> 

    public abstract ResourceType IsNeedThisResource(ResourceType resourceType);

    protected override void Start()
    {
        base.Start();
        
        _lastGetTime = Time.time - _gettingTime;

        CheckRequiringResources();

        TryAddNearWildResources((Resource resource) => IsNeedThisResource(resource.resourceType) != ResourceType.Empty);
    }

    private void AddNewItems(PlayerStorage playerStorage)
    {
        for(int i = playerStorage.ItemAmount - 1; i >= 0; i--)
        {
            ResourceType isNeedThisResource = IsNeedThisResource(playerStorage[i]);
            if(isNeedThisResource != ResourceType.Empty)
            {
                playerStorage.RemoveItem(this, i);
                _lastGetTime = Time.time;
                return;
            }
        }
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

        AddNewItems(playerStorage);
    }
}
