using UnityEngine;

public class zad02 : MonoBehaviour
{
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpening = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.rotation;
        openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));
    }

    void Update()
    {
        if (isOpening)
            transform.rotation = Quaternion.Slerp(transform.rotation, openRotation, Time.deltaTime * speed);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, closedRotation, Time.deltaTime * speed);
    }

    public void ToggleDoor()
    {
        isOpening = !isOpening;
    }
}
