using UnityEngine;

public class Factory : Building
{
    
    public GettingStorage gettingStorage;
    public GivingStorage givingStorage;

    public ResourceType InputResource = ResourceType.Any;
    public int AmountOfInputResource = 1;
    public ResourceType OutputResource = ResourceType.Wood;
    public int AmountOfOutputResource = 1;

    public float ResourceMakingTime = 3f;

    void Start()
    {
        if(AmountOfInputResource > gettingStorage.GetMaxItemAmount())
            throw new System.Exception("Wrong input resource amount");
        if(AmountOfOutputResource > givingStorage.GetMaxItemAmount())
            throw new System.Exception("Wrong output resource amount");

        
    }
}
