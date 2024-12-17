using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Destroyer")
        Destroy(gameObject);
        if(collision.gameObject.tag == "Player")
        {
            
            Debug.Log("Game Over");
            Destroy(gameObject);
        }

    }
    

    
}
