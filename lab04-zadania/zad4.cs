using UnityEngine;
using UnityEngine.InputSystem;
 
public class LookAround : MonoBehaviour
{
    [Header("References")]
    public Transform player;
 
    [Header("Settings")]
    public float mouseSensitivity = 100f;
 
    [Header("Input Actions")]
    public InputActionReference lookAction; // oczekuje Vector2 (Mouse Delta)
 
    private float xRotation = 0f;
    private Vector2 lookInput;
 
    void OnEnable()
    {
        lookAction.action.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    void OnDisable()
    {
        lookAction.action.Disable();
    }
 
    void Update()
    {
        lookInput = lookAction.action.ReadValue<Vector2>();
 
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = -lookInput.y * mouseSensitivity * Time.deltaTime;
 
        // Obrót gracza (lewo/prawo)
        player.Rotate(Vector3.up * mouseX);
 
        // Obrót kamery (góra/dół)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}