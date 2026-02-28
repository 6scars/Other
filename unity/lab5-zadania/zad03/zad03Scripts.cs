using UnityEngine;

public class MovingPlatform_zad_03 : MonoBehaviour
{
    [Header("Waypoints (set in Inspector)")]
    public Transform[] waypoints;     // punkty docelowe
    public float speed = 2f;          // prędkość platformy
    public float stopDistance = 0.1f; // minimalna odległość, żeby "osiągnąć" waypoint

    private int currentIndex = 0;     // indeks aktualnego waypointa
    private int direction = 1;        // kierunek ruchu: 1 = do przodu, -1 = wstecz

    void Update()
    {
        if (waypoints.Length == 0) return; // brak waypointów → nic nie rób

        Transform target = waypoints[currentIndex];

        // Ruch platformy
        Vector3 moveDir = (target.position - transform.position).normalized;
        transform.position += moveDir * speed * Time.deltaTime;

        // Sprawdzenie, czy osiągnęliśmy waypoint
        if (Vector3.Distance(transform.position, target.position) < stopDistance)
        {
            // Przechodzimy do następnego waypointa
            currentIndex += direction;

            // Jeśli osiągnęliśmy koniec trasy → zmieniamy kierunek
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = waypoints.Length - 2;
                direction = -1;
            }
            else if (currentIndex < 0)
            {
                currentIndex = 1;
                direction = 1;
            }
        }
    }
}
