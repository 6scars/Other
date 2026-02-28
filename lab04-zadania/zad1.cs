using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
public class RandomCubesGenerator : MonoBehaviour
{
    List<Vector3> positions = new List<Vector3>();
    public float delay = 3.0f;
    int objectCounter = 0;
 
    // prefab kostki
    public GameObject block;
 
    // materiały do losowania
    public Material[] materials; 
 
    void Start()
    {
        // losowe pozycje
        List<int> pozycje_x = new List<int>(Enumerable.Range(0, 20).OrderBy(x => Guid.NewGuid()).Take(10));
        List<int> pozycje_z = new List<int>(Enumerable.Range(0, 20).OrderBy(x => Guid.NewGuid()).Take(10));
 
        for (int i = 0; i < 10; i++)
        {
            this.positions.Add(new Vector3(pozycje_x[i], 5, pozycje_z[i]));
        }
 
        foreach (Vector3 elem in positions)
        {
            Debug.Log(elem);
        }
 
        StartCoroutine(GenerujObiekt());
    }
 
    IEnumerator GenerujObiekt()
    {
        Debug.Log("Wywołano coroutine");
 
        foreach (Vector3 pos in positions)
        {
            // tworzymy obiekt
            GameObject newBlock = Instantiate(this.block, pos, Quaternion.identity);
 
            // losowy materiał
            if (materials != null && materials.Length > 0)
            {
                Material randomMat = materials[UnityEngine.Random.Range(0, materials.Length)];
                Renderer rend = newBlock.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material = randomMat;
                }
            }
 
            objectCounter++;
            yield return new WaitForSeconds(this.delay);
        }
 
        StopCoroutine(GenerujObiekt());
    }
}