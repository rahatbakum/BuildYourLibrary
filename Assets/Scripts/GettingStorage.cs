using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(BoxCollider))]
public class GettingStorage : Storage
{
    public Dictionary<ResourceType, int> ExpectingResources = new Dictionary<ResourceType, int> {{ResourceType.Any, 5}};

    protected override void Start()
    {
        base.Start();
        
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Collider[] colliders = Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size * 0.5f, transform.rotation);
        foreach(var collider in colliders)
        {
            if(collider.tag == "Resource"){
                AddNewItem(collider.GetComponent<Resource>());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other?.tag != "Player")
            return;
        
        PlayerStorage playerStorage = other?.GetComponent<PlayerController>()?.Storage;
        if(playerStorage == null)
            return;
        
        if(IsFull())
            return;

        if(playerStorage.IsHasResourceType(ResourceType.Any))
            playerStorage.RemoveItem(this as IResourceHolder, ResourceType.Any);
    }
}
