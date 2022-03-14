using UnityEngine;

public class PlayerStorage : Storage
{
    public override void AddNewItem(Resource resource)
    {
        base.AddNewItem(resource);
        resource.IsAvailableToCatch = false;
    }

    public override void RemoveItemByNumber(IResourceHolder sender, int number)
    {
        Items[number].IsAvailableToCatch = true;
        base.RemoveItemByNumber(sender, number);
    }

}
