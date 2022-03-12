using UnityEngine;

public class PhysicsMovement
{
    private float _speed;
    private Rigidbody _rigidbody;
    private Transform _transform;
    private Vector3 _worldRight;
    private Vector3 _worldForward;

    public PhysicsMovement(float speed, Rigidbody rigidbody, Transform transform, Vector3 worldRightDirection, Vector3 worldForwardDirection)
    {
        _speed = speed;
        _rigidbody = rigidbody;
        _transform = transform;
        _worldRight = worldRightDirection.normalized;
        _worldForward = worldForwardDirection.normalized;
    }

    public void Move(Vector2 axisDirection){
        Vector3 worldDirection = _worldRight * axisDirection.x + _worldForward * axisDirection.y;
        Vector3 offset = worldDirection * _speed * Time.deltaTime;
        _rigidbody.MovePosition(_transform.position +  offset);
        _transform.LookAt (_transform.position + offset);
    }
}
