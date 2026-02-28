using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector3 startingPosition;
    public Vector3 targetPosition;
    public Vector3 direction;
    public bool reach;

    private Vector3 velocity;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startingPosition = new Vector3(10.0f, 0.0f, 0.0f);
        targetPosition = new Vector3(-10.0f, 0.0f, 0.0f);
        transform.position = startingPosition;
        direction = (targetPosition - startingPosition).normalized;
        velocity =  direction * speed * Time.fixedDeltaTime;
        reach = false;




        
    }

    void FixedUpdate()
    {
        if(!reach){
            Debug.Log("Rotation (Quaternion): " + transform.rotation);
            rb.MovePosition(rb.position + velocity);
            if(Vector3.Distance(rb.position, targetPosition)<1.0f){
                direction = (startingPosition - targetPosition).normalized;
                velocity = direction * speed * Time.fixedDeltaTime; 

                Vector3 temp = startingPosition;
                startingPosition = targetPosition;
                targetPosition = temp;

                reach=true;
            }
        }else{
            
            float angle = 90f * Time.fixedDeltaTime;
            Quaternion delta = Quaternion.Euler(0f,angle,0f);
            rb.MoveRotation(rb.rotation * delta);
            float currentY = transform.eulerAngles.y;
            Debug.Log("currentY: " + currentY);
            if(Mathf.Abs(currentY - 180f)<1f  || Mathf.Abs(currentY - 360f)<2f){
                reach = false;
            }
        }

        


    }
}
