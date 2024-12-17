using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstactleSystem : MonoBehaviour
{
    public GameObject obstactle;
    public float speed = 10f;
    public float spawnTime = 3f;
    public float z;

    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
        Transform t = GetComponent<Transform>();
        z = t.position.z;     
    }
    void Spawn()
    {// random float and not integerr
        float x = Random.Range(-0.8f, 0.9f);
        float y = Random.Range(0.7f, 2.1f);
        Vector3 spawnPosition = new Vector3(x, y, z);
        GameObject obstactleClone = Instantiate(obstactle, spawnPosition, Quaternion.identity);
        Rigidbody rb = obstactleClone.GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, -speed);
    }
}
