using UnityEngine.Events;
using UnityEngine;

public class StorageEventManager : MonoBehaviour
{
    public UnityEvent OnFull = new UnityEvent();
    public UnityEvent OnEmpty = new UnityEvent();
    public UnityEvent OnAddNewItem = new UnityEvent();
    public UnityEvent OnRemoveItem = new UnityEvent();

}
