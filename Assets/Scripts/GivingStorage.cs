using UnityEngine;

public class GivingStorage : Storage
{

    public override Vector3 PositionByNumber(int number)
    {
        Vector3 startPosition = transform.position + transform.right * StorageOffset.x + transform.up * StorageOffset.y + transform.forward * StorageOffset.z;
        int yNumber = Mathf.CeilToInt((float) number / MaxItemAmount.Item1 / MaxItemAmount.Item3);
        int layerNumber = number % (MaxItemAmount.Item1 * MaxItemAmount.Item3);
        int xNumber = layerNumber % MaxItemAmount.Item1;
        int zNumber = Mathf.CeilToInt((float) layerNumber / MaxItemAmount.Item1);

        return startPosition + (xNumber + 0.5f) * transform.right + (yNumber + 0.5f) * transform.up + (zNumber + 0.5f) * transform.forward;
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
