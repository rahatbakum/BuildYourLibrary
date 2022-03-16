using UnityEngine;

public abstract class Building : MonoBehaviour, IResourceHolder
{
    public abstract bool AddNewItem(Resource resource);
}
