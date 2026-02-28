using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpMultiplier = 3f; // mnożnik intensywności skoku


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("player enter the JumpPad");

            //Getting player object
            Example player = other.GetComponent<Example>();

            
            
            // Getting player jumpHeight value  ------- Multiply it by 3 
            player.movePlayerVelocity.y += jumpMultiplier * player.GetJumpHeight();
           
        }
    }
}
