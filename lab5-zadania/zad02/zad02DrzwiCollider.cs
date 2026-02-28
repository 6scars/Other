using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public zad02 door;  // przypisz obiekt drzwi w Inspectorze

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            door.ToggleDoor();
        }
    }

}
