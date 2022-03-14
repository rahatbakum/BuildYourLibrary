using UnityEngine;

public interface IResourceHolder
{
    int ItemAmount {get;}

    bool IsFull();
    bool IsHasResourceType(ResourceType resourceType);
    ResourceType LastResourceType(int number);
    void AddNewItem(Resource resource);
    void RemoveItemByNumber(IResourceHolder sender, int number);
    void RemoveItem(IResourceHolder sender, ResourceType resourceType);
    //Vector3 PositionByNumber(int number);
}
