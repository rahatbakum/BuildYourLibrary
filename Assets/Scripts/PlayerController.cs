using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float speed = 8f;
    private Rigidbody _rigidbody;
    public PlayerStorage storage; 
    
    private PhysicsMovement _physicsMovement;

    public Vector3 worldRightDirection = new Vector3(1f, 0f, -1f);
    public Vector3 worldForwardDirection = new Vector3(1f, 0f, 1f);

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
        _physicsMovement = new PhysicsMovement(speed, _rigidbody, transform, worldRightDirection, worldForwardDirection); 
    }

    public void Move(Vector2 axisDirection){
        _physicsMovement.Move(axisDirection);
    }
}
