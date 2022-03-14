using UnityEngine;

public interface IResourceHolder
{

    void AddNewItem(Resource resource);
    void RemoveItem(IResourceHolder sender, ResourceType resourceType);
}
