using UnityEngine;
using UnityEngine.Events;

public class Resource : MonoBehaviour
{
    private const float AnimationSpeed = 10f;
    private const float AnimationRotationSpeed = 6f;
    private const float MinGoodDistance = 0.025f;
    public static readonly Vector3 ResourceSize = new Vector3(0.4f, 0.2f, 0.2f);
    public static readonly bool[] IsResourceInCenter = new bool[] {true, false, true};

    public bool IsAvailableToCatch = true;
    public ResourceType resourceType;

    private bool _isInRightPlace;
    private Transform _resourceBuffer;
    [HideInInspector] public Transform _slot;

    public UnityEvent OnSetInRightPlace = new UnityEvent();

    private void Awake()
    {
        _isInRightPlace = true;
    }

    private void Update()
    {
        if(!_isInRightPlace)
            MoveToSlot();
    }

    public void ForceSetNewSlot(Transform slot)
    {
         _isInRightPlace = false;
        _slot = slot;
        transform.SetParent(_slot);
    }

    public void SetNewSlot(Transform slot)
    {
        if(!IsAvailableToCatch)
            return;

        ForceSetNewSlot(slot);
    }

    private void SetInRightPlace()
    {
        transform.position = _slot.position;
        transform.rotation = _slot.rotation;
        _isInRightPlace = true;
        OnSetInRightPlace.Invoke();
    }

    private void MoveToSlot() //call this each frame
    {
        if (Vector3.Distance(transform.position, _slot.position) > MinGoodDistance)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, AnimationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _slot.rotation, AnimationRotationSpeed * Time.deltaTime);
        }
        else
        {
            SetInRightPlace();
        }
    }
}

public enum ResourceType
{
    Empty,
    Any,
    Wood,
    Paper
}
