using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Pieces
    public GameObject[] pieces;
    
    // Start is called before the first frame update
    void Start()
    {
        SpawnNext();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNext()
    {
        // Random Index
        int i = Random.Range(0, this.pieces.Length);
        
        // Spawn Group at current Position
        Instantiate(this.pieces[i], this.transform.position, Quaternion.identity);
    }
}
