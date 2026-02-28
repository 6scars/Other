using UnityEngine;
using System.Collections.Generic; // potrzebne do listy zajętych pozycji

public class CubeSpawner : MonoBehaviour
{
    public GameObject cubePrefab;      // przypisz prefab Cube
    public int cubeCount = 10;         // ile sześcianów
    public float planeSize = 10f;      // rozmiar płaszczyzny

    private List<Vector3> usedPositions = new List<Vector3>(); // zapisuje zajęte pozycje

    void Start()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            Vector3 pos = GetUniqueRandomPosition();
            Instantiate(cubePrefab, pos, Quaternion.identity);
        }
    }

    Vector3 GetUniqueRandomPosition()
    {
        Vector3 pos;
        int safety = 0; // zabezpieczenie przed nieskończoną pętlą

        do
        {
            // generujemy losową pozycję na płaszczyźnie
            float x = Random.Range(-planeSize / 2f, planeSize / 2f);
            float z = Random.Range(-planeSize / 2f, planeSize / 2f);
            pos = new Vector3(x, 2f, z); // y=0.5 żeby Cube leżał na płaszczyźnie

            safety++;
            if (safety > 100) break;

        } while (IsPositionUsed(pos));

        usedPositions.Add(pos);
        return pos;
    }

    bool IsPositionUsed(Vector3 pos)
    {
        foreach (var used in usedPositions)
        {
            // jeśli dwa punkty są zbyt blisko, uznajemy że zajęte
            if (Vector3.Distance(used, pos) < 1f)
                return true;
        }
        return false;
    }
}
