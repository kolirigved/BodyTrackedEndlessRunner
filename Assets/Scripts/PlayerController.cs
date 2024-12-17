using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int xMultiplier=1;
    public float jumpThreshold = 0.1f;
    public float initialWaistLevel = -0.1f;
    public float crouchThreshold = -0.25f;

    public Vector3 InitialplayerPosition;
    public float jumpMultiplier = 1;
    public MyListener listener;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponent<Transform>();
        playerTransform.position = InitialplayerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        playerTransform.position = InitialplayerPosition + new Vector3(listener.xpos*xMultiplier,0, 0);
        if(listener.ypos<crouchThreshold){
            Crouch();
        }
        if(listener.jumpstate-initialWaistLevel>jumpThreshold){
            Jump();
        }
        else playerTransform.position = new Vector3(playerTransform.position.x, InitialplayerPosition.y, playerTransform.position.z);
    }

    void Crouch(){
        Debug.Log("Crouching");
    }
    void Jump(){
        Debug.Log("Jumping");
        playerTransform.position = InitialplayerPosition + new Vector3(0,jumpMultiplier*(listener.jumpstate-initialWaistLevel), 0);
    }
}
