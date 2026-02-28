// This first example shows how to move using Input System Package (New)

using UnityEngine;
using UnityEngine.InputSystem;

public class Example : MonoBehaviour
{
    private float playerSpeed = 8.0f;
    private float jumpHeight = 3.5f;
    private float gravityValue = -9.81f;
    public float pushPower = 10f;


    private CharacterController controller;
    public Vector3 movePlayerVelocity;
    private bool groundedPlayer;


    [Header("Input Actions")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    private void Awake(){
        controller = gameObject.GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        
    }

    private void OnDisable(){
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if(groundedPlayer && movePlayerVelocity.y <0){
            movePlayerVelocity.y = 0f;
        }


        //Read Input
        Vector2 input= moveAction.action.ReadValue<Vector2>();
        // Vector3 move = new Vector3(input.x, 0 , input.y);

        // transform right i forward sprawia, że zmieniamy kierunek lokalny
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        move = Vector3.ClampMagnitude(move, 1f);

        // if(move != Vector3.zero)
        // {
        //     transform.forward = move;
        // }



        //jump
        if(jumpAction.action.triggered){
            // jump formula
            movePlayerVelocity.y = Mathf.Sqrt(-2.0f * gravityValue * jumpHeight);

        }

        //gravity
        movePlayerVelocity.y += gravityValue * Time.deltaTime; 

        //movement
        Vector3 finalMove = (move * playerSpeed) + (movePlayerVelocity.y * Vector3.up);

        if(finalMove != Vector3.zero)
        {
            controller.Move(finalMove * Time.deltaTime);
        }


    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Debug.Log("Uderzyłem w: " + hit.collider.name);
        Rigidbody body = hit.collider.attachedRigidbody;
        // Debug.Log("Uderzyłem w: " + body);

        if(body == null || body.isKinematic)
        {
            return;
        }

        if(hit.moveDirection.y < -0.3)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.linearVelocity = pushDir * pushPower;



    }


    public float GetJumpHeight()
    {
        return jumpHeight;
    }


}
