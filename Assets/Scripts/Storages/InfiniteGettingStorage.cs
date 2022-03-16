using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent (typeof(BoxCollider))]
public class InfiniteGettingStorage : GettingStorage
{
    public ResourceType RequiringResource = ResourceType.Any;

    protected override void CheckRequiringResources() //this method made for bug avoiding
    {
        if(RequiringResource == ResourceType.Empty)
            throw new System.Exception("RequiringResource == ResourceType.Empty");
    }

    public override ResourceType IsNeedThisResource(ResourceType resourceType) 
    {
        return resourceType == RequiringResource ? resourceType : ResourceType.Empty;
    }
}
