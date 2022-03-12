using UnityEngine;

public class GivingStorage : MonoBehaviour, IResourceHolder
{
    private Resource[] _items; 
    public (int, int) MaxItemAmount = (4,3);
    public float StorageHeight = 0.1f;
    public Vector3 ResourceSize  = new Vector3(0.4f, 0.2f, 0.2f);
    public int ItemAmount {get; private set;}

    int GetMaxItemAmount ()
    {
        return MaxItemAmount.Item1 * MaxItemAmount.Item2;
    }

    void Start()
    {
        _items = new Resource[GetMaxItemAmount()];
        ItemAmount = 0;
    }

    public bool IsFull()
    {
        return ItemAmount >= GetMaxItemAmount();
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

    public void AddNewItem(Resource resource)
    {
        if(IsFull())
            return;
        
        _items[ItemAmount] = resource;
        resource.SetNewResourceHolder(transform, PositionByNumber(ItemAmount));
        resource.IsAvailableToCatch = false;
        ItemAmount++;
    }
    public void RemoveItemByNumber(IResourceHolder sender, int number)
    {
        sender.AddNewItem(_items[number]);
        _items[number] = _items[number + 1];
        for (int i = number + 1; i < ItemAmount - 1; i++)
        {
            _items[i].IsAvailableToCatch = true;
            _items[i].SetNewResourceHolder(transform, PositionByNumber(i));
            _items[i].IsAvailableToCatch = false;
            _items[i] = _items[i + 1];
        }
        _items[ItemAmount - 1].IsAvailableToCatch = true;
        _items[ItemAmount - 1].SetNewResourceHolder(transform, PositionByNumber(ItemAmount - 1));
        _items[ItemAmount - 1].IsAvailableToCatch = false;
        _items[ItemAmount - 1] = null;
        ItemAmount--;
    }
    public void RemoveItem(IResourceHolder sender, ResourceType resourceType)
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
    public Vector3 PositionByNumber(int number)
    {
        Vector3 startPosition = transform.position + transform.up * StorageHeight - transform.right * MaxItemAmount.Item1 * ResourceSize.z / 2;
        return startPosition + (number % MaxItemAmount.Item1) * ResourceSize.z * transform.right + (number / MaxItemAmount.Item1) * ResourceSize.y * transform.up;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other?.tag != "Player")
            return;
        
        PlayerStorage playerStorage = other.GetComponentInChildren<PlayerStorage>();
        if(playerStorage == null)
            return;
        
        if(playerStorage.IsFull())
            return;
        
        RemoveItem(playerStorage as IResourceHolder, ResourceType.Any);
    }
}
