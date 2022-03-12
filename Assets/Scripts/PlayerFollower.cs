using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private Transform _player;
    public float speed = 8f;
    private Vector3 _startOffset;

    void Start()
    {
        _startOffset = transform.position - _player.position;
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _player.position + _startOffset, speed * Time.fixedDeltaTime);
    }
}
