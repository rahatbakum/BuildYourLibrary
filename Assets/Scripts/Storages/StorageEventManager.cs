using UnityEngine.Events;

public class StorageEventManager
{
    public UnityEvent OnFull = new UnityEvent();
    public UnityEvent OnEmpty = new UnityEvent();
    public UnityEvent OnAddNewItem = new UnityEvent();
    public UnityEvent OnRemoveItem = new UnityEvent();

}
