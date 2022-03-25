using UnityEngine;


///<summary>
///Controls Player by Joystick
///</summrary>
[RequireComponent (typeof(Joystick))]
public class JoystickPlayerController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    private Joystick _joystick;

    private void Start()
    {
        _joystick = GetComponent<Joystick>();
    }
    
    private void FixedUpdate()
    {
        if(_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            _playerController.Move(new Vector2(_joystick.Horizontal, _joystick.Vertical));
    }
}
