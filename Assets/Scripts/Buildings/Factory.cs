using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class Factory : MonoBehaviour, IResourceMaker, IResourceGetter
{
    
    [SerializeField] private GettingStorage _gettingStorage;
    [SerializeField] private GivingStorage _givingStorage;

    public ResourceType InputResource = ResourceType.Wood;
    public int AmountOfInputResource = 1;
    public ResourceType OutputResource = ResourceType.Paper;
    public int AmountOfOutputResource = 1;

    public readonly float _resourceMakingTime = 3f;
    [SerializeField] private Transform _inputSlot;
    [SerializeField] private Transform _outputSlot;
    [SerializeField] private GameObject _outputResourcePrefab;

    private FactoryState _thisFactoryState = FactoryState.GettingStorageIsEmpty;
    public FactoryState ThisFactoryState
    {
        get => _thisFactoryState; 
        private set
        {
            _thisFactoryState = value;
            if(value == FactoryState.Making)
                OnStartMaking.Invoke();
            else 
                OnStopMaking.Invoke();
        }
    }

    public UnityEvent OnStartMaking = new UnityEvent();
    public UnityEvent OnStopMaking = new UnityEvent();

    private float _startMakingTime;
    private bool _isMaking = false;

    ///<summary>
    ///You can call this method to try. You don't need always check all 
    ///</summary>
    public void StartMaking()
    {
        if(_isMaking)
            return;  

        if(_givingStorage.IsFull())
        {
            ThisFactoryState = FactoryState.GivingStorageIsFull;
            return;
        }

        if(_gettingStorage.IsEmpty())
        {
            ThisFactoryState = FactoryState.GettingStorageIsEmpty;
            return;
        }
        
        bool isSuccess = AddNewItems();
        
        if(!isSuccess)
        {
            ThisFactoryState = FactoryState.GettingStorageIsEmpty;
            return;
        }
        
        ThisFactoryState = FactoryState.Making;

        _isMaking = true;
        _startMakingTime = Time.time;
        
    }

    public void FinishMaking()
    {
        _isMaking = false;
        for(int i = 0; i < AmountOfOutputResource; i++)
        {
            Resource resource = (Instantiate(_outputResourcePrefab, _outputSlot) as GameObject).GetComponent<Resource>();
            _givingStorage.AddNewItem(resource);
        }
        StartMaking();
    }

    public bool AddNewItem(Resource resource, bool isInvokeEvents = true)
    {
        resource.OnSetInRightPlace.AddListener(() => Destroy(resource.gameObject));
        resource.SetNewSlot(_inputSlot);
        return true;
    }

    private bool AddNewItems()
    {

        List<int> indexes = new List<int>();
        for(int i = _gettingStorage.ItemAmount - 1; i >= 0; i--)
        {
            if(_gettingStorage[i] == InputResource)
            {
                indexes.Add(i);
                if(indexes.Count >= AmountOfInputResource)
                    break;
            }
        }

        if(indexes.Count < AmountOfInputResource)
            return false;

        foreach(int item in indexes)
        {
            _gettingStorage.RemoveItem(this, item);
        }

        return true;
    }

    private void Start()
    {
        if(AmountOfInputResource > _gettingStorage.FullMaxItemAmount)
            throw new System.Exception("Wrong input resource amount");
        if(AmountOfOutputResource > _givingStorage.FullMaxItemAmount)
            throw new System.Exception("Wrong output resource amount");
        if(_outputResourcePrefab.GetComponent<Resource>().resourceType != OutputResource)
            Debug.LogWarning("Wrong resource type of prefab");

        _startMakingTime = Time.time - _resourceMakingTime;
        _gettingStorage.storageEventManager.OnAddNewItem.AddListener(StartMaking);
        _givingStorage.storageEventManager.OnRemoveItem.AddListener(StartMaking);
        StartMaking();
    }

    private void Update()
    {
        if(_isMaking && Time.time - _startMakingTime > _resourceMakingTime)
            FinishMaking();
    }
}

public enum FactoryState
{
    Making,
    GettingStorageIsEmpty,
    GivingStorageIsFull
}
