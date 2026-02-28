using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MoveAndTurn90 : MonoBehaviour
{
    public float speed = 2f;
    public float rotatingSpeed = 90f;
    public float segmentDistance = 5f; 

    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 direction;
    private bool rotating;
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        startPos = rb.position;
        direction = transform.forward;
        rotating = false;
    }

    void FixedUpdate()
    {
        if(rotating){
            float step = rotatingSpeed * Time.fixedDeltaTime;
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, targetRotation, step));
            if(Quaternion.Angle(rb.rotation,targetRotation)<0.5f){
                rb.MoveRotation(targetRotation);
                rotating = false;
                direction = transform.forward;
                startPos = rb.position;


            }
            return;
        }
        float distance = Vector3.Distance(startPos, rb.position);

        if (distance >= segmentDistance)
        {
            rotating = true;
            targetRotation = rb.rotation * Quaternion.Euler(0f,90f,0f);
            return;
        }
        Vector3 next = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(next);
    }
}
