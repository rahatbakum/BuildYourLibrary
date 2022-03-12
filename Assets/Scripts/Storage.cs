using UnityEngine;

public abstract class Storage : MonoBehaviour, IResourceHolder
{
    protected Resource[] _items; 
    public (int, int, int) MaxItemAmount = (4,3,0);
    public Vector3 StorageOffset = new Vector3(-0.4f, 0.1f, 0f);
    public Vector3 ResourceSize  = new Vector3(0.4f, 0.2f, 0.2f);
    public int ItemAmount {get; protected set;}
    
    public int GetMaxItemAmount ()
    {
        return MaxItemAmount.Item1 * MaxItemAmount.Item2 * MaxItemAmount.Item3;
    }

    protected void Start()
    {
        // BoxCollider boxCollider = GetComponent<BoxCollider>();
        // Collider[] colliders = Physics.OverlapBox(boxCollider.center, boxCollider.size * 0.5f, transform.rotation);

        // foreach(var collider in colliders)
        // {
        //     if(collider.tag == "Resource")
        //         AddNewItem(collider.GetComponent<Resource>());
        // }

        _items = new Resource[GetMaxItemAmount()];
        ItemAmount = 0;
    }

    public bool IsHasResourceType(ResourceType resourceType)
    {
        if(resourceType == ResourceType.Any)
            return ItemAmount > 0;
        if(resourceType == ResourceType.Empty)
            return ItemAmount < GetMaxItemAmount();

        foreach (var item in _items)
            if(item.resourceType == resourceType)
                return true;
        return false;
    }

    public ResourceType LastResourceType(int number)
    {
        return _items[GetMaxItemAmount() - number - 1].resourceType;
    }

    public bool IsFull()
    {
        return ItemAmount >= GetMaxItemAmount();
    }

    protected bool isInThisStorage(Resource resource)
    {
        foreach(var item in _items)
        {
            if(item == resource)
                return true;
        }

        return false;
    }

    public virtual void AddNewItem(Resource resource)
    {
        if(IsFull() || isInThisStorage(resource))
            return;
        
        _items[ItemAmount] = resource;
        resource.SetNewResourceHolder(transform, PositionByNumber(ItemAmount));
        ItemAmount++;
    }

    public virtual void RemoveItemByNumber(IResourceHolder sender, int number)
    {
        sender.AddNewItem(_items[number]);
        _items[number] = _items[number + 1];
        for (int i = number + 1; i < ItemAmount - 1; i++)
        {
            _items[i].SetNewResourceHolder(transform, PositionByNumber(i));
            _items[i] = _items[i + 1];
        }
        _items[ItemAmount - 2].SetNewResourceHolder(transform, PositionByNumber(ItemAmount - 2));
        _items[ItemAmount - 1] = null;
        ItemAmount--;
    }

    public virtual void RemoveItem(IResourceHolder sender, ResourceType resourceType)
    {
        if(resourceType == ResourceType.Empty)
            return;

         if(resourceType == ResourceType.Any)
        {
            RemoveItemByNumber(sender, ItemAmount - 1);
            return;
        }

        for(int i = ItemAmount - 1; i >= 0; i--)
        {
            if(_items[i].resourceType == resourceType)
            {
                RemoveItemByNumber(sender, i);
                return;
            }
        }
    }
    public abstract Vector3 PositionByNumber(int number);
}
