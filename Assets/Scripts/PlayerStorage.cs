using UnityEngine;

public class PlayerStorage : Storage
{
    public override bool AddNewItem(Resource resource)
    {
        bool isSuccess = base.AddNewItem(resource);
        resource.IsAvailableToCatch = false;
        return isSuccess;
    }

    public override void RemoveItem(IResourceHolder sender, int number)
    {
        Items[number].IsAvailableToCatch = true;
        base.RemoveItem(sender, number);
    }

    protected override Vector3 CorrectCenterVector(Vector3Int value)
    {
        
        return new Vector3(value.x, CorrectCenter(value.y, 1), CorrectCenter(value.z, 2)); 
    }

}
