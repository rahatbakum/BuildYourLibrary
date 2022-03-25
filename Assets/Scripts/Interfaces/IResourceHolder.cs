public interface IResourceHolder
{
    int ItemAmount {get;}
    ResourceType this[int index] {get;}
    int FullMaxItemAmount {get;}
    public int IsHasResourceType(ResourceType resourceType);
    public bool IsFull();
    public bool IsEmpty();
    void RemoveItem(IResourceGetter resourceGetter, int number);
    void RemoveItem(IResourceGetter resourceGetter, ResourceType resourceType);
}
