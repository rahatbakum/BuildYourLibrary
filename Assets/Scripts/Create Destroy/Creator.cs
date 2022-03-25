using UnityEngine;

public class Creator : MonoBehaviour
{
    [SerializeField] private GameObject _creationPrefab;
    [SerializeField] private ActionAfterCreating _actionAfterCreating = ActionAfterCreating.DestroyComponent;

    public void Create()
    {
        Instantiate(_creationPrefab, transform.position, transform.rotation, transform.parent);
        switch(_actionAfterCreating)
        {
            case ActionAfterCreating.DestroyComponent:
                Destroy(this);
                break;
            case ActionAfterCreating.DestroyGameObject:
                Destroy(gameObject);
                break;
        }
    }
}

public enum ActionAfterCreating
{
    Nothing,
    DestroyComponent,
    DestroyGameObject
}
