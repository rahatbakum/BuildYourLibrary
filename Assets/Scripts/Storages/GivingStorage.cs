using UnityEngine;


[RequireComponent (typeof(BoxCollider))]
public class GivingStorage : Storage
{

    [SerializeField] private float _givingTime = 0.15f;

    private float _lastGiveTime;

    protected override void Start()
    {
        base.Start();
        
        _lastGiveTime = Time.time - _givingTime;

        TryAddNearWildResources((Resource resource) => true);
    }


    void OnTriggerStay(Collider other)
    {
        if(Time.time - _lastGiveTime < _givingTime)
            return;

        if(other?.tag != "Player")
            return;
        
        PlayerStorage playerStorage = other.GetComponentInChildren<PlayerStorage>();
        if(playerStorage == null)
            return;
        
        if(playerStorage.IsFull())
            return;
        
        if(IsHasResourceType(ResourceType.Any) < 0)
            return;
        
        RemoveItem(playerStorage, ResourceType.Any);

        _lastGiveTime = Time.time;
    }
}
