using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed = 8f;
    private Vector3 _startOffset;

    private void Start()
    {
        _startOffset = transform.position - _player.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _player.position + _startOffset, _speed * Time.fixedDeltaTime);
    }
}
