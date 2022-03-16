using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class House : Building
{
    [SerializeField] private GivingStorage givingStorage;
    [SerializeField] private ResourceType _outputResource = ResourceType.Wood;
    [SerializeField] private int _amountOfOutputResource = 1;

    [SerializeField] private float _resourceMakingTime = 1.5f;
    [SerializeField] private Transform _outputSlot;
    [SerializeField] private GameObject _outputResourcePrefab;

    private float _startMakingTime;
    private bool _isMaking = false;

    private void StartMaking()
    {
        if(_isMaking)
            return;

        if(givingStorage.IsFull())
        {
            Debug.Log("GivingStorage is full");
            return;
        }

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
        StartMaking();
    }

    public override bool AddNewItem(Resource resource)
    {
        Debug.LogWarning("House can't get Items");
        return false;
    }

    private void Start()
    {
        if(_amountOfOutputResource > givingStorage.FullMaxItemAmount)
            throw new System.Exception("Wrong output resource amount");
        if(_outputResourcePrefab.GetComponent<Resource>().resourceType != _outputResource)
            Debug.LogWarning("Wrong resource type of prefab");

        _startMakingTime = Time.time - _resourceMakingTime;
        givingStorage.storageEventManager.OnRemoveItem.AddListener(StartMaking);
        StartMaking();
    }

    private void Update()
    {
        if(_isMaking && Time.time - _startMakingTime > _resourceMakingTime)
            FinishMaking();
    }
}
