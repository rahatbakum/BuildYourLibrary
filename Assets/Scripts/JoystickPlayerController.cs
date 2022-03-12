using UnityEngine;


///<summary>
///Controls Player by Joystick
///</summrary>
[RequireComponent (typeof(Joystick))]
public class JoystickPlayerController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    private Joystick _joystick;

    void Start()
    {
        _joystick = GetComponent<Joystick>();
    }
    
    void Update()
    {
        _playerController.Move(new Vector2(_joystick.Horizontal, _joystick.Vertical));
    }
}
