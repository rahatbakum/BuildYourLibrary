using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float Speed = 8f;
    private Rigidbody _rigidbody;
    public PlayerStorage Storage; 
    
    private PhysicsMovement _physicsMovement;

    public Vector3 WorldRightDirection = new Vector3(1f, 0f, -1f);
    public Vector3 WorldForwardDirection = new Vector3(1f, 0f, 1f);

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _physicsMovement = new PhysicsMovement(Speed, _rigidbody, transform, WorldRightDirection, WorldForwardDirection); 
    }

    public void Move(Vector2 axisDirection){
        _physicsMovement.Move(axisDirection);
    }
}
