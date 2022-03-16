using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float Speed = 8f;
    private Rigidbody _rigidbody;
    [HideInInspector] public PlayerStorage _storage; 
    
    private PhysicsMovement _physicsMovement;

    public Vector3 WorldRightDirection = new Vector3(1f, 0f, -1f);
    public Vector3 WorldForwardDirection = new Vector3(1f, 0f, 1f);

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _storage = GetComponentInChildren<PlayerStorage>();
        _physicsMovement = new PhysicsMovement(Speed, _rigidbody, transform, WorldRightDirection, WorldForwardDirection); 
    }

    public void Move(Vector2 axisDirection){
        _physicsMovement.Move(axisDirection);
    }
}
