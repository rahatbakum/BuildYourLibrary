using UnityEngine;


public abstract class Storage : MonoBehaviour, IResourceHolder
{
    protected Resource[] Items; //default is protected, make public or SerializeField for debug
    protected Transform[] Slots;
    public Vector3Int MaxItemAmount = new Vector3Int(3,3,3);
    public Vector3 StorageOffset = new Vector3(-0.4f, 0.1f, 0.4f);
    public Vector3 ResourceSize  = new Vector3(0.45f, 0.25f, 0.25f);
    public Vector3 DefaultRotation = new Vector3(0f, 90f, 0f); //default rotation of resources
    public bool[] IsResourceInCenter = new bool[] {true, false, true}; //this var made for repair resource size offset
    public int ItemAmount {get; protected set;}
    
    public int GetMaxItemAmount ()
    {
        return MaxItemAmount.x * MaxItemAmount.y * MaxItemAmount.z;
    }

    private void CreateNewSlots()
    {
        Slots = new Transform[GetMaxItemAmount()];
        Quaternion rotation = Quaternion.Euler(DefaultRotation);
        Transform parentForSlots = (Instantiate(new GameObject(), transform) as GameObject).transform;
        parentForSlots.name = "Slots";

        for (int i = 0; i < GetMaxItemAmount(); i++)
        {
            Slots[i] = Instantiate(new GameObject(), parentForSlots).transform;
            Slots[i].localPosition = LocalPositionByNumber(i);
            Slots[i].localRotation = rotation;
            Slots[i].name = $"Slot {i}";
        }
    }

    protected virtual void Start()
    {
        Items = new Resource[GetMaxItemAmount()];
        ItemAmount = 0;

        CreateNewSlots();

    }

    public bool IsHasResourceType(ResourceType resourceType)
    {
        
        if(resourceType == ResourceType.Any)
            return ItemAmount > 0;
        if(resourceType == ResourceType.Empty)
            return ItemAmount < GetMaxItemAmount();

        foreach (var item in Items)
            if(item.resourceType == resourceType)
                return true;
        return false;
    }

    public ResourceType LastResourceType(int number)
    {
        return Items[GetMaxItemAmount() - number - 1].resourceType;
    }

    public bool IsFull()
    {
        return ItemAmount >= GetMaxItemAmount();
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

    public virtual void AddNewItem(Resource resource)
    {
        if(!resource.IsAvailableToCatch)
            throw new System.Exception($"Resource {resource.resourceType} is not available to catch");
        
        if( IsFull() || isInThisStorage(resource))
            return;
        Items[ItemAmount] = resource;
        resource.SetNewResourceHolder(Slots[ItemAmount]);
        ItemAmount++;
    }

    public virtual void RemoveItemByNumber(IResourceHolder sender, int number)
    {
        sender.AddNewItem(Items[number]);

        if(number >= ItemAmount - 1)
        {
            ItemAmount--;
            return;
        }

        Items[number] = Items[number + 1];
        for (int i = number + 1; i < ItemAmount - 1; i++)
        {
            Items[i].SetNewResourceHolder(Slots[i]);
            Items[i] = Items[i + 1];
        }
        Items[ItemAmount - 2].SetNewResourceHolder(Slots[ItemAmount - 1]);
        Items[ItemAmount - 1] = null;
        ItemAmount--;
    }

    public virtual void RemoveItem(IResourceHolder sender, ResourceType resourceType) //removes Item by ResourceType
    {
        if(resourceType == ResourceType.Empty)
            throw new System.Exception("resourceType == ResourceType.Empty");

         if(resourceType == ResourceType.Any)
        {
            RemoveItemByNumber(sender, ItemAmount - 1);
            return;
        }

        for(int i = ItemAmount - 1; i >= 0; i--)
        {
            if(Items[i].resourceType == resourceType)
            {
                RemoveItemByNumber(sender, i);
                return;
            }
        }
    }

    protected float CorrectCenter(int value, int number) 
    {
        return value + (IsResourceInCenter[number] ? 0.5f : 0f); 
    }

    protected Vector3 CorrectCenterVector(Vector3Int value)
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
        Vector3 resourceSize = defaultRotation * ResourceSize;
        Vector3 axisNumbers = CorrectCenterVector(numberCoords);
        return startPosition + new Vector3(axisNumbers.x * resourceSize.x, axisNumbers.y * resourceSize.y, axisNumbers.z * resourceSize.z);
    }
}
