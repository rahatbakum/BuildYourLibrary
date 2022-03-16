using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Factory : Building
{
    
    [SerializeField] private GettingStorage gettingStorage;
    [SerializeField] private GivingStorage givingStorage;

    [SerializeField] private ResourceType _inputResource = ResourceType.Wood;
    [SerializeField] private int _amountOfInputResource = 1;
    [SerializeField] private ResourceType _outputResource = ResourceType.Paper;
    [SerializeField] private int _amountOfOutputResource = 1;

    [SerializeField] private float _resourceMakingTime = 3f;
    [SerializeField] private Transform _inputSlot;
    [SerializeField] private Transform _outputSlot;
    [SerializeField] private GameObject _outputResourcePrefab;

    private float _startMakingTime;
    private bool _isMaking = false;

    private void StartMaking()
    {
        _isMaking = true;
        _startMakingTime = Time.time;
    }

    private void FinishMaking()
    {
        _isMaking = false;
        for(int i = 0; i < _amountOfOutputResource; i++)
        {
            Resource resource = (Instantiate(_outputResourcePrefab, _outputSlot) as GameObject).GetComponent<Resource>();
            givingStorage.AddNewItem(resource);
        }
        _TryAddNewItem();
    }

    public override bool AddNewItem(Resource resource)
    {
        resource.OnSetInRightPlace.AddListener(() => Destroy(resource.gameObject));
        resource.SetNewSlot(_inputSlot);
        return true;
    }

    private void _TryAddNewItem()
    {

        if(_isMaking)
            return;


        if(givingStorage.IsFull())
        {
            Debug.Log("GivingStorage is full");
            return;
        }

        if(gettingStorage.ItemAmount <= 0)
        {
            Debug.Log("GettingStorage is empty");
            return;
        }

        List<int> indexes = new List<int>();
        for(int i = gettingStorage.ItemAmount - 1; i >= 0; i--)
        {
            if(gettingStorage[i] == _inputResource)
            {
                indexes.Add(i);
                if(indexes.Count >= _amountOfInputResource)
                    break;
            }
        }

        if(indexes.Count < _amountOfInputResource)
            return;

        StartMaking();
        foreach(int item in indexes)
        {
            gettingStorage.RemoveItem(this as IResourceHolder, item);
        }
    }

    private void Start()
    {
        if(_amountOfInputResource > gettingStorage.FullMaxItemAmount)
            throw new System.Exception("Wrong input resource amount");
        if(_amountOfOutputResource > givingStorage.FullMaxItemAmount)
            throw new System.Exception("Wrong output resource amount");
        if(_outputResourcePrefab.GetComponent<Resource>().resourceType != _outputResource)
            Debug.LogWarning("Wrong resource type of prefab");

        _startMakingTime = Time.time - _resourceMakingTime;
        gettingStorage.storageEventManager.OnAddNewItem.AddListener(_TryAddNewItem);
        givingStorage.storageEventManager.OnRemoveItem.AddListener(_TryAddNewItem);
        _TryAddNewItem();
    }

    private void Update()
    {
        if(_isMaking && Time.time - _startMakingTime > _resourceMakingTime)
            FinishMaking();
    }
}
