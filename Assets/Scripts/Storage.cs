using UnityEngine;
using System;

public abstract class Storage : MonoBehaviour, IResourceHolder
{
    private const float ResourceInterval = 0.05f;

    public Resource[] Items; //default is protected, make public or SerializeField for debug
    protected Transform[] Slots;
    [SerializeField] protected Vector3Int MaxItemAmount = new Vector3Int(3,3,3);
    [SerializeField] protected Vector3 StorageOffset = new Vector3(-0.4f, 0.1f, 0.4f);
    [SerializeField] protected Vector3 DefaultRotation = new Vector3(0f, 90f, 0f); //default rotation of resources

    [HideInInspector] public StorageEventManager storageEventManager = new StorageEventManager();
    
    public int ItemAmount {get; protected set;}
    
    public int FullMaxItemAmount 
    {
        get => MaxItemAmount.x * MaxItemAmount.y * MaxItemAmount.z;
    }

    private GameObject CleanLocalInstantiate(string name, Vector3 localPosition, Quaternion localRotation, Transform parent)
    {
        GameObject newGameObject = new GameObject(name);
        newGameObject.transform.SetParent(parent);
        newGameObject.transform.localPosition = localPosition;
        newGameObject.transform.localRotation = localRotation;
        
        return newGameObject;
    }

    private void CreateNewSlots()
    {
        Slots = new Transform[FullMaxItemAmount];
        Quaternion rotation = Quaternion.Euler(DefaultRotation);

        Transform parentForSlots = CleanLocalInstantiate("Slots", Vector3.zero, Quaternion.identity, transform).transform;

        for (int i = 0; i < FullMaxItemAmount; i++)
            Slots[i] = CleanLocalInstantiate($"Slot {i}", LocalPositionByNumber(i), rotation, parentForSlots).transform;
    }

    protected void TryAddNearWildResources(Predicate<Resource> condition)
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Collider[] colliders = Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size * 0.5f, transform.rotation);
        foreach(var collider in colliders)
        {
            if(collider.tag == "Resource"){
                Resource resource = collider.GetComponent<Resource>();
                if(condition(resource))
                    AddNewItem(collider.GetComponent<Resource>());
            }
        }
    }

    protected virtual void Start()
    {
        Items = new Resource[FullMaxItemAmount];
        ItemAmount = 0;

        CreateNewSlots();
    }

    public int IsHasResourceType(ResourceType resourceType) //returns index when ResourseType is in, returns -1 when here is no requiring ResourceType
    {
        if(resourceType == ResourceType.Any)
            return ItemAmount > 0 ? ItemAmount - 1 : -1;
        if(resourceType == ResourceType.Empty)
            throw new System.Exception("ResourceType is Empty");

        for(int i = ItemAmount - 1; i >= 0; i--)
            if(Items[i].resourceType == resourceType)
                return i;
        return -1;
    }

    public ResourceType this[int index]
    {
        get => Items[index].resourceType;
    }

    public bool IsFull()
    {
        return ItemAmount >= FullMaxItemAmount;
    }

    protected bool isInThisStorage(Resource resource)
    {
        foreach(var item in Items)
        {
            if(item == resource)
                return true;
        }

        return false;
    }

    public virtual bool AddNewItem(Resource resource) //returns false when error, returns true when success
    {
        if(!resource.IsAvailableToCatch)
            throw new System.Exception($"Resource {resource.resourceType} is not available to catch");
        
        if( IsFull() || isInThisStorage(resource))
            return false;

        Items[ItemAmount] = resource;
        resource.SetNewSlot(Slots[ItemAmount]);
        ItemAmount++;

        storageEventManager.OnAddNewItem.Invoke();
        if(IsFull())
            storageEventManager.OnFull.Invoke();
        return true;
    }

    private void InvokeRemoveItemEvents()
    {
        storageEventManager.OnRemoveItem.Invoke();
        if(ItemAmount <= 0)
            storageEventManager.OnEmpty.Invoke();
    }

    private void ShiftItem(int number)
    {
        Items[number].ForceSetNewSlot(Slots[number - 1]);
        Items[number] = Items[number + 1];
    }

    public virtual void RemoveItem(IResourceHolder sender, int number)
    {
        if(!sender.AddNewItem(Items[number]))
            return;

        if(number > ItemAmount - 1)
            throw new System.Exception("Index > ItemAmount - 1");

        if(number == ItemAmount - 1)
        {
            Items[number] = null;
            ItemAmount--;
            InvokeRemoveItemEvents();
            return;
        }

        Items[number] = Items[number + 1];
        for (int i = number + 1; i < ItemAmount; i++)
        {
            ShiftItem(i);
        }
        ItemAmount--;

        InvokeRemoveItemEvents();
    }

    public virtual void RemoveItem(IResourceHolder sender, ResourceType resourceType) //removes Item by ResourceType
    {
        if(resourceType == ResourceType.Empty)
            throw new System.Exception("ResourceType is Empty");

        if(resourceType == ResourceType.Any)
        {
            RemoveItem(sender, ItemAmount - 1);
            return;
        }

        int number = IsHasResourceType(resourceType);

        if(number >= 0)
            RemoveItem(sender, number);
    }

    protected float CorrectCenter(int value, int number) 
    {
        return value + (Resource.IsResourceInCenter[number] ? 0.5f : 0f); 
    }

    protected virtual Vector3 CorrectCenterVector(Vector3Int value)
    {
        return new Vector3(CorrectCenter(value.x, 0), CorrectCenter(value.y, 1), CorrectCenter(value.z, 2)); 
    }

    protected Vector3Int NumberCoords(int number) 
    {
        int yNumber = number / (MaxItemAmount.x * MaxItemAmount.z);
        int layerNumber = number % (MaxItemAmount.x * MaxItemAmount.z);
        int xNumber = layerNumber % MaxItemAmount.x;
        int zNumber = layerNumber / MaxItemAmount.x;

        return new Vector3Int(xNumber, yNumber, zNumber);
    }

    protected virtual Vector3 LocalPositionByNumber(int number)
    {
        Vector3 startPosition = StorageOffset;
        Vector3Int numberCoords = NumberCoords(number);
        Quaternion defaultRotation = Quaternion.Euler(DefaultRotation);
        Vector3 resourceSizeWithIntervals = Resource.ResourceSize + new Vector3(ResourceInterval, ResourceInterval, ResourceInterval);
        Vector3 resourceSize = defaultRotation * resourceSizeWithIntervals;
        Vector3 axisNumbers = CorrectCenterVector(numberCoords);
        return startPosition + new Vector3(axisNumbers.x * resourceSize.x, axisNumbers.y * resourceSize.y, axisNumbers.z * resourceSize.z);
    }
}
