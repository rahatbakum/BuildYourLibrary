using UnityEngine;


[RequireComponent (typeof(BoxCollider))]
public class GivingStorage : Storage
{

    public float GivingTimeInterval = 0.15f;

    private float _startTime;
    private float _lastGiveTime;

    protected override void Start()
    {
        base.Start();
        
        _startTime = Time.time;
        _lastGiveTime = _startTime;

        BoxCollider boxCollider = GetComponent<BoxCollider>();
        Collider[] colliders = Physics.OverlapBox(transform.position + boxCollider.center, boxCollider.size * 0.5f, transform.rotation);
        foreach(var collider in colliders)
        {
            if(collider.tag == "Resource"){
                AddNewItem(collider.GetComponent<Resource>());
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        if(Time.time - _lastGiveTime < GivingTimeInterval)
            return;

        if(other?.tag != "Player")
            return;
        
        PlayerStorage playerStorage = other.GetComponentInChildren<PlayerStorage>();
        if(playerStorage == null)
            return;
        
        if(playerStorage.IsFull())
            return;
        
        if(!IsHasResourceType(ResourceType.Any))
            return;
        
        RemoveItem(playerStorage as IResourceHolder, ResourceType.Any);

        _lastGiveTime = Time.time;
    }
}
