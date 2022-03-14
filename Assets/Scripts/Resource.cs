using UnityEngine;

public class Resource : MonoBehaviour
{
    const float AnimationSpeed = 10f;
    const float AnimationRotationSpeed = 5f;
    const float MinGoodDistance = 0.025f;

    public bool IsAvailableToCatch = true;
    public ResourceType resourceType;

    private bool _isInRightPlace;
    private Transform _resourceBuffer;
    private Transform _slot;

    void Awake()
    {
        ResourceBuffer rb = FindObjectOfType<ResourceBuffer>();
        _resourceBuffer = rb.transform;
        _isInRightPlace = true;
        _slot = _resourceBuffer;
        transform.SetParent(_slot);
    }

    void Update()
    {
        MoveToSlot();
    }

    public void SetNewResourceHolder (Transform slot)
    {
        if(!IsAvailableToCatch)
            return;

        _isInRightPlace = false;
        _slot = slot;
        transform.SetParent(_slot);
    }
    void SetInRightPlace()
    {
        transform.position = _slot.position;
        transform.rotation = _slot.rotation;
        _isInRightPlace = true;
    }

    void MoveToSlot()
    {
        if(_isInRightPlace)
            return;

        if (Vector3.Distance(transform.position, _slot.position) > MinGoodDistance)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, AnimationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _slot.rotation, AnimationRotationSpeed * Time.deltaTime);
        }
        else {
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
