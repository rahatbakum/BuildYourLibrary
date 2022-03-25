using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class Library : MonoBehaviour, IResourceGetter
{
    [SerializeField] private LibraryAnimator _libraryAnimator;
    [SerializeField] private GettingStorage _gettingStorage;
    public ResourceType[] InputResources = new ResourceType[] {ResourceType.Wood, ResourceType.Paper};
    public int[] AmountOfInputResources = new int[] {12, 6};
    public int AmountOfBlocks = 12;
    public float _resourceMakingTime = 0.5f;
    [SerializeField] private Transform _inputSlot;
    [SerializeField] private string _nextLevelName = "Main";

    public UnityEvent OnStartMaking = new UnityEvent();
    public UnityEvent OnStopMaking = new UnityEvent();

    private bool _isMaking = false;
    private float _startMakingTime;
    private int _madeBlocksCount;

    ///<summary>
    ///You can call this method to try. You don't need always check all 
    ///</summary>
    public void StartMaking()
    {
        if(_isMaking)
            return;  

        if(_gettingStorage.IsEmpty())
        {
            OnStopMaking.Invoke();
            return;
        }

        bool isSuccess = AddNewItems();
        
        if(!isSuccess)
        {
            OnStopMaking.Invoke();
            return;
        }

        OnStartMaking.Invoke();
        _isMaking = true;
        _startMakingTime = Time.time;
        _libraryAnimator?.StartMaking();
    }

    public void FinishMaking()
    {
        _isMaking = false;
        _libraryAnimator?.FinishMaking();
        _madeBlocksCount++;
        if(_madeBlocksCount >= AmountOfBlocks)
            Win();
        StartMaking();
    }

    private void Win()
    {
        SceneManager.LoadScene(_nextLevelName);
    }

    private int FullAmountOfInputResources()
    {
        int sum = 0;
        for(int i = 0; i < AmountOfInputResources.Length; i++)
        {
            sum += AmountOfInputResources[i];
        }

        return sum;
    }

    public bool AddNewItem(Resource resource, bool isInvokeEvents = true)
    {
        resource.OnSetInRightPlace.AddListener(() => Destroy(resource.gameObject));
        resource.SetNewSlot(_inputSlot);
        return true;
    }

    private int[] MakeResourceMap()
    {
        int resourceTypeAmount = Enum.GetNames(typeof(ResourceType)).Length;
        int[] resourceMap = new int[resourceTypeAmount];
        for(int i = 0; i < AmountOfInputResources.Length; i++)
        {
            resourceMap[(int) InputResources[i]] = AmountOfInputResources[i];
        }

        return resourceMap;
    }

    private bool AddNewItems()
    {
        List<int> indexes = new List<int>();

        int[] resourceMap = MakeResourceMap(); 
        int amountOfInputResources = FullAmountOfInputResources();
        
        for(int i = _gettingStorage.ItemAmount - 1; i >= 0; i--)
        {
            if(resourceMap[(int) _gettingStorage[i]] > 0)
            {
                indexes.Add(i);
                resourceMap[(int) _gettingStorage[i]]--;
                if(indexes.Count >= amountOfInputResources)
                    break;
            }
        }

        if(indexes.Count < amountOfInputResources)
            return false;

        foreach(int item in indexes)
        {
            _gettingStorage.RemoveItem(this, item);
        }

        return true;
    }

    private void Start()
    {
        _startMakingTime = Time.time - _resourceMakingTime;
        _gettingStorage.storageEventManager.OnAddNewItem.AddListener(StartMaking);
        StartMaking();
        _madeBlocksCount = 0;
    }

    private void Update()
    {
        if(_isMaking && Time.time - _startMakingTime > _resourceMakingTime)
            FinishMaking();
    }

}
