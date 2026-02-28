using UnityEngine;

public class zad05Scripts : MonoBehaviour
{


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player touched to the obstacle");
        }
    }
}
