using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [Tooltip ("If null the Destroyer will destroy this gameObject")]

    [SerializeField] private GameObject _destroyingObject;

    public void Destroy()
    {
        if(_destroyingObject == null)
            Destroy(gameObject);
        else
            Destroy(_destroyingObject);
    } 

}
