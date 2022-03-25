using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent (typeof(BoxCollider))]
public class InfiniteGettingStorage : GettingStorage
{
    [SerializeField] private ResourceType _requiringResource = ResourceType.Any;

    protected override void CheckRequiringResources() //this method made for bug avoiding
    {
        if(_requiringResource == ResourceType.Empty)
            throw new System.Exception("RequiringResource == ResourceType.Empty");
    }

    public override ResourceType IsNeedThisResource(ResourceType resourceType) 
    {
        if(resourceType == _requiringResource)
            return resourceType;
        if(_requiringResource == ResourceType.Any)
            return ResourceType.Any;
        return ResourceType.Empty;
    }
}
