using UnityEngine;

public class zad01 : MonoBehaviour
{
    private float distance = 10f;
    public float platformSpeed = 5f;


    private bool isRunning = false;
    private bool isRunningEndPoint = true;
    private bool isRunningStartPoint = false;




    private float startingPoint;
    private float endPoint;
   
   
    void Start()
    {
        startingPoint = transform.position.x;
        endPoint = transform.position.x  + distance;
    }

    // Update is called once per frame
    void Update()
    {

        if(isRunningEndPoint == true && transform.position.x >= endPoint)
        {
            isRunning = false;


        }else if(isRunningStartPoint == true && transform.position.x <= startingPoint)
        {
            isRunning = false;

        }

        if (isRunning)
        {
            Vector3 move = transform.right * platformSpeed * Time.deltaTime;
            transform.Translate(move);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player") && !isRunning)
        {
            Debug.Log("Player wszedł na platforme");
            if(transform.position.x <= endPoint)
            {
                Debug.Log("player idzie do endPoint");
                isRunningStartPoint = false;
                isRunningEndPoint = true;
                platformSpeed = Mathf.Abs(platformSpeed);
            }
            else if(transform.position.x >= startingPoint)
            {
                Debug.Log("player idzie do startingPoint");
                isRunningStartPoint = true;
                isRunningEndPoint = false;
                platformSpeed = -platformSpeed;
            }

            isRunning = true;
            
        }
    }
}
