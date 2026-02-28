using UnityEngine;

using UnityEngine.InputSystem;
 
public class MoveWithCharacterController : MonoBehaviour

{

    private float playerSpeed = 5.0f;

    private float jumpHeight = 1.5f;

    private float gravityValue = -9.81f;
 
    private CharacterController controller;

    private Vector3 playerVelocity;

    private bool groundedPlayer;
 
    [Header("Input Actions")]

    public InputActionReference moveAction; // expects Vector2

    public InputActionReference jumpAction; // expects Button
 
    private void Awake()

    {

        controller = gameObject.GetComponent<CharacterController>();

    }
 
    private void OnEnable()

    {

        moveAction.action.Enable();

        jumpAction.action.Enable();

    }
 
    private void OnDisable()

    {

        moveAction.action.Disable();

        jumpAction.action.Disable();

    }
 
    void Update()

    {

        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)

        {

            playerVelocity.y = 0f;

        }
 
        // Read input

        Vector2 input = moveAction.action.ReadValue<Vector2>();
 
        // skladamy dwa wektory w jeden wektor ruchu

        Vector3 move = transform.right * input.x + transform.forward * input.y;

        move = Vector3.ClampMagnitude(move, 1f);
 
        // Jump

        if (jumpAction.action.triggered && groundedPlayer)

        {

            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);

        }
 
        // Apply gravity

        playerVelocity.y += gravityValue * Time.deltaTime;
 
        // Combine horizontal and vertical movement

        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);

        // warunek, aby nie wywoływać Move jeżeli nie ma ruchu

        if (finalMove != Vector3.zero)

        {

            controller.Move(finalMove * Time.deltaTime);

        }

    }

}
 