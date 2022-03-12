using UnityEngine;

public class Resource : MonoBehaviour
{
    const float AnimationSpeed = 8f;
    const float MinGoodDistance = 0.025f;

    public bool IsAvailableToCatch = true;
    public ResourceType resourceType;

    private Vector3 _targetPosition;
    private bool _isInRightPlace;
    private Transform _resourceBuffer;
    private Transform _parent;

    void Awake()
    {
        ResourceBuffer rb = FindObjectOfType<ResourceBuffer>();
        _resourceBuffer = rb.transform;
        _isInRightPlace = true;
        _targetPosition = transform.position;
        _parent = _resourceBuffer;
        transform.SetParent(_parent);
    }

    void Update()
    {
        MoveToTargetPosition();
    }

    public void SetNewResourceHolder ( Transform parent, Vector3 targetPosition)
    {
        if(!IsAvailableToCatch)
            return;

        _targetPosition = targetPosition;
        _isInRightPlace = false;
        _parent = parent;
        transform.SetParent(_resourceBuffer);
    }
    void SetInRightPlace()
    {
        transform.position = _targetPosition;
        _isInRightPlace = true;
        transform.SetParent(_parent);
    }

    void MoveToTargetPosition()
    {
        if(_isInRightPlace)
            return;

        if (Vector3.Distance(transform.position, _targetPosition) > MinGoodDistance)
            transform.position = Vector3.Lerp(transform.position, _targetPosition, AnimationSpeed * Time.deltaTime);
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
